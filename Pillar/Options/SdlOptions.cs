using SDL3;

namespace Pillar;

public enum MixerInitOptions
{
    /// <summary>
    /// do not init SDL-Mixer
    /// </summary>
    None = 0,
    /// <summary>
    /// init SDL-Mixer to support .ogg and .mp3 format.
    /// This operation may fail and threw an exception.
    /// See <see cref="Mixer.InitFlags"/>.
    /// </summary>
    Basic = 1,
    /// <summary>
    /// init SDL-Mixer to support all format that SDL-Mixer supports.
    /// This operation may fail and threw an exception.
    /// See <see cref="Mixer.InitFlags"/>.
    /// </summary>
    Full = 2
}

public sealed class SdlOptions
{
    public Action<SDL.LogCategory, SDL.LogPriority, string>? Log { get; set; } = null;

    public MixerInitOptions MixerOptions { get; set; } = MixerInitOptions.Basic;
}