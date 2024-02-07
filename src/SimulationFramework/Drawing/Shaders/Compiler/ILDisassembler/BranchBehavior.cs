namespace SimulationFramework.Drawing.Shaders.Compiler.ILDisassembler;

/// <summary>
/// Specifies the branching behavior of an instruction.
/// </summary>
internal enum BranchBehavior
{
    /// <summary>
    /// After the instruction executes, the following instruction executes.
    /// </summary>
    Continue,
    /// <summary>
    /// The instruction always branches to another instruction.
    /// </summary>
    Branch,
    /// <summary>
    /// The instruction may branch to another instruction.
    /// </summary>
    BranchOrContinue,
    /// <summary>
    /// The instruction exits the method.
    /// </summary>
    Return,
}
