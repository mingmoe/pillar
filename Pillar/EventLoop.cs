using Pillar.Exceptions;
using SDL3;

namespace Pillar;

public abstract class EventLoop
{
    public Window Window { get; init; }

    private int _stop = 0;

    public bool Running => _stop != 0;

    public EventLoop(Window window)
    {
        ArgumentNullException.ThrowIfNull(window);
        Window = window;
    }

    protected abstract void Initialize();

    protected abstract void OnEvent(SDL.Event @event);

    protected abstract void Update();

    protected abstract void Draw(Window window);
    protected abstract void OnExit();

    protected SDL.Event? GetEvent()
    {
        _ = SDL.PollEvent(out var e);
        return e;
    }

    protected void EventStage()
    {
        var e = GetEvent();
        while(true){
            if (!e.HasValue || e.Value.Type == (nint)SDL.EventType.PollSentinel)
            {
                return;
            }

            OnEvent(e.Value);
            e = GetEvent();
        }
    }

    public void Stop()
    {
        Interlocked.Exchange(ref _stop, 0);
    }

    public void Run()
    {
        var started = Interlocked.CompareExchange(ref _stop,int.MaxValue,0);

        if (started != 0)
        {
            throw new InvalidOperationException("the event loop is already running");
        }

        try
        {
            Initialize();
            try
            {
                while (_stop != 0)
                {
                    EventStage();
                    Update();
                    Draw(Window);
                }
            }
            finally
            {
                OnExit();
            }
        }
        finally
        {
            Stop();
        }
    }
}