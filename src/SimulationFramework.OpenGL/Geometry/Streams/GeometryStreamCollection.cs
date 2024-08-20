using SimulationFramework.Drawing;
using SimulationFramework.Drawing.Shaders.Compiler;
using SimulationFramework.OpenGL.Shaders;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SimulationFramework.OpenGL.Geometry.Streams;

class GeometryStreamCollection
{
    private readonly Dictionary<Type, CustomVertexGeometryStream> customVertexGeometryStreams = [];

    private readonly PositionGeometryStream positionGeometryStream;
    private readonly ColorGeometryStream colorGeometryStream;
    private readonly TextureGeometryStream textureGeometryStream;

    private readonly GLGraphics graphics;

    public GeometryStreamCollection(GLGraphics graphics)
    {
        this.graphics = graphics;

        positionGeometryStream = new PositionGeometryStream();
        colorGeometryStream = new ColorGeometryStream();
        textureGeometryStream = new TextureGeometryStream();
    }

    public GeometryStream GetStream(ref readonly CanvasState state)
    {
        if (state.Shader == null)
        {
            colorGeometryStream.Color = state.Color.ToColor();
            colorGeometryStream.TransformMatrix = state.Transform;
            return colorGeometryStream;
        }
        else
        {
            positionGeometryStream.TransformMatrix = state.Transform;
            return positionGeometryStream;
        }
    }

    internal CustomVertexGeometryStream GetCustomVertexGeometryStream(Type type, ref readonly CanvasState state)
    {
        if (!customVertexGeometryStreams.TryGetValue(type, out var stream))
        {
            var effect = graphics.GetShaderProgram(state.Shader, state.VertexShader);
            var vsCompilation = effect.vsCompilation;
            var vertexDataVar = vsCompilation!.Variables.Single(v => v.Kind is ShaderVariableKind.VertexData);

            stream = new CustomVertexGeometryStream(vertexDataVar.Type);
            customVertexGeometryStreams[type] = stream;
        }

        return stream;
    }

    public TextureGeometryStream GetTextureGeometryStream()
    {
        return textureGeometryStream;
    }
}
