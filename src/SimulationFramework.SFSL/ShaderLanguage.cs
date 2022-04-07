using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.SFSL;

public enum ShaderLanguage
{
    SFSL, 
    HLSL, // dx & vk
    GLSL, // gl & vk
    MSL,  // mtl
}