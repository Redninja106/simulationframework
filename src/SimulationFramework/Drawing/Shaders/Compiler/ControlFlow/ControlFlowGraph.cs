﻿using SimulationFramework.Drawing.Shaders.Compiler.Disassembler;
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

internal class ControlFlowGraph : ControlFlowNode
{
    public ControlFlowNode EntryNode { get; set; }
    public ControlFlowNode ExitNode { get; set; }
    public MethodDisassembly Disassembly { get; set; }

    // public ControlFlowNode Parent { get; set; }

    public readonly List<BasicBlockNode> BasicBlocks = new();
    public readonly List<ControlFlowNode> Nodes = new();
    
    public SubgraphKind? SubgraphKind { get; set; }

    public ControlFlowGraph(MethodDisassembly disassembly)
    {
        EntryNode = new DummyNode();
        ExitNode = new DummyNode(true);
        AddNode(EntryNode);
        AddNode(ExitNode);

        Disassembly = disassembly;
        EntryNode.AddSuccessor(GetBasicBlock(disassembly.instructions[0]));

        RecomputeDominators();
        ReplaceLoops();
        ReplaceConditionals();

        if (ShaderCompiler.DumpShaders)
        {
            DgmlBuilder.WriteDGML(disassembly.Method.DeclaringType.Name + "_" + disassembly.Method.Name, this);
        }
    }

    private ControlFlowGraph(ControlFlowNode parent, ControlFlowNode entryNode, ControlFlowNode exitNode, IEnumerable<ControlFlowNode> nodes)
    {
        EntryNode = entryNode;
        ExitNode = exitNode;

        // foreach (var node in nodes)
        // {
        //     if (node == node.Graph.EntryNode)
        //         node.Graph.EntryNode = parent;
        // 
        //     if (node == node.Graph.ExitNode)
        //         node.Graph.ExitNode = parent;
        // 
        //     node.Graph.RemoveNode(node);
        //     node.Graph = this;
        // }

        this.Nodes.AddRange(nodes);
        RecomputeDominators();
    }

    private ControlFlowGraph()
    {

    }

    private ControlFlowGraph InsertSubgraph(HashSet<ControlFlowNode> nodes, SubgraphKind kind)
    {
        ControlFlowGraph subgraph = new()
        {
            SubgraphKind = kind,
        };

        DummyNode exitDummy = new ExitDummyNode(this);

        // all incoming and outgoing connections to a subgraph must go to the same node.
        ControlFlowNode? incomingNode = null;
        ControlFlowNode? outgoingNode = null;

        foreach (var node in nodes)
        {
            this.RemoveNode(node);
            subgraph.AddNode(node);

            foreach (var pred in node.Predecessors.ToArray())
            {
                if (!nodes.Contains(pred))
                {
                    incomingNode ??= node;
                    if (incomingNode != node)
                    {
                        throw new Exception("invalid subgraph!");
                    }
                    
                    pred.RemoveSuccessor(node);
                    pred.AddSuccessor(subgraph);
                }
            }

            foreach (var succ in node.Successors.ToArray())
            {
                if (!nodes.Contains(succ))
                {
                    outgoingNode ??= succ;
                    if (outgoingNode != succ)
                    {
                       throw new Exception("invalid subgraph!");
                    }

                    succ.RemovePredecessor(node);
                    succ.AddPredecessor(subgraph);
                    exitDummy.AddPredecessor(node);
                }
            }
        }
        subgraph.EntryNode = incomingNode;
        subgraph.ExitNode = exitDummy;
        subgraph.Nodes.Add(exitDummy);

        AddNode(subgraph);
        return subgraph;
    }

