using System.Runtime.CompilerServices;
using Vortice.Direct3D11;
using Vortice.DXGI;

namespace SimulationFramework.Drawing.Direct3D11.Buffers;

internal sealed class D3D11Buffer<T> : IUnorderedAccessViewProvider, IShaderResourceViewProvider, IBuffer<T> where T : unmanaged
{
    public int Length { get; private set; }
    private int SizeInBytes => Length * Stride;
    public int Stride => Unsafe.SizeOf<T>();
    public ResourceOptions Options => this.options;
    public Span<T> Data => data;

    private T[] data;
    private DeviceResources resources;
    private ID3D11Buffer[] internalBuffers;
    private ResourceOptions options;

    public LazyResource<ID3D11UnorderedAccessView> UnorderedAccessView { get; }
    public LazyResource<ID3D11ShaderResourceView> ShaderResourceView { get; }

    public D3D11Buffer(DeviceResources resources, int length, ResourceOptions options)
    {
        this.resources = resources;
        this.Length = length;
        this.options = options;

        internalBuffers = new ID3D11Buffer[Enum.GetValues<BufferUsage>().Length];
        UnorderedAccessView = new(resources, CreateUAV);
        ShaderResourceView = new(resources, CreateSRV);

        if (!options.HasFlag(ResourceOptions.Readonly))
        {
            data = new T[length];
        }
    }

    ID3D11UnorderedAccessView IUnorderedAccessViewProvider.GetUnorderedAccessView() => this.UnorderedAccessView.GetValue();

    private ID3D11UnorderedAccessView CreateUAV()
    {
        var desc = new UnorderedAccessViewDescription()
        {
            Format = Format.Unknown,
            ViewDimension = UnorderedAccessViewDimension.Buffer,
            Buffer = new() 
            { 
                NumElements = this.Length 
            }
        };

        var buffer = GetInternalbuffer(BufferUsage.UnorderedAccessResource);
        return resources.Device.CreateUnorderedAccessView(buffer, desc);
    }

    private ID3D11ShaderResourceView CreateSRV()
    {
        var desc = new ShaderResourceViewDescription()
        {
            Format = Vortice.DXGI.Format.Unknown,
            ViewDimension = Vortice.Direct3D.ShaderResourceViewDimension.Buffer,
            Buffer = new BufferShaderResourceView()
            {
                ElementWidth = this.SizeInBytes / this.Length,
            }
        };

        var buffer = GetInternalbuffer(BufferUsage.ShaderResource);
        return resources.Device.CreateShaderResourceView(buffer, desc);
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);

        foreach (var buffer in internalBuffers)
        {
            buffer?.Dispose();
        }
    }

    public void SetData(Span<T> data)
    {
        if (this.Length != data.Length)
        {
            this.Length = data.Length;

            // recreate all already existing buffers with the proper size.
            foreach (var usage in Enum.GetValues<BufferUsage>())
            {
                if (internalBuffers[(int)usage] != null)
                {
                    CreateInternalBuffer(usage);
                }
            }
        }

        // make sure there is at least one buffer to hold the data
        if (internalBuffers.All(b => b is null))
        {
            GetInternalbuffer(BufferUsage.Default);
        }

        foreach (var buffer in this.internalBuffers)
        {
            if (buffer is not null)
                resources.Device.ImmediateContext.UpdateSubresource(data, buffer);
        }
    }

    public T[] GetData()
    {
        var queue = Graphics.ImmediateQueue as D3D11QueueBase;

        var buffer = this.internalBuffers[(int)BufferUsage.UnorderedAccessResource];

        var mapped = queue.DeviceContext.Map(buffer, MapMode.Read);
        
        T[] result = mapped.AsSpan<T>(buffer).ToArray();

        queue.DeviceContext.Unmap(buffer);

        return result;
    }

    // makes sure the internal buffer of a certain usage is initialized and the proper size
    public ID3D11Buffer GetInternalbuffer(BufferUsage usage)
    {
        if (internalBuffers[(int)usage] is null)
        {
            CreateInternalBuffer(usage);
        }

        return internalBuffers[(int)usage];
    }

    private void CreateInternalBuffer(BufferUsage usage)
    {
        BufferDescription description = GetBufferDescriptionForUsage(usage);

        var newBuffer = resources.Device.CreateBuffer(description);

        var firstNotNullBuffer = internalBuffers.FirstOrDefault(b => b is not null && b != newBuffer);

        if (firstNotNullBuffer != null)
        {
            resources.Device.ImmediateContext.CopyResource(newBuffer, firstNotNullBuffer);
        }

        internalBuffers[(int)usage] = newBuffer;
    }

    private BufferDescription GetBufferDescriptionForUsage(BufferUsage usage)
    {
        BufferDescription baseDesc = new()
        {
            SizeInBytes = this.SizeInBytes,
            StructureByteStride = this.Stride,
            CpuAccessFlags = CpuAccessFlags.None,
            BindFlags = BindFlags.None,
            OptionFlags = ResourceOptionFlags.None,
            Usage = ResourceUsage.Default,
        };

        return usage switch
        {
            BufferUsage.Default => baseDesc,
            BufferUsage.VertexBuffer => baseDesc with { BindFlags = BindFlags.VertexBuffer },
            BufferUsage.ConstantBuffer => baseDesc with { BindFlags = BindFlags.ConstantBuffer },
            BufferUsage.IndexBuffer => baseDesc with { BindFlags = BindFlags.IndexBuffer },
            BufferUsage.ShaderResource => baseDesc with { BindFlags = BindFlags.ShaderResource, OptionFlags = ResourceOptionFlags.BufferStructured },
            BufferUsage.UnorderedAccessResource => baseDesc with 
            { 
                BindFlags = BindFlags.UnorderedAccess, 
                CpuAccessFlags = CpuAccessFlags.Read,
                OptionFlags = ResourceOptionFlags.BufferStructured,
            },
            _ => throw new NotImplementedException(),
        };
    }

    public void ApplyChanges()
    {
        SetData(this.Data);
    }

    public ID3D11ShaderResourceView GetShaderResourceView() => ShaderResourceView.GetValue();

    ~D3D11Buffer()
    {
        this.Dispose();
    }
}
