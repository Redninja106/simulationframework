using SimulationFramework.Drawing.Shaders.Compiler.Disassembler;
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
        ExitNode = new DummyNode();
        AddNode(EntryNode);
        AddNode(ExitNode);

        Disassembly = disassembly;

        EntryNode.AddSuccessor(GetBasicBlock(disassembly.instructions[0]));

        DetectReturns(ExitNode);
        RecomputeDominators();
        ReplaceLoops();
        Dump();
        ReplaceConditionals();

        if (ShaderCompiler.DumpShaders)
        {
            Dump();
        }
    }

    [Conditional("DEBUG")]
    private void Dump()
    {
        if (Disassembly != null)
        {
            try
            {
                DgmlBuilder.WriteDGML(Disassembly.Method.DeclaringType!.Name + "_" + Disassembly.Method.Name, this);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString() + " while writing shader DGML file!");
            } 
        }
    }

    // subgraph constructor
    private ControlFlowGraph(MethodDisassembly disassembly, SubgraphKind subgraphKind)
    {
        this.Disassembly = disassembly;
        this.SubgraphKind = subgraphKind;
    }

    private ControlFlowGraph InsertSubgraph(HashSet<ControlFlowNode> nodes, SubgraphKind kind, ControlFlowNode? returnTarget)
    {
        ControlFlowGraph subgraph = new(this.Disassembly, kind);

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
                        throw new Exception($"invalid subgraph in method '{this.Disassembly.Method.DeclaringType}.{this.Disassembly.Method.Name}'!");
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
                        throw new Exception($"invalid subgraph in method '{this.Disassembly.Method.DeclaringType}.{this. Disassembly.Method.Name}'!");
                    }

                    succ.RemovePredecessor(node);
                    succ.AddPredecessor(subgraph);
                    exitDummy.AddPredecessor(node);
                }
            }
        }

        if (incomingNode == null && nodes.Contains(this.EntryNode))
        {
            incomingNode = this.EntryNode;
            this.EntryNode = subgraph;
        }

        subgraph.EntryNode = incomingNode;
        subgraph.ExitNode = exitDummy;
        subgraph.Nodes.Add(exitDummy);

        if (outgoingNode != null)
        {
            subgraph.AddSuccessor(outgoingNode);
        }

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
            node.UpdateClonable();

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

            if (node == ExitNode || node is BreakNode || node is ReturnNode || node is ContinueNode)
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
            node.immediatePostDominator = null!;
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
        if (!Nodes.Remove(node))
        {
            throw new("node not in subgraph!");
        }
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
        while (workingQueue.TryDequeue(out ControlFlowNode? node))
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
        List<(ControlFlowNode head, ControlFlowNode tail)> loops = [];
        HashSet<ControlFlowNode> loopHeaders = [];
        foreach (var node in Nodes)
        {
            foreach (var succ in node.Successors)
            {
                // Every successor that dominates its predecessor
                // is a loop header

                if (node.dominators.Contains(succ) && !loopHeaders.Contains(succ))
                {
                    loops.Add((succ, node));
                    loopHeaders.Add(succ);
                }
            }
        }

        foreach (var (head, tail) in loops)
        {
            // skip the loop this subgraph was created for
            if (head == EntryNode || !this.Nodes.Contains(head))
            {
                continue;
            }

            HashSet<ControlFlowNode> nodes = FindNodesInLoop(head, tail);
            ControlFlowNode? breakTarget = head.Successors.SingleOrDefault(s => !nodes.Contains(s));

            DetectContinuesAndBreaks(nodes, head, breakTarget);

            var subgraph = InsertSubgraph(nodes, ControlFlow.SubgraphKind.Loop, null);
            subgraph.ReplaceLoops();
        }
    }

    // traces a loop from its tail to its head to determine which nodes it includes
    private HashSet<ControlFlowNode> FindNodesInLoop(ControlFlowNode header, ControlFlowNode tail)
    {
        HashSet<ControlFlowNode> nodes = [header]; // the set of nodes in the loop
        Stack<ControlFlowNode> stack = []; // a stack of nodes we need to visit

        if (header != tail)
        {
            nodes.Add(tail);
            stack.Push(tail);
        }

        // search upwards until there are more nodes left
        // the search will stop at the head since it's already in the list
        while (stack.Count > 0)
        {
            var block = stack.Pop();
            foreach (var pred in block.Predecessors)
            {
                if (nodes.Add(pred))
                {
                    stack.Push(pred);
                }

                // the loop may have branches off it leading to return/break statements
                foreach (var succ in block.Successors)
                {
                    if (succ.Predecessors.Contains(header))
                    {
                        continue;
                    }

                    if (nodes.Add(succ))
                    {
                        stack.Push(succ);
                    }
                }
            }
        }

        return nodes;

        // adds nodes that branch off the main cycle to 'nodes'
        static void AddSuccessorsToSet(HashSet<ControlFlowNode> nodes, ControlFlowNode node, ControlFlowNode loopHeader)
        {
            Stack<ControlFlowNode> stack = [];
            stack.Push(node);
            nodes.Add(node);

            while (stack.Count > 0)
            {
                var n = stack.Pop();
                
                foreach (var succ in n.Successors)
                {
                    // if the node is a successor of the loop header, it's the break target. don't include it.
                    if (succ.Predecessors.Contains(loopHeader))
                    {
                        continue;
                    }

                    if (nodes.Add(succ))
                    {
                        stack.Push(succ);
                    }
                }
            }
        }

    }

    private void DetectContinuesAndBreaks(HashSet<ControlFlowNode> nodes, ControlFlowNode header, ControlFlowNode? breakTarget)
    {
        if (breakTarget != null)
        {
            // replace every edge to the break target with a break node
            foreach (var brk in breakTarget.Predecessors.Except([header]))
            {
                brk.RemoveSuccessor(breakTarget);

                var brkNode = new BreakNode();
                brk.AddSuccessor(brkNode);
                nodes.Add(brkNode);
                this.Nodes.Add(brkNode);
            }
        }

        // replace every node in the loop targeting the header with a continue
        foreach (var pred in header.Predecessors)
        {
            if (nodes.Contains(pred))
            {
                pred.RemoveSuccessor(header);

                var continueNode = new ContinueNode();
                nodes.Add(continueNode);
                this.Nodes.Add(continueNode);

                pred.AddSuccessor(continueNode);
            }
        }
    }

    private void DetectReturns(ControlFlowNode target)
    {
        // special case: in debug builds, the compiler likes to save return values
        // to a local and then jump to a common return block which returns that local.
        ControlFlowNode? returnVarTarget = FindReturnVarTarget(target);

        foreach (var node in Nodes.ToArray())
        {
            foreach (var successor in node.Successors.ToArray())
            {
                if (successor != target && successor != returnVarTarget)
                    continue;

                node.RemoveSuccessor(successor);

                ReturnNode retNode = new() { isReturnVarBlock = successor == returnVarTarget };
                node.AddSuccessor(retNode);
                Nodes.Add(retNode);
            }
        }
        Trim();
    }

    private ControlFlowNode? FindReturnVarTarget(ControlFlowNode returnTarget)
    {
        if (returnTarget.Predecessors.Count != 1)
        {
            return null;
        }

        var pred = returnTarget.Predecessors.Single();
        if (pred is not BasicBlockNode returnVarBB)
        {
            return null;
        }

        // return var blocks look exactly like this:
        // ldloc.n 
        // ret
        // we need to handle them specially because they could be jumped to from anywhere on the cfg (they need to be treated as returns)

        if (returnVarBB.Instructions.Count != 2) 
        {
            return null;
        }
        if (!IsLdloc(returnVarBB.Instructions[0].OpCode)) 
        {
            return null;
        }
        if (returnVarBB.Instructions[1].OpCode != OpCode.Ret)
        {
            return null;
        }
        var returnVarLocalIdx = GetLocalIdx(returnVarBB.Instructions[0]);

        // final check: for a return var block, every jump to it is preceded by a stloc (possibly a stind?) instruction.
        foreach (var returnStatementBlock in returnVarBB.Predecessors)
        {
            var bb = (BasicBlockNode)returnStatementBlock;
            if (!IsStloc(bb.Instructions[^2].OpCode)) 
            {
                return null;
            }
            
            var idx = GetLocalIdx(bb.Instructions[^2]);
            if (idx != returnVarLocalIdx)
            {
                return null;
            }
        }

        return returnVarBB;

        static int GetLocalIdx(Instruction instruction) => instruction.OpCode switch
        {
            OpCode.Ldloc_0 or OpCode.Stloc_0 => 0,
            OpCode.Ldloc_1 or OpCode.Stloc_1 => 1,
            OpCode.Ldloc_2 or OpCode.Stloc_2 => 2,
            OpCode.Ldloc_3 or OpCode.Stloc_3 => 3,
            OpCode.Ldloc_S or OpCode.Stloc_S => (byte)instruction.Argument,
            OpCode.Ldloc or OpCode.Stloc => (int)instruction.Argument,
            _ => -1,
        };

        static bool IsLdloc(OpCode opcode) => opcode is
            OpCode.Ldloc_0 or
            OpCode.Ldloc_1 or
            OpCode.Ldloc_2 or
            OpCode.Ldloc_3 or
            OpCode.Ldloc_S or
            OpCode.Ldloc;

        static bool IsStloc(OpCode opcode) => opcode is
            OpCode.Stloc_0 or
            OpCode.Stloc_1 or
            OpCode.Stloc_2 or
            OpCode.Stloc_3 or
            OpCode.Stloc_S or
            OpCode.Stloc;
    }

    private void Trim()
    {
        HashSet<ControlFlowNode> touchedNodes = [];
        DepthFirstTraverse(n => touchedNodes.Add(n));
        foreach (var node in Nodes.ToArray())
        {
            if (!touchedNodes.Contains(node))
            {
                Nodes.Remove(node);
            }
        }
    }

    void ReplaceConditionals()
    {
        // a conditional is any node with two successors
        // there are no loops left at this point
        List<ControlFlowNode> conditionNodes = [];
        for (int i = Nodes.Count - 1; i >= 0; i--)
        {
            var node = Nodes[i];

            if (SubgraphKind == ControlFlow.SubgraphKind.Conditional && node == EntryNode)
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

            RecomputeDominators();
            RecomputePostDominators();
            
            HashSet<ControlFlowNode> nodes = FindConditionalNodes(node, node.immediatePostDominator);
            InsertSubgraph(nodes, ControlFlow.SubgraphKind.Conditional, null);
        }

        foreach (var node in Nodes)
        {
            if (node is ControlFlowGraph subgraph)
            {
                subgraph.ReplaceConditionals();
            }
        }


    }

    private HashSet<ControlFlowNode> FindConditionalNodes(ControlFlowNode divergence, ControlFlowNode? convergence)
    {
        Debug.Assert(divergence.Successors.Count == 2);
        HashSet<ControlFlowNode> nodes = [divergence];
        foreach (var successor in divergence.Successors)
        {
            // the convergence should not be included in the conditional
            if (successor == convergence)
            {
                continue;
            }

            // skip nodes that the divergence doesn't dominate as they're not part of the conditional
            Stack<ControlFlowNode> stack = [];
            if (successor.dominators.Contains(divergence))
            {
                nodes.Add(successor);
                stack.Push(successor);
            }
            else if (successor is BasicBlockNode bb && bb.IsTrivallyClonable)
            {
                // create a clone to take the place of the node
                BasicBlockNode clone = bb.Clone();
                AddNode(clone);

                // move all incoming connections not from the divergent node to the clone
                foreach (var pred in successor.Predecessors)
                {
                    if (pred != divergence)
                    {
                        clone.AddPredecessor(pred);
                        successor.RemovePredecessor(pred);
                    }
                }

                foreach (var succ in successor.Successors)
                {
                    clone.AddSuccessor(succ);
                }

                nodes.Add(successor);
                stack.Push(successor);
            }
            else
            {
                continue;
            }

            while (stack.Count > 0)
            {
                ControlFlowNode n = stack.Pop();
                foreach (var succ in n.Successors)
                {
                    if (!succ.dominators.Contains(divergence) || succ == convergence)
                    {
                        continue;
                    }

                    nodes.Add(succ);
                    stack.Push(succ);
                }
            }
        }

        return nodes;
    }
}

class ReturnNode : DummyNode
{
    public bool isReturnVarBlock;
}