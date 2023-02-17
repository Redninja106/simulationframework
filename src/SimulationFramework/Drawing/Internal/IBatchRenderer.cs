using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Internal;
internal interface IBatchRenderer
{
    void PushQuad(Rectangle quad);
    void PushLine(Vector2 from, Vector2 to);
    void PushPolygon(Span<Vector2> poly);
}
