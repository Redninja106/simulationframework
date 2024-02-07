using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace SimulationFramework.Drawing.Shaders.Compiler.ControlFlow;

// https://stackoverflow.com/questions/8199600/c-sharp-directed-graph-generating-library
public static class DgmlBuilder
{
    internal static void WriteDGML(string path, ControlFlowGraph graph)
    {
        List<Node> nodes = new();
        List<Link> links = new();
        List<Category> categories = new();
        List<Property> properties = new();

        categories.Add(new("BasicBlockNode", "Red", null));
        categories.Add(new("IdomEdge", null, "Gray"));
        categories.Add(new("Edge", null, "White"));
        categories.Add(new("Contains", null, null, "Contains", "False", "True", "Contained By", "True", "Contains"));

        properties.Add(new("Il", "Instructions", "System.String"));
        properties.Add(new("CanBeDataDriven", null, "System.Boolean"));
        properties.Add(new("CanLinkedNodesBeDataDriven", null, "System.Boolean"));
        properties.Add(new("Group", null, "Microsoft.VisualStudio.GraphModel.GraphGroupStyle"));
        properties.Add(new("UseManualLocation", null, "System.Boolean"));


        graph.DepthFirstTraverse(node => AddNode(node, null));

        void AddNode(ControlFlowNode node, ControlFlowNode parent)
        {
            var id = GetNodeID(node);
            nodes.Add(new(id, node.ToString(), node.GetType().Name, GetIl(node), GetGroup(node)));

            if (parent is not null)
                links.Add(new Link(GetNodeID(parent), id, "", "Contains"));

            foreach (var successor in node.Successors)
            {
                var successorId = GetNodeID(successor);
                links.Add(new(id, successorId, "Successor", "Edge"));
            }

            if (node is ISubgraphContainer container)
            {
                var subgraph = container.Subgraph;
                subgraph.DepthFirstTraverse(n => AddNode(n, node));

                var entryNodeId = GetNodeID(node) + "-ENTRY";
                nodes.Add(new(entryNodeId, "ENTRY"));
                links.Add(new(entryNodeId, GetNodeID(subgraph.EntryNode), "", "Edge"));
                links.Add(new(id, entryNodeId, "", "Contains"));

                var exitNodeId = GetNodeID(node) + "-EXIT";
                nodes.Add(new(exitNodeId, "EXIT"));
                links.Add(new(GetNodeID(subgraph.ExitNode), exitNodeId, "", "Edge"));
                links.Add(new(id, exitNodeId, "", "Contains"));
            }
        }

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
        // <Category Id = "Contains" Label = "Contains" Description = "Whether the source of the link contains the target object" CanBeDataDriven = "False" CanLinkedNodesBeDataDriven = "True" IncomingActionLabel = "Contained By" IsContainment = "True" OutgoingActionLabel = "Contains" />
    }

    private static string GetGroup(ControlFlowNode node)
    {
        return node is ISubgraphContainer ? "Expanded" : null;
    }

    private static string GetIl(ControlFlowNode node)
    {
        if (node is not BasicBlockNode basicBlock)
            return string.Empty;

        return basicBlock.Instructions.Aggregate(string.Empty, (a, b) => $"{a}\r\n{b}");
    }

    private static string GetNodeID(ControlFlowNode node)
    {
        return node.ToString().ToString();
    }

    public record struct Node(
        [property: XmlAttribute] string Id,
        [property: XmlAttribute] string Label,
        [property: XmlAttribute] string Category = null,
        [property: XmlAttribute] string Il = null,
        [property: XmlAttribute] string Group = null
        );

    public record struct Link(
        [property: XmlAttribute] string Source,
        [property: XmlAttribute] string Target,
        [property: XmlAttribute] string Label,
        [property: XmlAttribute] string Category
        );

    public record struct DgmlGraph(
        Node[] Nodes,
        Link[] Links,
        Category[] Categories,
        Property[] Properties
        );

    public record struct Category(
        [property: XmlAttribute] string Id,
        [property: XmlAttribute] string Background = null,
        [property: XmlAttribute] string Stroke = null,
        [property: XmlAttribute] string Label = null,
        [property: XmlAttribute] string CanBeDataDriven = null,
        [property: XmlAttribute] string CanLinkedNodesBeDataDriven = null,
        [property: XmlAttribute] string IncomingActionLabel = null,
        [property: XmlAttribute] string IsContainment = null,
        [property: XmlAttribute] string OutgoingActionLabel = null
        );

    public record struct Property(
        [property: XmlAttribute] string Id,
        [property: XmlAttribute] string Label,
        [property: XmlAttribute] string DataType
        );
}