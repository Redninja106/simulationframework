using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Shaders;
class ThreadGroupSize : Attribute 
{ 
    public ThreadGroupSize(int width, int height, int depth) 
    { 
    } 
}