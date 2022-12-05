using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Shaders;

public struct TrianglePrimitive<T> where T : unmanaged { public T VertexA, VertexB, VertexC; }