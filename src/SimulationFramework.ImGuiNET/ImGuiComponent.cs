using ImGuiNET;
using SimulationFramework.Drawing;
using SimulationFramework.Messaging;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace SimulationFramework.ImGuiNET;

public sealed class ImGuiComponent : IAppComponent
{
    // https://github.com/mellinoe/ImGui.NET/blob/master/src/ImGui.NET.SampleProgram.XNA/ImGuiRenderer.cs

    private readonly List<ITexture> loadedTextures = new();
    private IBuffer<ImDrawVert>? vertexBuffer;
    private IBuffer<ushort>? indexBuffer;

    public void Initialize(Application application)
    {
        application.Dispatcher.Subscribe<RenderMessage>(BeforeRender, ListenerPriority.Before);
        application.Dispatcher.Subscribe<RenderMessage>(AfterRender, ListenerPriority.After);

        var context = ImGui.CreateContext();
        ImGui.SetCurrentContext(context);
    }

    public void BeforeRender(RenderMessage message)
    {
        var io = ImGui.GetIO();

        io.DeltaTime = Time.DeltaTime;

        ImGui.NewFrame();
    }

    public void AfterRender(RenderMessage message)
    {
        ImGui.Render();

        RenderDrawData(message.Renderer, ImGui.GetDrawData());
    }

    public void RenderDrawData(IRenderer renderer, ImDrawDataPtr drawData)
    {
        renderer.PushState();

        renderer.SetViewport(0, 0, renderer.RenderTarget.Width, renderer.RenderTarget.Height);

        UpdateBuffers(drawData);
        RenderCommandLists(renderer, drawData);

        renderer.PopState();
    }

    private void UpdateBuffers(ImDrawDataPtr drawData)
    {
        if (vertexBuffer is null || vertexBuffer.Length < drawData.TotalVtxCount)
        {
            vertexBuffer = Graphics.CreateBuffer<ImDrawVert>(drawData.TotalVtxCount);
        }

        if (indexBuffer is null || indexBuffer.Length < drawData.TotalVtxCount)
        {
            indexBuffer = Graphics.CreateBuffer<ushort>(drawData.TotalIdxCount);
        }

        int vertexOffset = 0, indexOffset = 0;

        for (int i = 0; i < drawData.CmdListsCount; i++)
        {
            var cmdList = drawData.CmdListsRange[i];

            unsafe
            {
                var vertexData = new Span<ImDrawVert>(cmdList.VtxBuffer.Data.ToPointer(), (int)(cmdList.VtxBuffer.Size * 1.5f));
                vertexData.CopyTo(vertexBuffer.Data[vertexOffset..]);
                vertexOffset += vertexData.Length;

                var indexData = new Span<ushort>(cmdList.IdxBuffer.Data.ToPointer(), (int)(cmdList.IdxBuffer.Size * 1.5f));
                indexData.CopyTo(indexBuffer.Data[indexOffset..]);
                indexOffset += indexData.Length;
            }
        }

        vertexBuffer.ApplyChanges();
        indexBuffer.ApplyChanges();
    }

    
    private void RenderCommandLists(IRenderer renderer, ImDrawDataPtr drawData)
    {
        if (vertexBuffer is null || indexBuffer is null)
            throw new InvalidOperationException();

        renderer.SetVertexBuffer(vertexBuffer);
        renderer.SetIndexBuffer(indexBuffer);

        ImGuiVertexShader vertexShader = new()
        {
            ProjectionMatrix = Matrix4x4.CreateOrthographicOffCenter(0f, renderer.RenderTarget.Width, renderer.RenderTarget.Height, 0, -1, 1),
        };

        renderer.SetVertexShader(vertexShader);

        int vertexOffset = 0, indexOffset = 0;

        for (int i = 0; i < drawData.CmdListsCount; i++)
        {
            var commandList = drawData.CmdListsRange[i];

            for (int j = 0; j < commandList.CmdBuffer.Size; j++)
            {
                var command = commandList.CmdBuffer[j];

                if (command.ElemCount is 0)
                    continue;

                var clipRect = Rectangle.CreateLTRB(command.ClipRect.X, command.ClipRect.Y, command.ClipRect.Z, command.ClipRect.W);
                renderer.Clip(clipRect);

                ImGuiFragmentShader fragmentShader = new()
                {
                    texture = loadedTextures[command.TextureId.ToInt32()]
                };

                renderer.SetFragmentShader(fragmentShader);

                renderer.DrawIndexedPrimitives(PrimitiveKind.Triangles, (int)command.ElemCount / 3, vertexOffset + (int)command.VtxOffset, indexOffset + (int)command.IdxOffset);
            }

            vertexOffset += commandList.VtxBuffer.Size;
            indexOffset += commandList.IdxBuffer.Size;
        }
    }

    public void Dispose()
    {
        vertexBuffer?.Dispose();
        indexBuffer?.Dispose();
    }
}