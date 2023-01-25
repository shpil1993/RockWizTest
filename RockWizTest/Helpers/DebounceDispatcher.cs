using System;
using System.Windows.Threading;

namespace RockWizTest.Helpers
{
    public class DebounceDispatcher
    {
        private DispatcherTimer _timer;
        private DateTime TimerStarted { get; set; } = DateTime.UtcNow.AddYears(-1);

        /// <summary>
        /// Debounce an event by resetting the event timeout every time the event is 
        /// fired. The behavior is that the Action passed is fired only after events
        /// stop firing for the given timeout period.
        /// 
        /// Use Debounce when you want events to fire only after events stop firing
        /// after the given interval timeout period.
        /// 
        /// </summary>
        /// <param name="interval">Timeout in Milliseconds</param>
        /// <param name="action">Action<object> to fire when debounced event fires</object></param>
        /// <param name="param">optional parameter</param>
        /// <param name="priority">optional priority for the dispatcher</param>
        /// <param name="dispatcher">optional dispatcher. If not passed or null CurrentDispatcher is used.</param>        
        public void Debounce(int interval, Action<object> action,
            object param = null,
            DispatcherPriority priority = DispatcherPriority.ApplicationIdle,
            Dispatcher dispatcher = null)
        {
            // kill pending timer and pending ticks
            _timer?.Stop();
            _timer = null;

            if (dispatcher == null)
                dispatcher = Dispatcher.CurrentDispatcher;

            // timer is recreated for each event and effectively
            // resets the timeout. Action only fires after timeout has fully
            // elapsed without other events firing in between
            _timer = new DispatcherTimer(TimeSpan.FromMilliseconds(interval), priority, (s, e) =>
            {
                if (_timer == null)
                    return;

                _timer?.Stop();
                _timer = null;
                action.Invoke(param);
            }, dispatcher);

            _timer.Start();
        }
    }
}
