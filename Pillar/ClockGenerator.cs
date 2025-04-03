using System.Diagnostics;
using Utopia.Core;

namespace Pillar;

public sealed partial class ClockGenerator
{
    private readonly Stopwatch stopwatch = new();

    [EmitEvent]
    private readonly WeakEvent<EventArgs> _kickEvent = new();

    public long ElapsedMillisecondsSinceLastKick => stopwatch.ElapsedMilliseconds;

    /// <summary>
    ///
    /// </summary>
    /// <returns>Elapsed milliseconds since last kick</returns>
    public long Kick()
    {
        stopwatch.Stop();
        var elapsed = stopwatch.ElapsedMilliseconds;
        stopwatch.Restart();
        return elapsed;
    }

    /// <summary>
    /// Busy wait! Occupy a lot of cpu time but can be precise.
    /// </summary>
    /// <param name="elapsedMillisecondsSinceLastKick"></param>
    public void WaitWhen(int elapsedMillisecondsSinceLastKick)
    {
        while (elapsedMillisecondsSinceLastKick < ElapsedMillisecondsSinceLastKick)
        {
            Thread.Yield();
        }
    }

    /// <summary>
    /// Busy wait! Occupy a lot of cpu time but can be precise.
    /// </summary>
    /// <param name="elapsedMillisecondsSinceLastKick"></param>
    /// <returns>Elapsed milliseconds since last kick</returns>
    public long WaitWhenAndKick(int elapsedMillisecondsSinceLastKick)
    {
        WaitWhen(elapsedMillisecondsSinceLastKick);
        return Kick();
    }
}