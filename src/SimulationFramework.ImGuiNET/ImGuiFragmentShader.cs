using SimulationFramework.Drawing;
using SimulationFramework.Shaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.ImGuiNET;
internal struct ImGuiFragmentShader : IShader
{
    public ITexture<ColorF> texture;

    [Input, OverrideSemantic("color", InputSemantic.Position)] 
    (Vector4 position, Vector2 uv, ColorF color) input;

    [Output(OutputSemantic.Color)]
    ColorF fragColor;

    public void Main()
    {
        fragColor = input.color;
        // color = input.color * texture.Sample(input.uv);
    }
}

class OverrideSemanticAttribute : Attribute { public OverrideSemanticAttribute(string field, InputSemantic semantic) { } }

//interface ISamplable<T> where T : ISamplable<T>
//{
//    static abstract T Lerp(T a, T b, float t);
//}

//static class SamplingExtensions
//{
//    public static T Sample<T>(this ITexture<T> texture, TextureSampler sampler)
//        where T : ISamplable<T>
//    {

//    }
//}

//struct ColorF : ISamplable<ColorF>
//{
//    public static ColorF Lerp(ColorF a, ColorF b, float t)
//    {
//    }
//}

//struct TextureSampler
//{
//}

//class UniformAttribute
//{

//}

//class OutputAttribute
//{
//    public string? LinkageName { get; set; } = null;

//    public OutputAttribute()
//    {

//    }

//    public OutputAttribute(OutputSemantic semantic)
//    {

//    }
//}


//class InputAttribute : Attribute
//{
//    public string? LinkageName { get; set; } = null;

//    public InputAttribute()
//    {

//    }

//    public InputAttribute(InputSemantic semantic)
//    {

//    }
//}


//class OverrideSemanticAttribute : Attribute
//{
//    public OverrideSemanticAttribute(string fieldName, InputSemantic semantic)
//    {

//    }

//    public OverrideSemanticAttribute(string fieldName, OutputSemantic semantic)
//    {

//    }
//}