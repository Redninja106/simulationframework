﻿using ImGuiNET;
using SimulationFramework.Drawing;
using SimulationFramework.Messaging;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Xml.Linq;

namespace SimulationFramework.ImGuiNET;

public sealed class ImGuiComponent : IApplicationComponent
{
    // https://github.com/mellinoe/ImGui.NET/blob/master/src/ImGui.NET.SampleProgram.XNA/ImGuiRenderer.cs

    private readonly List<ITexture<Color>> loadedTextures = new();
    private IBuffer<ImDrawVert>? vertexBuffer;
    private IBuffer<uint>? indexBuffer;
    private ITexture<Color>? fontTexture;
    private List<int> keys = new List<int>();
    private IRenderer renderer;

    public void Initialize(Application application)
    {
        renderer = Graphics.CreateRenderer();

        application.Dispatcher.Subscribe<RenderMessage>(BeforeRender, ListenerPriority.Before);
        application.Dispatcher.Subscribe<RenderMessage>(AfterRender, ListenerPriority.Low);

        var context = ImGui.CreateContext();
        ImGui.SetCurrentContext(context);

        var io = ImGui.GetIO();

        io.Fonts.AddFontDefault();
        io.BackendFlags |= ImGuiBackendFlags.RendererHasVtxOffset;

        RecreateFontDeviceTexture();
    }

    public void BeforeRender(RenderMessage message)
    {
        var io = ImGui.GetIO();
        io.DisplaySize = new(Graphics.DefaultRenderTarget.Width, Graphics.DefaultRenderTarget.Width);
        io.DeltaTime = Time.DeltaTime;
        UpdateInput();
        ImGui.NewFrame();
    }
    private unsafe void RecreateFontDeviceTexture() 
    { 
        ImGuiIOPtr io = ImGui.GetIO(); 
        IntPtr pixels; 
        int width, height, bytesPerPixel;
        io.Fonts.GetTexDataAsRGBA32(out pixels, out width, out height, out bytesPerPixel);
        fontTexture = Graphics.CreateTexture(width, height, new Span<Color>(pixels.ToPointer(), width * height));
        loadedTextures.Add(fontTexture);
        io.Fonts.SetTexID(new(0));
    }

    void UpdateInput()
    {
        var io = ImGui.GetIO();

        foreach (var c in Keyboard.GetChars())
        {
            if (c != '\t')
                io.AddInputCharacter(c);
        }

        for (int i = 0; i < keys.Count; i++)
        {
            io.KeysDown[keys[i]] = Keyboard.IsKeyDown((Key)keys[i]);
        }

        io.KeyShift = Keyboard.IsKeyDown(Key.LShift) || Keyboard.IsKeyDown(Key.RShift);
        io.KeyCtrl = Keyboard.IsKeyDown(Key.LCtrl) || Keyboard.IsKeyDown(Key.RCtrl);
        io.KeyAlt = Keyboard.IsKeyDown(Key.LAlt) || Keyboard.IsKeyDown(Key.RAlt);
        io.KeySuper = Keyboard.IsKeyDown(Key.LMeta) || Keyboard.IsKeyDown(Key.RMeta);

        io.DisplayFramebufferScale = new System.Numerics.Vector2(1f, 1f);

        io.MousePos = new Vector2(Mouse.Position.X, Mouse.Position.Y);

        io.MouseDown[0] = Mouse.IsButtonDown(MouseButton.Left);
        io.MouseDown[1] = Mouse.IsButtonDown(MouseButton.Right);
        io.MouseDown[2] = Mouse.IsButtonDown(MouseButton.Middle);

        var scrollDelta = Mouse.ScrollWheelDelta;
        io.MouseWheel = scrollDelta > 0 ? 1 : scrollDelta < 0 ? -1 : 0;
    }

