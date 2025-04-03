using Pillar;
using SDL3;

namespace Sample;

class Program
{
    public sealed class Loop(Window window) : EventLoop(window)
    {
        protected override void Initialize()
        {

        }

        protected override void OnEvent(SDL.Event @event)
        {
            if (@event.Type == (uint)SDL.EventType.Quit)
            {
                Stop();
            }
        }

        protected override void Update()
        {

        }

        protected override void Draw(Window window)
        {

        }

        protected override void OnExit()
        {

        }
    }

    [STAThread]
    static void Main(string[] args)
    {
        SdlOptions options = new()
        {
            MixerOptions = MixerInitOptions.Basic,
            Log = ((category, priority, msg) =>
            {
                Console.WriteLine($"[{category}] [{priority}]: {msg}");
            })
        };
        ApplicationOptions app = new()
        {
            Name = "Sample",
            Version = "1.0.0",
            Identifier = "moe.kawayi.org",
        };

        using SdlLibrary sdlLibrary = new(options,app);

        using var window = new Window("Hello World!",256,256);

        var loop = new Loop(window);

        loop.Run();
    }
}