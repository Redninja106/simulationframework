using ImGuiNET;
using SimulationFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

internal class FreeCamera
{
    public Matrix4x4 ViewMatrix { get; set; }
    public Matrix4x4 ProjectionMatrix { get; set; }

    public ref Vector3 Position => ref position;
    public ref float Yaw => ref yaw;
    public ref float Pitch => ref pitch;
    public ref float MoveSpeed => ref moveSpeed;
    public ref float TurnSpeed => ref turnSpeed;
    public ref float FieldOfView => ref fieldOfView;
    public ref float AspectRatio => ref aspectRatio;

    private Vector3 position = Vector3.Zero;
    private float yaw = 0, pitch = 0;
    private float moveSpeed = 1, turnSpeed = 1;
    private float fieldOfView; // in degrees
    private float aspectRatio; // width/height
    private float nearPlane;
    private float farPlane;

    private bool isWindowOpen;

    public FreeCamera(float fieldOfView, float aspectRatio, float nearPlane = 0.01f, float farPlane = 100f)
    {
        this.fieldOfView = fieldOfView;
        this.aspectRatio = aspectRatio;
        this.nearPlane = nearPlane;
        this.farPlane = farPlane;
    }

    public void Update()
    {
        if (Mouse.IsButtonDown(MouseButton.Right))
        {
            yaw += Mouse.DeltaPosition.X * MathF.PI * 0.002f * turnSpeed;
            pitch -= Mouse.DeltaPosition.Y * MathF.PI * 0.002f * turnSpeed;
        }

        pitch = Math.Clamp(pitch, -MathF.PI/2, MathF.PI/2);

        Matrix4x4 rotationMatrix = Matrix4x4.CreateRotationX(Pitch) * Matrix4x4.CreateRotationY(Yaw);// Matrix4x4.CreateFromYawPitchRoll(Yaw, Pitch, 0);

        Vector3 delta = Vector3.Zero;

        if (Keyboard.IsKeyDown(Key.W)) 
            delta += Vector3.UnitZ;
        if (Keyboard.IsKeyDown(Key.A)) 
            delta += Vector3.UnitX;
        if (Keyboard.IsKeyDown(Key.S)) 
            delta -= Vector3.UnitZ;
        if (Keyboard.IsKeyDown(Key.D)) 
            delta -= Vector3.UnitX;
        if (Keyboard.IsKeyDown(Key.C)) 
            delta -= Vector3.UnitY;
        if (Keyboard.IsKeyDown(Key.Space)) 
            delta += Vector3.UnitY;

        float speed = 1;
        if (Keyboard.IsKeyDown(Key.LShift))
            speed *= 5;
        if (Keyboard.IsKeyDown(Key.LAlt))
            speed /= 5;

        Vector3 transformedDelta = Vector3.Transform(delta, rotationMatrix);
        this.Position += transformedDelta * Time.DeltaTime * MoveSpeed * speed;

        ViewMatrix = Matrix4x4.CreateLookAt(this.position, this.position + Vector3.Transform(Vector3.UnitZ, rotationMatrix), Vector3.Transform(Vector3.UnitY, rotationMatrix));

        fieldOfView = Math.Clamp(fieldOfView, 5f, 134f);
        nearPlane = MathF.Max(0.01f, this.nearPlane);

        farPlane = MathF.Max(nearPlane, farPlane);
        if (farPlane == nearPlane)
            farPlane += 0.01f;

        ProjectionMatrix = Matrix4x4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(this.fieldOfView), this.aspectRatio, this.nearPlane, this.farPlane);
    }

    public void Layout()
    {
        if (Keyboard.IsKeyPressed(Key.F1))
            isWindowOpen = !isWindowOpen;

        if (isWindowOpen && ImGui.Begin("Camera", ref isWindowOpen))
        {
            ImGui.SliderFloat("Field Of View", ref fieldOfView, 5f, 135f);
            ImGui.Text($"Aspect Ratio: {aspectRatio}");
            ImGui.DragFloatRange2("Near/Far planes", ref nearPlane, ref farPlane, 0.1f, 0.01f);

            ImGui.Separator();

            ImGui.DragFloat3("Position", ref position, 0.01f);
            ImGui.SliderAngle("Pitch", ref pitch, -90, 90);
            ImGui.SliderAngle("Yaw", ref yaw);

            ImGui.Separator();

            ImGui.SliderFloat("Move Speed", ref moveSpeed, 0f, 10f);
            ImGui.SliderFloat("Turn Speed", ref turnSpeed, 0f, 10f);
        }
        ImGui.End();
    }

    public Matrix4x4 GetTransformMatrix()
    {
        return ViewMatrix * ProjectionMatrix;   
    }
}
