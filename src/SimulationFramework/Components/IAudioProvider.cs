using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Components;

/// <summary>
/// Provides audio functionality to the application
/// </summary>
public interface IAudioProvider : ISimulationComponent
{
    float MasterVolume { get; set; }
    ISound LoadSound(ReadOnlySpan<byte> encodedData, SoundFileKind kind);
}
