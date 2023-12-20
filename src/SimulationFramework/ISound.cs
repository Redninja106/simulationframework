using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework;

/// <summary>
/// Represents a playable sound
/// </summary>
public interface ISound : IDisposable
{
    int SampleRate { get; }
    float Duration { get; }
    bool IsStereo { get; }
    bool Is16Bit { get; }

    SoundPlayback Play();
}
