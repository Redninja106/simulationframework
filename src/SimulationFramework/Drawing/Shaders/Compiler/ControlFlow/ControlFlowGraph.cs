﻿using SimulationFramework.Drawing.Shaders.Compiler.ILDisassembler;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Shaders.Compiler.ControlFlow;

internal class ControlFlowGraph
{
    public ControlFlowNode EntryNode { get; set; }
    public ControlFlowNode ExitNode { get; set; }
    public MethodDisassembly Disassembly { get; set; }

    public ControlFlowNode Parent { get; set; }

    public readonly List<BasicBlockNode> BasicBlocks = new();
    public readonly List<ControlFlowNode> Nodes = new();

    public ControlFlowGraph(MethodDisassembly disassembly)
    {
        Parent = null;
        EntryNode = new BasicBlockNode(this);
        ExitNode = new BasicBlockNode(this);
        AddNode(EntryNode);
        AddNode(ExitNode);

        Disassembly = disassembly;
        EntryNode.AddSuccessor(GetBasicBlock(disassembly.instructions[0]));

        RecomputeDominators();
        DgmlBuilder.WriteDGML(disassembly.Method.DeclaringType.Name + "_" + disassembly.Method.Name, this);
        ReplaceLoops();
        ReplaceConditionals();
    }

    private ControlFlowGraph(ControlFlowNode parent, ControlFlowNode entryNode, ControlFlowNode exitNode, IEnumerable<ControlFlowNode> nodes)
    {
        Parent = parent;
        EntryNode = entryNode;
        ExitNode = exitNode;

        foreach (var node in nodes)
        {
            if (node == node.Graph.EntryNode)
                node.Graph.EntryNode = parent;

            if (node == node.Graph.ExitNode)
                node.Graph.ExitNode = parent;

            node.Graph.RemoveNode(node);
            node.Graph = this;
        }

        this.Nodes.AddRange(nodes);
        RecomputeDominators();
    }

    public ControlFlowGraph GetRootGraph()
    {
        var graph = this;

        while (graph.Parent is not null)
        {
            graph = graph.Parent.Graph;
        }

        return graph;
    }

    public static ControlFlowGraph CreateConditionalSubgraph(ControlFlowNode parent, ControlFlowNode entryNode, ControlFlowNode exitNode)
    {
        exitNode.Graph.RecomputeDominators();

        Debug.Assert(exitNode.immediateDominator == entryNode);
        Debug.Assert(entryNode.Graph == exitNode.Graph);

        entryNode.Graph.RecomputeDominators();
        var nodes = entryNode.Graph.GetNodesBetween(entryNode, exitNode);
        var graph = new ControlFlowGraph(parent, entryNode, exitNode, nodes);

        graph.EntryNode = entryNode;
        graph.ExitNode = exitNode;

        return graph;
    }

    public BasicBlockNode GetBasicBlock(Instruction startInstruction)
    {
        var block = BasicBlocks.SingleOrDefault(br => br.Instructions.Contains(startInstruction));

        if (block is null)
        {
            block = new BasicBlockNode(this);
            AddNode(block);

            var stream = new InstructionStream(Disassembly);
            stream.Position = startInstruction.Location;

            AddNextBlocks(block, stream);
        }

        return block;
    }

    private void AddNextBlocks(BasicBlockNode node, InstructionStream instructions)
    {
        while (!instructions.IsAtEnd)
        {
            var instruction = instructions.Peek();

            if (node.Instructions.Count > 0 && instruction.IsBranchTarget())
            {
                node.AddSuccessor(GetBasicBlock(instruction));
                break;
            }

            node.Instructions.Add(instructions.Read());

            var branchBehavior = instruction.OpCode.GetBranchBehavior();
            if (branchBehavior is BranchBehavior.Branch or BranchBehavior.BranchOrContinue)
            {
                var target = instruction.GetBranchTarget();
                Debug.Assert(target is not null);
                node.AddSuccessor(GetBasicBlock(target));

                if (branchBehavior is BranchBehavior.BranchOrContinue)
                {
                    node.AddSuccessor(GetBasicBlock(instructions.Peek()));
                }

                break;
            }

            if (branchBehavior is BranchBehavior.Return)
            {
                node.AddSuccessor(ExitNode);
                break;
            }
        }
    }

    public void RecomputeDominators()
    {
        foreach (var node in this.Nodes)
        {
            node.dominators.Clear();

            if (node == EntryNode)
            {
                node.dominators.Add(node);
            }
            else
            {
                node.dominators.AddRange(this.Nodes);
            }
        }

        bool changed;
        do
        {
            changed = false;
            foreach (var node in this.Nodes)
            {
                if (node == EntryNode)
                    continue;

                foreach (var pred in node.Predecessors)
                {
                    var intersect = node.dominators.Intersect(pred.dominators).Append(node).Distinct().ToArray();

                    if (!node.dominators.SequenceEqual(intersect))
                    {
                        changed = true;
                        node.dominators.Clear();
                        node.dominators.AddRange(intersect);
                    }
                }
            }
        }
        while (changed);

        // immediate dominators
        foreach (var node in Nodes)
        {
            node.immediateDominator = null;
            if (node == EntryNode)
                continue;

            foreach (var dominator in node.dominators)
            {
                if (node == dominator)
                    continue;

                if (node.immediateDominator is null)
                {
                    node.immediateDominator = dominator;
                    continue;
                }

                if (dominator.dominators.Contains(node.immediateDominator))
                {
                    node.immediateDominator = dominator;
                }
            }
        }
    }

