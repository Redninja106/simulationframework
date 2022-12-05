using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Shaders;
public class MaxVerticesAttribute : Attribute 
{ 
    public MaxVerticesAttribute(int maxVertices) 
    {
    } 
}