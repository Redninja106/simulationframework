using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimulationFramework.SFSL.Nodes;

namespace SimulationFramework.SFSL.Emit.HLSL;

internal class HlslEmitter : Emitter
{
    private const string EMITTED_PREFIX = "___";
    private int nextTextureId = 0;

    private List<(string type, string name)> cbufferMembers = new();

    public HlslEmitter(TextWriter writer) : base(writer)
    {
        Out.WriteLine($"struct {EMITTED_PREFIX}cbuffer;");
    }

    public override void Flush()
    {
        Out.WriteLine();
        Out.WriteLine($"struct {EMITTED_PREFIX}cbuffer : register(b0){Environment.NewLine}{{");
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
        Out.WriteLine($"Texture2D {node.NameToken.Value} : register(t{nextTextureId});");
        Out.WriteLine($"sampler {EMITTED_PREFIX}_{node.NameToken.Value}Sampler : register(s{nextTextureId});");

        nextTextureId++;

        base.Visit(node);
    }
}