    public BasicBlockNode GetBasicBlock(Instruction startInstruction)
    {
        var block = BasicBlocks.SingleOrDefault(br => br.Instructions.Contains(startInstruction));

        if (block is null)
        {
            block = new BasicBlockNode();
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
                foreach (var n in this.Nodes)
                {
                    node.dominators.Add(n);
                }
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
                        
                        foreach (var i in intersect)
                        {
                            node.dominators.Add(i);
                        }
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
    public void RecomputePostDominators()
    {
        foreach (var node in this.Nodes)
        {
            node.postdominators.Clear();

            if (node == ExitNode)
            {
                node.postdominators.Add(node);
            }
            else
            {
                foreach (var n in this.Nodes)
                {
                    node.postdominators.Add(n);
                }
            }
        }

        bool changed;
        do
        {
            changed = false;
            foreach (var node in this.Nodes)
            {
                if (node == ExitNode)
                    continue;

                foreach (var succ in node.Successors)
                {
                    var intersect = node.postdominators.Intersect(succ.postdominators).Append(node).Distinct().ToArray();

                    if (!node.postdominators.SequenceEqual(intersect))
                    {
                        changed = true;
                        node.postdominators.Clear();

                        foreach (var i in intersect)
                        {
                            node.postdominators.Add(i);
                        }
                    }
                }
            }
        }
        while (changed);

        // immediate dominators
        foreach (var node in Nodes)
        {
            node.immediatePostDominator = null;
            if (node == ExitNode)
                continue;

            foreach (var postDominator in node.postdominators)
            {
                if (node == postDominator)
                    continue;

                if (node.immediatePostDominator is null)
                {
                    node.immediatePostDominator = postDominator;
                    continue;
                }

                if (postDominator.postdominators.Contains(node.immediatePostDominator))
                {
                    node.immediatePostDominator = postDominator;
                }
            }
        }
    }

    public void RemoveNode(ControlFlowNode node)
    {
        Nodes.Remove(node);
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

    public HashSet<ControlFlowNode> GetNodesBetween(ControlFlowNode top, ControlFlowNode bottom)
    {
        HashSet<ControlFlowNode> nodes = Nodes.Where(node => node.dominators.Contains(top) && !node.dominators.Contains(bottom)).ToHashSet();
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

            if (searchSubgraphs && current is ControlFlowGraph subgraph)
            {
                subgraph.DepthFirstSearch(predicate, subgraph.EntryNode, searchSubgraphs);
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

            if (searchSubgraphs && node is ControlFlowGraph subgraph)
            {
                workingQueue.Enqueue(subgraph.EntryNode);
                visited.Add(subgraph.EntryNode);
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
        RecomputeDominators();
        foreach (var block in BasicBlocks)
        {
            if (block == EntryNode)
                continue;

            foreach (var succ in block.Successors)
            {
                // Every successor that dominates its predecessor
                // is a loop header

                if (block.dominators.Contains(succ))
                {
                    HashSet<ControlFlowNode> nodes = FindNodesInLoop(succ, block);
                    InsertSubgraph(nodes, ControlFlow.SubgraphKind.Loop);
                }
            }
        }
    }

    private HashSet<ControlFlowNode> FindNodesInLoop(ControlFlowNode header, ControlFlowNode tail)
    {
        HashSet<ControlFlowNode> nodes = [header];
        Stack<ControlFlowNode> stack = [];

        if (header != tail)
        {
            nodes.Add(tail);
            stack.Push(tail);
        }

        while (stack.Count > 0)
        {
            var block = stack.Pop();
            foreach (var pred in block.Predecessors)
            {
                if (nodes.Add(pred))
                {
                    stack.Push(pred);
                }
            }
        }

        return nodes;
    }

    void ReplaceConditionals()
    {
        // a conditional is any node with two successors
        // there are no loops at this point
        List<ControlFlowNode> conditionNodes = [];
        for (int i = 0; i < Nodes.Count; i++)
        {
            var node = Nodes[i];

            if (node == EntryNode)
            {
                continue;
            }

            if (node.Successors.Count is 2)
            {
                conditionNodes.Add(node);
            }
        }

        foreach (var node in conditionNodes)
        {
            if (!this.Nodes.Contains(node))
                continue;

            RecomputePostDominators();

            var convergence = node.immediatePostDominator;
            Debug.Assert(convergence is not null);
            var nodes = GetNodesBetween(node, convergence);
            
            InsertSubgraph(nodes, ControlFlow.SubgraphKind.Conditional);
        }

        foreach (var node in Nodes)
        {
            if (node is ControlFlowGraph subgraph)
            {
                subgraph.ReplaceConditionals();
            }
        }
    }
}