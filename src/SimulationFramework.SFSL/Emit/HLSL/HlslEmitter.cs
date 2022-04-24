using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFSLPrototype.Nodes;

namespace SFSLPrototype.Emit.HLSL;

internal class HlslEmitter : Emitter
{
    private const string EMITTED_PREFIX = "_e";
    private const string cbufferName = EMITTED_PREFIX + "_cbuffer";
    private int nextTextureId = 0;

    private List<(string type, string name)> cbufferMembers = new();

    public HlslEmitter(TextWriter writer) : base(writer)
    {
        Out.WriteLine($"struct {cbufferName};");
    }

    public override void Flush()
    {
        Out.WriteLine();
        Out.WriteLine($"struct {cbufferName} : register(b0){Environment.NewLine}{{");
        foreach (var member in cbufferMembers)
        {
            Out.WriteLine($"\t{member.type} {member.name};");
        }
        Out.WriteLine($"}};");
    }

    public override void Visit(VariableNode node)
    {
        switch (node.Modifier)
        {
            case VariableModifier.None:
                cbufferMembers.Add((node.Type.Value, node.Name.Value));
                break;
            case VariableModifier.In:
                break;
            case VariableModifier.Out:
                break;
            default:
                break;
        }

        base.Visit(node);
    }

    public override void Visit(TextureNode node)
    {
        // Texture2D Texture : register(t0);
        // sampler Sampler : register(s0);
        Out.WriteLine();
        Out.WriteLine($"Texture2D {EMITTED_PREFIX}_{node.NameToken.Value}_texture : register(t{nextTextureId});");
        Out.WriteLine($"sampler {EMITTED_PREFIX}_{node.NameToken.Value}_sampler : register(s{nextTextureId});");

        nextTextureId++;

        base.Visit(node);
    }
}
