using SimulationFramework.Drawing.Pipelines;
using System.Runtime.CompilerServices;
using Vortice.Direct3D11;

namespace SimulationFramework.Drawing.Direct3D11.Buffers;

internal sealed class D3D11Buffer<T> : IBuffer<T> where T : unmanaged
{
    public int Size { get; private set; }
    
    private int SizeInBytes => Size * Stride;
    public int Stride => Unsafe.SizeOf<T>();

    private DeviceResources resources;
    private ID3D11Buffer[] internalBuffers;
    private ResourceOptions options;

    public D3D11Buffer(DeviceResources resources, int size, ResourceOptions options)
    {
        this.resources = resources;
        this.Size = size;
        this.options = options;
        internalBuffers = new ID3D11Buffer[Enum.GetValues<BufferUsage>().Length];
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
        if (this.Size != data.Length)
        {
            this.Size = data.Length;
         
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
                resources.ImmediateRenderer.DeviceContext.UpdateSubresource(data, buffer);
        }
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
            resources.ImmediateRenderer.DeviceContext.CopyResource(newBuffer, firstNotNullBuffer);
        }

        internalBuffers[(int)usage] = newBuffer;
    }

    // a buffer is versatile and can be used in many different ways,
    // which is something direct3d is not a fan of.
    // we will need to create multiple copies of our internal buffer
    // with different descriptions in order to match every use case.
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
            _ => throw new NotImplementedException(),
        };
    }

    ~D3D11Buffer()
    {
        this.Dispose();
    }
}
