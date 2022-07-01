using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Imaging.PNG;

public record struct PNGMetadata(uint Width, uint Height, byte BitDepth, byte ColorType, byte CompressionMethod, byte FilterMethod, byte InterlaceMethod);