    void SetupInput()
    {
        var io = ImGui.GetIO();

        keys.Add(io.KeyMap[(int)ImGuiKey.Tab] = (int)Key.Tab);
        keys.Add(io.KeyMap[(int)ImGuiKey.LeftArrow] = (int)Key.LeftArrow);
        keys.Add(io.KeyMap[(int)ImGuiKey.RightArrow] = (int)Key.RightArrow);
        keys.Add(io.KeyMap[(int)ImGuiKey.UpArrow] = (int)Key.UpArrow);
        keys.Add(io.KeyMap[(int)ImGuiKey.DownArrow] = (int)Key.DownArrow);
        keys.Add(io.KeyMap[(int)ImGuiKey.PageUp] = (int)Key.PageUp);
        keys.Add(io.KeyMap[(int)ImGuiKey.PageDown] = (int)Key.PageDown);
        keys.Add(io.KeyMap[(int)ImGuiKey.Home] = (int)Key.Home);
        keys.Add(io.KeyMap[(int)ImGuiKey.End] = (int)Key.End);
        keys.Add(io.KeyMap[(int)ImGuiKey.Delete] = (int)Key.Delete);
        keys.Add(io.KeyMap[(int)ImGuiKey.Backspace] = (int)Key.Backspace);
        keys.Add(io.KeyMap[(int)ImGuiKey.Enter] = (int)Key.Enter);
        keys.Add(io.KeyMap[(int)ImGuiKey.Escape] = (int)Key.Esc);
        keys.Add(io.KeyMap[(int)ImGuiKey.Space] = (int)Key.Space);
        keys.Add(io.KeyMap[(int)ImGuiKey.A] = (int)Key.A);
        keys.Add(io.KeyMap[(int)ImGuiKey.C] = (int)Key.C);
        keys.Add(io.KeyMap[(int)ImGuiKey.V] = (int)Key.V);
        keys.Add(io.KeyMap[(int)ImGuiKey.X] = (int)Key.X);
        keys.Add(io.KeyMap[(int)ImGuiKey.Y] = (int)Key.Y);
        keys.Add(io.KeyMap[(int)ImGuiKey.Z] = (int)Key.Z);
    }

    public void AfterRender(RenderMessage message)
    {
        ImGui.Render();

        RenderDrawData(ImGui.GetDrawData());
    }

    public void RenderDrawData(ImDrawDataPtr drawData)
    {
        renderer.PushState();

        renderer.RenderTarget = Graphics.DefaultRenderTarget;
        renderer.SetViewport(new(0, 0, renderer.RenderTarget.Width, renderer.RenderTarget.Height));

        if (drawData.TotalVtxCount > 0)
        {
            UpdateBuffers(drawData);
            RenderCommandLists(renderer, drawData);
        }

        renderer.PopState();
    }

    private void UpdateBuffers(ImDrawDataPtr drawData)
    {
        if (vertexBuffer is null || vertexBuffer.Length < drawData.TotalVtxCount)
        {
            vertexBuffer = Graphics.CreateBuffer<ImDrawVert>((int)(drawData.TotalVtxCount * 1.5f));
        }

        if (indexBuffer is null || indexBuffer.Length < drawData.TotalIdxCount)
        {
            indexBuffer = Graphics.CreateBuffer<uint>((int)(drawData.TotalIdxCount * 1.5f));
        }

        int vertexOffset = 0, indexOffset = 0;

        for (int i = 0; i < drawData.CmdListsCount; i++)
        {
            var cmdList = drawData.CmdListsRange[i];

            unsafe
            {
                var vertexData = new Span<ImDrawVert>(cmdList.VtxBuffer.Data.ToPointer(), cmdList.VtxBuffer.Size);
                vertexData.CopyTo(vertexBuffer.Data[vertexOffset..]);
                vertexOffset += vertexData.Length;

                var indexData = new Span<ushort>(cmdList.IdxBuffer.Data.ToPointer(), cmdList.IdxBuffer.Size);

                for (int j = 0; j < indexData.Length; j++)
                {
                    // widen to 32 bit indices
                    indexBuffer.Data[j] = indexData[j];
                }

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

        renderer.RenderTarget = Graphics.DefaultRenderTarget;
        renderer.SetVertexBuffer(vertexBuffer);
        renderer.CullMode = CullMode.None;

        ImGuiVertexShader vertexShader = new()
        {
            ProjectionMatrix = Matrix4x4.CreateOrthographicOffCenter(0f, renderer.RenderTarget.Width, renderer.RenderTarget.Height, 0, -1, 1),
        };

        renderer.SetVertexShader(vertexShader);

        int vertexOffset = 0, indexOffset = 0;

        renderer.SetIndexBuffer(indexBuffer);
        renderer.SetVertexBuffer(vertexBuffer);

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
                    //texture = loadedTextures[command.TextureId.ToInt32()]
                };

                renderer.SetFragmentShader(fragmentShader);


                renderer.DrawIndexedPrimitives(
                    PrimitiveKind.Triangles, 
                    (int)command.ElemCount, 
                    indexOffset + (int)command.IdxOffset,
                    vertexOffset + (int)command.VtxOffset);
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