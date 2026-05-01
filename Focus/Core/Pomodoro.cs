using Microsoft.UI.Xaml;

using System;

namespace Focus.Core;

/// <summary>
/// Representing a custom pomodoro timer.
/// </summary>
/// <param name="time">Target pomodoro time.</param>
public sealed class Pomodoro(TimeSpan time)
{
    private DispatcherTimer _timer = new DispatcherTimer();

    private void _timer_Tick(object? sender, object e)
    {
        time = time.Add(TimeSpan.FromMilliseconds(-500));
        if (time <= TimeSpan.Zero)
        {
            _timer.Stop();
            Finished.Invoke(this, EventArgs.Empty);
        }
    }

    /// <summary>
    /// Gets the number of remaining breaks based on the total time. A break is taken every 25 minutes of work, and each break lasts for 5 minutes.
    /// </summary>
    /// <returns>Number of the remaining breaks.</returns>
    public int GetBreaks()
    {
        return (int)time.TotalMinutes / 25;
    }
    
    /// <summary>
    /// Gets the total time remaining.
    /// </summary>
    /// <returns></returns>
    public TimeSpan GetRemainingTime()
    {
        int breaks = GetBreaks();
        TimeSpan totalBreakTime = TimeSpan.FromMinutes(breaks * 5);
        return time - totalBreakTime;
    }

    /// <summary>
    /// Stárts or resumes the pomodoro timer. If the timer is already running, this method will have no effect.
    /// </summary>
    public void Start()
    {
        _timer.Interval = TimeSpan.FromMilliseconds(500);
        _timer.Tick += _timer_Tick;
        _timer.Start();
        Started.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Pauses the timer, halting its progress until resumed.
    /// </summary>
    /// <remarks>This method stops the timer and raises the Paused event. Call this method to temporarily
    /// suspend timer activity without resetting its state. To continue, use the appropriate resume method if
    /// available.</remarks>
    public void Pause()
    {
        _timer.Stop();
        Paused.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Stops the timer and resets its state. After calling this method, the timer will be in a stopped state, and any progress made will be lost. To start the timer again, use the Start method.
    /// </summary>
    public void Stop()
    {
        _timer.Stop();
        time = TimeSpan.Zero;
        Stopped.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Occurs when the operation or process has started.
    /// </summary>
    public event EventHandler Started = delegate { };

    /// <summary>
    /// Occurs when the operation or process has been paused.
    /// </summary>
    public event EventHandler Paused = delegate { };

    /// <summary>
    /// Occurs when the associated process or operation has stopped.
    /// </summary>
    public event EventHandler Stopped = delegate { };

    /// <summary>
    /// Occurs when the pomodoro timer has completed its countdown, indicating that the designated time has elapsed. This event is triggered when the timer reaches zero, signaling the end of the pomodoro session. Subscribers to this event can perform actions such as notifying the user, starting a break, or logging the completion of the session.
    /// </summary>
    public event EventHandler Finished = delegate { };
}
