using SharpGen.Runtime;
using System;
using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Vortice.Direct3D11;
using Vortice.DXGI;

namespace SimulationFramework.Drawing.Direct3D11;

internal sealed class Buffer<T> : D3D11Object, 
    IBindableResource,
    IResourceProvider<ID3D11UnorderedAccessView>,
    IResourceProvider<ID3D11ShaderResourceView>, 
    IResourceProvider<ID3D11Buffer>, 
    IBuffer<T> 
    where T : unmanaged
{
    public int SizeInBytes => Length * Stride;
    public int Stride => Unsafe.SizeOf<T>();

    public ResourceOptions Options { get; }
    public int Length { get; }

    private T[]? data;
    private (uint min, uint max)? dirtyRange;

    private readonly EventWaitHandle waitHandle;

    private ID3D11Fence fence;
    
    private ID3D11Buffer? internalBuffer;
    private ID3D11Buffer? internalStructuredBuffer;

    private ID3D11UnorderedAccessView? unorderedAccessView;
    private ID3D11ShaderResourceView? shaderResourceView;

    private BindFlags bindFlags;

    private ulong lastRead;
    private ulong cpuVersion;
    private ulong gpuVersion;

    public T this[int index]
    {
        get
        {
            if (index < 0)
                throw new IndexOutOfRangeException();


            return this[(uint)index];
        }
        set
        {
            if (index < 0)
                throw new IndexOutOfRangeException();


            this[(uint)index] = value;
        }
    }

    public T this[uint index]
    {
        get
        {
            if (index >= Length)
                throw new IndexOutOfRangeException();

            RequireCPUData();

            waitHandle.WaitOne();
            lastRead = Performance.FrameCount;
            return data[index];
        }
        set
        {
            if (index >= Length)
                throw new IndexOutOfRangeException();

            RequireCPUData();

            waitHandle.WaitOne();

            data[index] = value;

            cpuVersion++;

            if (dirtyRange is null)
            {
                dirtyRange = (index, index);
            }
            else
            {
                dirtyRange = (
                    Math.Min(dirtyRange.Value.min, index),
                    Math.Max(dirtyRange.Value.max, index)
                    );
            }
        }
    }

    public Buffer(DeviceResources deviceResources, int length, ResourceOptions options) : base(deviceResources)
    {
        Length = length;
        Options = options;

        waitHandle = new(true, EventResetMode.ManualReset);

        fence = deviceResources.Device.CreateFence(0);
    }

    [MemberNotNull(nameof(data))]
    public void RequireCPUData()
    {
        if (data is null)
        {
            data = ArrayPool<T>.Shared.Rent(this.Length);
            cpuVersion = 0;
        }
    }

    public override void Dispose()
    {
        if (data is not null)
        {
            ArrayPool<T>.Shared.Return(data);
            data = null;
        }

        internalBuffer?.Dispose();
        internalStructuredBuffer?.Dispose();
        unorderedAccessView?.Dispose();
        shaderResourceView?.Dispose();
        base.Dispose();
    }

    public ReadOnlySpan<T> GetData()
    {
        RequireCPUData();
        Synchronize();

        return this.data.AsSpan(0, this.Length);
    }

    public unsafe void Update(nint data, nint length, IGraphicsQueue? queue = null)
    {
        Update(new ReadOnlySpan<T>(data.ToPointer(), (int)length), 0, queue);
    }

    public void Update(T[] data, int offset = 0, IGraphicsQueue? queue = null)
    {
        Update(data.AsSpan(), offset, queue);
    }

    public unsafe void Update(ReadOnlySpan<T> data, int offset = 0, IGraphicsQueue? queue = null)
    {
        Synchronize();

        if (this.internalBuffer is null)
        {
            this.data ??= ArrayPool<T>.Shared.Rent(this.Length);

            data.CopyTo(this.data.AsSpan(offset));

            return;
        }

        var deviceContext = D3DUtils.GetQueueBase(queue ?? Graphics.ImmediateQueue);

        fixed (T* dataPtr = data) 
        {
            deviceContext.UpdateSubresource(new Span<T>(dataPtr, data.Length), this.internalBuffer, 0, 0, 0, new Vortice.Mathematics.Box(offset * Stride, 0, 0, (offset + data.Length) * Stride, 1, 1));
        }
    }

    private void Synchronize()
    {
        fence.SetEventOnCompletion(gpuVersion, waitHandle);
        waitHandle.WaitOne();

        if (gpuVersion < cpuVersion)
        {
            Update(this.data!);
            this.dirtyRange = null;

            if (Performance.FrameCount - lastRead >= 2)
            {
                ArrayPool<T>.Shared.Return(this.data!);
                this.data = null;
            }

            gpuVersion = cpuVersion;
        }

        if (this.gpuVersion > this.cpuVersion)
        {
            var deviceContext = D3DUtils.GetQueueBase(Graphics.ImmediateQueue);

            var mapped = deviceContext.Map(internalStructuredBuffer, MapMode.Read);

            var span = mapped.AsSpan<T>(Unsafe.SizeOf<T>() * this.Length);
            span.CopyTo(this.data);

            deviceContext.Unmap(internalStructuredBuffer);

            cpuVersion = gpuVersion;
        }
    }

    public void NotifyBound(GraphicsQueueBase queue, BindingUsage usage, bool mayWrite)
    {
        if (queue is not GraphicsQueueBase queueBase)
            throw new Exception();

        var bindFlag = D3DUtils.GetBindFlagsFromUsage(usage);

        if (this.bindFlags.HasFlag(bindFlag))
            return;

        this.bindFlags |= bindFlag;

        BufferDescription desc = new()
        {
            SizeInBytes = SizeInBytes,
            StructureByteStride = Stride,
            CpuAccessFlags = CpuAccessFlags.None,
            BindFlags = this.bindFlags & ~(BindFlags.UnorderedAccess | BindFlags.ShaderResource),
            OptionFlags = ResourceOptionFlags.None,
            Usage = ResourceUsage.Default,
        };

        BufferDescription structuredDesc = desc with
        {
            CpuAccessFlags = CpuAccessFlags.Read,
            OptionFlags = ResourceOptionFlags.BufferStructured,
            BindFlags = this.bindFlags & (BindFlags.UnorderedAccess | BindFlags.ShaderResource)
        };

        // if we don't have a cpu-side data copy and we have an existing buffer, use that
        if (data is null && this.internalBuffer is not null)
        {
            var newBuffer = Resources.Device.CreateBuffer(desc);
            var deviceContext = D3DUtils.GetQueueBase(Graphics.ImmediateQueue);
            deviceContext.CopyResource(newBuffer, this.internalBuffer);
            this.internalBuffer.Dispose();
            this.internalBuffer = newBuffer;
        }
        else
        {
            this.shaderResourceView?.Dispose();
            this.shaderResourceView = null;

            this.unorderedAccessView?.Dispose();
            this.unorderedAccessView = null;

            this.internalBuffer?.Dispose();
            this.internalStructuredBuffer?.Dispose();

            unsafe
            {
                fixed (T* dataPtr = &data![0])
                {
                    this.internalBuffer = Resources.Device.CreateBuffer(desc, new SubresourceData(dataPtr));

                    if (this.bindFlags.HasFlag(BindFlags.UnorderedAccess) || this.bindFlags.HasFlag(BindFlags.ShaderResource))
                    {
                        this.internalStructuredBuffer = Resources.Device.CreateBuffer(structuredDesc, new SubresourceData(dataPtr));
                    }
                }
            }
            gpuVersion = cpuVersion;
        }

        if (mayWrite)
            gpuVersion++;
    }

    public void GetResource(out ID3D11Buffer resource)
    {
        resource = this.internalBuffer;
    }

    public void GetResource(out ID3D11UnorderedAccessView resource)
    {
        if (unorderedAccessView is null)
        {
            var desc = new UnorderedAccessViewDescription()
            {
                Format = Format.Unknown,
                ViewDimension = UnorderedAccessViewDimension.Buffer,
                Buffer = new()
                {
                    NumElements = Length
                }
            };

            unorderedAccessView = Resources.Device.CreateUnorderedAccessView(this.internalStructuredBuffer, desc);
        }

        resource = this.unorderedAccessView;
    }

    public void GetResource(out ID3D11ShaderResourceView resource)
    {
        if (shaderResourceView is null)
        {
            var desc = new ShaderResourceViewDescription()
            {
                Format = Format.Unknown,
                ViewDimension = Vortice.Direct3D.ShaderResourceViewDimension.Buffer,
                Buffer = new BufferShaderResourceView()
                {
                    ElementWidth = (int)SizeInBytes / Length,
                }
            };

            shaderResourceView = Resources.Device.CreateShaderResourceView(this.internalStructuredBuffer, desc);
        }

        resource = this.shaderResourceView;
    }

    public void NotifyUnbound(GraphicsQueueBase queue)
    {
        queue.DeviceContext.Signal(this.fence, gpuVersion);
    }
}