    Loop CreateLoop(BasicBlockNode header, BasicBlockNode tail)
    {
        Stack<BasicBlockNode> stack = new();
        Loop loop = new();

        var nodes = new List<ControlFlowNode> { header };

        if (header != tail)
        {
            nodes.Add(tail);
            stack.Push(tail);
        }

        while (stack.Any())
        {
            var block = stack.Pop();
            foreach (var pred in block.Predecessors)
            {
                if (!nodes.Contains(pred))
                {
                    nodes.Add(pred as BasicBlockNode);
                    stack.Push(pred as BasicBlockNode);
                }
            }
        }

        loop = new Loop
        {
            header = header,
            tail = tail,
            subgraph = new(null, header, tail, nodes)
        };

        return loop;
    }

    List<Loop> ComputeLoops()
    {
        List<Loop> loops = new();

        foreach (var block in BasicBlocks)
        {
            if (block == EntryNode)
                continue;

            foreach (var succ in block.Successors)
            {
                // Every successor that dominates its predecessor
                // is a loop header

                if (block.dominators.Contains(succ))
                    loops.Add(CreateLoop(succ as BasicBlockNode, block));
            }
        }

        return loops;
    }

    public void RemoveNode(ControlFlowNode node)
    {
        Debug.Assert(Nodes.Remove(node));
    }

    public void DepthFirstTraverse(Action<ControlFlowNode> visitNode)
    {
        DepthFirstTraverse(visitNode, EntryNode);
    }

    public void DepthFirstTraverse(Action<ControlFlowNode> visitNode, ControlFlowNode startNode)
    {
        _ = DepthFirstSearch(node =>
        {
            visitNode(node);
            return false;
        }, startNode);
    }

    public List<ControlFlowNode> GetNodesBetween(ControlFlowNode top, ControlFlowNode bottom)
    {
        List<ControlFlowNode> nodes = Nodes.Where(node => node.dominators.Contains(top) && !node.dominators.Contains(bottom)).ToList();
        nodes.Add(bottom);
        return nodes;
    }

    public ControlFlowNode DepthFirstSearch(Predicate<ControlFlowNode> predicate, bool searchSubgraphs = false)
    {
        return DepthFirstSearch(predicate, EntryNode, searchSubgraphs);
    }

    public ControlFlowNode DepthFirstSearch(Predicate<ControlFlowNode> predicate, ControlFlowNode startNode, bool searchSubgraphs = false)
    {
        HashSet<ControlFlowNode> visited = new();
        return DepthFirstSearchHelper(predicate, startNode, visited, searchSubgraphs);

        static ControlFlowNode DepthFirstSearchHelper(Predicate<ControlFlowNode> predicate, ControlFlowNode current, HashSet<ControlFlowNode> visited, bool searchSubgraphs)
        {
            if (predicate(current))
                return current;

            visited.Add(current);

            foreach (var successor in current.Successors)
            {
                if (visited.Contains(successor))
                    continue;

                ControlFlowNode result = DepthFirstSearchHelper(predicate, successor, visited, searchSubgraphs);

                if (result is not null)
                    return result;
            }

            if (searchSubgraphs && current is ISubgraphContainer subgraphContainer)
            {
                subgraphContainer.Subgraph.DepthFirstSearch(predicate, subgraphContainer.Subgraph.EntryNode, searchSubgraphs);
            }

            return null;
        }
    }

    public void BreadthFirstTraverse(Action<ControlFlowNode> visitNode, bool searchSubgraphs = false)
    {
        _ = BreadthFirstSearch(node =>
        {
            visitNode(node);
            return false;
        });
    }

    public ControlFlowNode BreadthFirstSearch(Predicate<ControlFlowNode> predicate, bool searchSubgraphs = false)
    {
        return BreadthFirstSearch(predicate, EntryNode, searchSubgraphs);
    }

    public ControlFlowNode BreadthFirstSearch(Predicate<ControlFlowNode> predicate, ControlFlowNode startNode, bool searchSubgraphs = false)
    {
        Queue<ControlFlowNode> workingQueue = new();
        HashSet<ControlFlowNode> visited = new();

        workingQueue.Enqueue(startNode);
        while (workingQueue.TryDequeue(out ControlFlowNode node))
        {
            if (predicate(node))
                return node;

            foreach (var successor in node.Successors)
            {
                if (visited.Contains(successor))
                    continue;

                visited.Add(successor);
                workingQueue.Enqueue(successor);
            }

            if (searchSubgraphs && node is ISubgraphContainer subgraphContainer)
            {
                workingQueue.Enqueue(subgraphContainer.Subgraph.EntryNode);
                visited.Add(subgraphContainer.Subgraph.EntryNode);
            }
        }

        return null;
    }


    public void AddNode(ControlFlowNode node)
    {
        Nodes.Add(node);

        if (node is BasicBlockNode basicBlock)
            BasicBlocks.Add(basicBlock);
    }

    public void ReplaceLoops()
    {
        var loops = ComputeLoops();

        foreach (var loop in loops)
        {
            AddNode(new LoopNode(this, loop));
            RecomputeDominators();
        }
    }


    void ReplaceConditionals()
    {
        // a conditional is any node with two successors
        // there are no loops at this point
        for (int i = 0; i < Nodes.Count; i++)
        {
            var node = Nodes[i];

            if (node == EntryNode)
                continue;

            if (node.Successors.Count is 2)
            {
                AddNode(new ConditionalNode(this, node));
            }
        }

        for (int i = 0; i < Nodes.Count; i++)
        {
            var node = Nodes[i];

            if (node is ISubgraphContainer container)
            {
                container.Subgraph.ReplaceConditionals();
            }
        }

        DepthFirstTraverse(node => (node as ConditionalNode)?.FixConnections());
    }
}
