using SimulationFramework;
using SimulationFramework.Drawing;
using SimulationFramework.Drawing.Canvas;
using SimulationFramework.Drawing.Pipelines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Basic3D;
internal class Basic3DSimulation : Simulation
{
    IShader vertexShader, fragmentShader;
    IBuffer<Vector3> vertexBuffer;
    
    public override void OnInitialize(AppConfig config)
    {
        vertexShader = Graphics.CreateShader(ShaderKind.Vertex, @"
float4 main(float3 pos : position) : SV_Position
{
    return float4(pos, 1.0f);
}

");
        fragmentShader = Graphics.CreateShader(ShaderKind.Fragment, @"
float4 main(float4 pos : SV_Position) : SV_Target
{
    return pos;
}
");

        vertexBuffer = Graphics.CreateBuffer<Vector3>(3);
        vertexBuffer.SetData(new Vector3[]
        {
            new(-.5f,-.5f, 0),
            new(.5f, 0, 0),
            new(0, .5f, 0),
        });
    }

    public override void OnRender(ICanvas canvas)
    {
        var renderer = Graphics.GetRenderer();

        renderer.Clear(Color.Black);
        
        renderer.VertexBuffer(vertexBuffer);
        renderer.UseShader(vertexShader);
        renderer.UseShader(fragmentShader);

        renderer.DrawPrimitives(PrimitiveKind.Triangles, 1, 0);
    }
}
