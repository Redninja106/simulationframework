using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Shaders.Compiler;

public class TranslationProfile
{
    public static readonly TranslationProfile Default = new()
    {

    };

    public Dictionary<MethodBase, MethodBase> methods = new();
}