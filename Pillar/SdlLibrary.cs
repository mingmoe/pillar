using System.Runtime.InteropServices;
using JetBrains.Annotations;
using Pillar.Exceptions;
using SDL3;

namespace Pillar;

[MustDisposeResource]
public sealed class SdlLibrary : IDisposable
{
    private bool _disposed = false;

    public static string GetLastError()
    {
        var err = SDL.GetError();
        return string.IsNullOrWhiteSpace(err) ? "unknown error" : err;
    }

    public SdlLibrary(SdlOptions sdlOptions,ApplicationOptions? applicationOptions= null)
    {
        if (applicationOptions != null)
        {
            SDL.SetAppMetadata(applicationOptions.Name, applicationOptions.Version, applicationOptions.Identifier);
        }

        SDL.SetMainReady();
        var success = SDL.Init(SDL.InitFlags.Audio |
                                   SDL.InitFlags.Video |
                                   SDL.InitFlags.Joystick |
                                   SDL.InitFlags.Haptic |
                                   SDL.InitFlags.Gamepad |
                                   SDL.InitFlags.Events |
                                   SDL.InitFlags.Sensor |
                                   SDL.InitFlags.Camera
        );
        
        if (!success)
        {
            throw ExceptionHelper.FailedToInit("SDL3",GetLastError());
        }

        if (sdlOptions.Log is null) return;
        
        SDL.SetLogPriorities(SDL.LogPriority.Trace);
        // note that we should reset the output function when disposing
        SDL.GetLogOutputFunction(out var defaultCallback,out _);

        SDL.SetLogOutputFunction((_,category,priority,message) =>
            {
                sdlOptions.Log?.Invoke(category, priority, message);
            },
            // save the default callback as userdata so we can back to default callback in Dispose()
            Marshal.GetFunctionPointerForDelegate(defaultCallback));

        success = TTF.Init();
        if (!success)
        {
            throw ExceptionHelper.FailedToInit("SDL3-TTF",GetLastError());
        }

        Mixer.InitFlags expect;

        if (sdlOptions.MixerOptions == MixerInitOptions.Full)
        {
            expect = Mixer.InitFlags.FLAC |
                     Mixer.InitFlags.MOD |
                     Mixer.InitFlags.MP3 |
                     Mixer.InitFlags.OGG |
                     Mixer.InitFlags.MID |
                     Mixer.InitFlags.OPUS |
                     Mixer.InitFlags.WAVPACK;
        }
        else if (sdlOptions.MixerOptions != MixerInitOptions.Basic)
        {
            expect = Mixer.InitFlags.MP3 |
                     Mixer.InitFlags.OGG;
        }
        else
        {
            return;
        }

        var got = Mixer.Init(expect);
        if (expect != got)
        {
            throw ExceptionHelper.FailedToInit("SDL3-Mixer",$"fail to initialize SDL3-Mixer for some format was not supported(expect to support {expect} but only got support for {got}");
        }
    }
    
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~SdlLibrary()
    {
        Dispose(false);
    }

    private void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        _disposed = true;

        TTF.Quit();
        Mixer.Quit();
        SDL.ResetLogPriorities();
        // reset the log function
        SDL.GetLogOutputFunction(out _,out var callback);
        SDL.SetLogOutputFunction(Marshal.GetDelegateForFunctionPointer<SDL.LogOutputFunction>(callback),nint.Zero);
        SDL.Quit();
    }
}