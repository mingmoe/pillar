using System.Drawing;
using JetBrains.Annotations;
using Pillar.Exceptions;
using SDL3;

namespace Pillar;

[MustDisposeResource]
public sealed class Window : IDisposable
{
    public nint WindowHandle { get; init; }
    
    public nint RendererHandle { get; init; }
    
    private bool _disposed = false;

    ~Window()
    {
        Dispose(false);
    }
    
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if(_disposed)
        {
            return;
        }

        _disposed = true;

        SDL.DestroyRenderer(RendererHandle);
        SDL.DestroyWindow(WindowHandle);
    }

    public Window(string title, int width, int height, bool fullscreen = false, bool borderless = false, bool resizable = false)
    {
        SDL.WindowFlags flags = 0;

        if (fullscreen)
        {
            flags |= SDL.WindowFlags.Fullscreen;
        }
        if (borderless)
        {
            flags |= SDL.WindowFlags.Borderless;
        }
        if (resizable)
        {
            flags |= SDL.WindowFlags.Resizable;
        }
        
        var success  = SDL.CreateWindowAndRenderer(title, width, height,flags, out var windowHandle,out var rendererHandle);
        WindowHandle = windowHandle;
        RendererHandle = rendererHandle;

        if (!success)
        {
            throw ExceptionHelper.SdlFailed("Failed to create window");
        }
    }

    public Window(string title, int width, int height,SDL.WindowFlags  flags)
    {
        var success  = SDL.CreateWindowAndRenderer(title, width, height,flags, out var windowHandle,out var rendererHandle);
        WindowHandle = windowHandle;
        RendererHandle = rendererHandle;
        
        if (!success)
        {
            throw ExceptionHelper.SdlFailed("Failed to create window");
        }
    }
    
}