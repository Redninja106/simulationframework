namespace SimulationFramework;

/// <summary>
/// Represents a single playback of a sound.
/// </summary>
public abstract class SoundPlayback : IDisposable
{
    /// <summary>
    /// <see langword="true"/> if the playback has been paused; otherwise <see langword="false"/>. To pause the playback, use <see cref="Pause"/>.
    /// </summary>
    public abstract bool IsPaused { get; }

    /// <summary>
    /// <see langword="true"/> if the playback has been stopped; otherwise <see langword="false"/>. To stop playback, use <see cref="Stop"/>.
    /// </summary>
    public abstract bool IsStopped { get; }

    /// <summary>
    /// <see langword="true"/> if the sound is playing; otherwise <see langword="false"/>.
    /// </summary>
    public bool IsPlaying => !IsPaused && !IsStopped;

    /// <summary>
    /// The volume of the sound. Value range from 0 to 1, where 0 is no sound and 1 is full volume.
    /// </summary>
    public abstract float Volume { get; set; }

    /// <summary>
    /// Pauses the playback. To resume, use <see cref="Resume"/>.
    /// </summary>
    public abstract void Pause();

    /// <summary>
    /// Resumes the playback when paused using <see cref="Pause"/>.
    /// </summary>
    public abstract void Resume();

    /// <summary>
    /// Restarts the sound.
    /// </summary>
    public abstract void Restart();

    /// <summary>
    /// Stops the playback. To restart it, use <see cref="Restart"/>. 
    /// <para>
    /// Stopped playbacks cannot be resumed. If you need to resume the playback, use <see cref="Pause"/>.
    /// </para>
    /// </summary>
    public abstract void Stop();

    /// <summary>
    /// Creates a new instance of the <see cref="SoundPlayback"/> class.
    /// </summary>
    public SoundPlayback()
    {

    }

    /// <inheritdoc/>
    public virtual void Dispose()
    {
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Disposes the playback.
    /// </summary>
    ~SoundPlayback()
    {
        Dispose();
    }
}