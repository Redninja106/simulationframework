﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Shaders;

[AttributeUsage(AttributeTargets.Field)]
public sealed class ShaderUniformAttribute : Attribute
{
}