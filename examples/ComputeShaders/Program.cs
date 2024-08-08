using Silk.NET.Core.Native;
using SimulationFramework;
using SimulationFramework.Drawing;
using SimulationFramework.Drawing.Shaders;
using System.Numerics;

Start<Program>();

partial class Program : Simulation
{
    Particle[] particles;

    ParticleComputeShader particleShader;

    public override void OnInitialize()
    {
        particles = new Particle[100_000];
        for (int i = 0; i < particles.Length; i++)
        {
            particles[i].position = Random.Shared.NextVector2() * Window.Size;
            particles[i].velocity = Random.Shared.NextUnitVector2() * Random.Shared.NextSingle(50, 100);
        }
        particleShader = new()
        {
            particles = particles,
        };
    }

    public override void OnRender(ICanvas canvas)
    {
        canvas.Clear(Color.Black);
        particleShader.deltaTime = Time.DeltaTime;
        Window.Title = Performance.Framerate.ToString();
        Graphics.Dispatch(particleShader, particleShader.particles.Length, 1, 1);

        for (int i = 0; i < particles.Length; i++)
        {
            canvas.PushState();
            canvas.Translate(particles[i].position);
            canvas.DrawRect(0, 0, 10, 10, Alignment.Center);
            canvas.PopState();
        }
    }
}

struct Particle
{
    public Vector2 position;
    public Vector2 velocity;
}

class ParticleComputeShader : ComputeShader
{
    public Particle[] particles;
    public float deltaTime;

    public override void RunThread(int i, int j, int k)
    {
        particles[i].position += deltaTime * particles[i].velocity;
    }
}
