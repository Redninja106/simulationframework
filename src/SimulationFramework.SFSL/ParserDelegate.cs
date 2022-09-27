using System;
using SimulationFramework.ShaderLanguage.SyntaxTree;

namespace SimulationFramework.ShaderLanguage;

internal delegate T ParserDelegate<T>(ref TokenStream stream) where T : Node;