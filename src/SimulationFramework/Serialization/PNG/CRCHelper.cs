using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Serialization.PNG;
internal class CRCHelper
{
    static uint[] crcTable = new uint[256];

    static CRCHelper()
    {
        for (uint i = 0; i < 256; i++)
        {
            uint crc = i;
            for (int j = 0; j < 8; j++)
            {
                crc = crc >> 1 ^ ((crc & 1) != 0 ? 0xEDB88320 : 0);
            }
            crcTable[i] = crc;
        }
    }

    public static uint Calculate(byte[] data, int offset, int length)
    {
        uint crc = 0xFFFFFFFF;
        for (int i = offset; i < offset + length; i++)
        {
            crc = crcTable[(crc ^ data[i]) & 0xFF] ^ crc >> 8;
        }
        return crc ^ 0xFFFFFFFF;
    }
}
