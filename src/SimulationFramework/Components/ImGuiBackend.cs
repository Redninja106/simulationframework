using ImGuiNET;
using SimulationFramework.Drawing;
using SimulationFramework.Drawing.Shaders;
using SimulationFramework.Input;
using SimulationFramework.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection.PortableExecutable;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Components;
#if FALSE

internal class ImGuiBackend : ISimulationComponent
{
    private nint context;

    public void Dispose()
    {
    }

    public void Initialize(MessageDispatcher dispatcher)
    {
        context = ImGui.CreateContext();
        ImGui.SetCurrentContext(context);

        dispatcher.Subscribe<BeforeRenderMessage>(PreRender);
        dispatcher.Subscribe<AfterRenderMessage>(AfterRender);


    }

    private void SetupInput()
    {
        ImGuiIOPtr io = ImGui.GetIO();

    }

    private void PreRender(BeforeRenderMessage message)
    {
        ImGui.NewFrame();

        var windowCanvas = Application.GetComponent<IGraphicsProvider>().GetWindowCanvas();
        ImGuiIOPtr io = ImGui.GetIO();
        io.DisplaySize = new(windowCanvas.Width, windowCanvas.Height);

        io.DeltaTime = Time.DeltaTime;

        io.MouseDown[0] = Mouse.IsButtonDown(MouseButton.Left);
        io.MouseDown[1] = Mouse.IsButtonDown(MouseButton.Right);
        io.MouseDown[2] = Mouse.IsButtonDown(MouseButton.Middle);

        var fixedResolutionInterceptor = Application.GetComponent<FixedResolutionInterceptor>();

        io.MousePos = fixedResolutionInterceptor.BaseMouseProvider.Position;

        io.MouseWheel += Mouse.ScrollWheelDelta;

        foreach (char c in Keyboard.TypedKeys)
        {
            io.AddInputCharacter(c);
        }

        io.KeyCtrl = Keyboard.IsKeyPressed(Key.LeftControl) || Keyboard.IsKeyPressed(Key.RightControl);
        io.KeyAlt = Keyboard.IsKeyPressed(Key.LeftAlt) || Keyboard.IsKeyPressed(Key.RightAlt);
        io.KeyShift = Keyboard.IsKeyPressed(Key.LeftShift) || Keyboard.IsKeyPressed(Key.RightShift);
        io.KeySuper = Keyboard.IsKeyPressed(Key.LeftMeta) || Keyboard.IsKeyPressed(Key.RightMeta);
    }


    private void AfterRender(AfterRenderMessage message)
    {

        // imGuiController.Update(Time.DeltaTime);

        // var io = ImGui.GetIO();
        // keyboardProvider.capturedByImgui = io.WantCaptureKeyboard;
        // mouseProvider.capturedByImgui = io.WantCaptureMouse;
    }

    ImGuiCanvasShader canvasShader;
    ImGuiVertexShader vertexShader;

    private unsafe void RenderDrawData(ICanvas canvas, ImDrawDataPtr drawData)
    {
        vertexShader.projection = Matrix4x4.CreateOrthographicOffCenter(0f, canvas.Width, canvas.Height, 0, -1, 1);
        canvas.Fill(canvasShader, vertexShader);

        for (int i = 0; i < drawData.CmdLists.Size; i++)
        {
            ImDrawListPtr commandList = drawData.CmdLists[i];

            Span<ImDrawVert> vertexBuffer = new(
                (void*)commandList.VtxBuffer.Data,
                commandList.VtxBuffer.Size
                );

            Span<ushort> indexBuffer = new(
                (void*)commandList.IdxBuffer.Data,
                commandList.IdxBuffer.Size
                );

            for (int j = 0; j < commandList.CmdBuffer.Size; j++)
            {
                ImDrawCmdPtr command = commandList.CmdBuffer[j];

                canvas.Clip(Rectangle.FromLTRB(command.ClipRect.X, command.ClipRect.Y, command.ClipRect.Z, command.ClipRect.W));

                canvasShader.texture = (ITexture)(object)command.GetTexID();

                Span<ImDrawVert> vertices = vertexBuffer[(int)command.VtxOffset..];
                Span<ushort> indices = indexBuffer.Slice((int)command.IdxOffset, (int)command.ElemCount);

                canvas.DrawIndexedTriangles<ImDrawVert>(vertices, indices);
            }
        }
    }

    private class ImGuiVertexShader : VertexShader
    {
        [VertexData]
        private ImDrawVert vertex;

        public Matrix4x4 projection;

        [VertexShaderOutput]
        private Vector2 uv;

        [VertexShaderOutput]
        private ColorF color;

        [UseClipSpace]
        public override Vector4 GetVertexPosition()
        {
            Vector4 position = new(vertex.pos, 0, 1);
            position = Vector4.Transform(position, projection);
            uv = vertex.uv;
            color = new Color(vertex.col).ToColorF();
            return position;
        }
    }

    private class ImGuiCanvasShader : CanvasShader
    {
        public ITexture texture;

        [VertexShaderOutput]
        private Vector2 uv;

        [VertexShaderOutput]
        private ColorF color;
        
        public override ColorF GetPixelColor(Vector2 position)
        {
            return color * texture.SampleUV(uv);
        }
    }

}
#endif