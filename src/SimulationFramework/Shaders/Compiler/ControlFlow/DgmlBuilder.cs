using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace SimulationFramework.Shaders.Compiler.ControlFlow;

// https://stackoverflow.com/questions/8199600/c-sharp-directed-graph-generating-library
public static class DgmlBuilder
{
    internal static void WriteDGML(string path, ControlFlowGraph graph)
    {
        List<Node> nodes = new();
        List<Link> links = new();
        List<Category> categories = new();
        List<Property> properties = new();

        categories.Add(new("BasicBlockNode", "Red"));
        properties.Add(new("Il", "Instructions", "System.String"));

        graph.Traverse(node =>
        {
            var id = GetNodeID(node);
            nodes.Add(new(id, node.ToString(), node.GetType().Name, GetIl(node)));

            foreach (var successor in node.Successors)
            {
                var successorId = GetNodeID(successor);
                links.Add(new(id, successorId, "Successor"));
            }
        });

        DgmlGraph dgmlGraph = new(
            nodes.ToArray(), 
            links.ToArray(), 
            categories.ToArray(), 
            properties.ToArray()
            );

        XmlRootAttribute root = new("DirectedGraph")
        {
            Namespace = "http://schemas.microsoft.com/vs/2009/dgml"
        };

        XmlSerializer serializer = new(typeof(DgmlGraph), root);

        XmlWriterSettings settings = new()
        {
            Indent = true,
        };

        using XmlWriter xmlWriter = XmlWriter.Create(Path.GetFileNameWithoutExtension(path) + ".dgml", settings);
        serializer.Serialize(xmlWriter, dgmlGraph);
    }

    private static string GetIl(ControlFlowNode node)
    {
        if (node is not BasicBlockNode basicBlock)
            return string.Empty;

        return basicBlock.Instructions.Aggregate(string.Empty, (a, b) => $"{a}\r\n{b}");
    }

    private static string GetNodeID(ControlFlowNode node)
    {
        return node.GetHashCode().ToString();
    }

    public record struct Node(
        [property: XmlAttribute] string Id,
        [property: XmlAttribute] string Label,
        [property: XmlAttribute] string? Category = null,
        [property: XmlAttribute] string? Il = null
        );

    public record struct Link(
        [property: XmlAttribute] string Source,
        [property: XmlAttribute] string Target,
        [property: XmlAttribute] string Label
        );

    public record struct DgmlGraph(
        Node[] Nodes, 
        Link[] Links,
        Category[] Categories,
        Property[] Properties
        );

    public record struct Category(
        [property: XmlAttribute] string Id,
        [property: XmlAttribute] string Background
        );

    public record struct Property(
        [property: XmlAttribute] string Id,
        [property: XmlAttribute] string Label,
        [property: XmlAttribute] string DataType
        );
}