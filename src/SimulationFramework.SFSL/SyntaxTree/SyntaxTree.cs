using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.ShaderLanguage.SyntaxTree;

/// <summary>
/// A parsed SFSL shader
/// </summary>
internal abstract class ShaderDocument
{
    public abstract IEnumerable<DeclarationNode> GetDeclarations();
}
