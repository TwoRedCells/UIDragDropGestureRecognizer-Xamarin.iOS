using System;
using System.Diagnostics;
using System.Timers;
using CoreGraphics;
using Foundation;
using UIKit;

namespace RedCell.UI.iOS
{
    /// <summary>
    /// A drag-and-drop gesture recognizer.
    /// </summary>
    /// <remarks>
    /// <para>This UIGestureRecognizer looks for the following sequence of events.
    /// If there is any deviation from this, the gesture fails.</para>
    /// <list type="number">
    /// <item><description>Touch begins with a single finger. (Possible)</description></item>
    /// <item><description>A timer starts. (Possible)</description></item>
    /// <item><description>The finger must stay at the same point until the timer elapses. (Possible)</description></item>
    /// <item><description>Once the timer elapses, the finger can move. (Changed)</description></item>
    /// <item><description>When the finger is lefted, the gesture is complete. (Recognized)</description></item>
    /// </list>
    /// </remarks>
    public class DragDropGestureRecognizer : UIGestureRecognizer
    {
        #region Constants
        /// <summary>
        /// The default duration
        /// </summary>
        public const int DefaultDuration = 500;

        /// <summary>
        /// The default movement threshold
        /// </summary>
        public const double DefaultMovementThreshold = 10.0;
        #endregion

        #region Fields
        /// <summary>
        /// The time to hold-to-drag in milliseconds before qualifying.
        /// </summary>
        public static int HoldToBeginThresholdMilliseconds = DefaultDuration;

        /// <summary>
        /// The movement threshold.
        /// </summary>
        public static double MovementThreshold = DefaultMovementThreshold;

        private readonly Action<DragDropGestureRecognizer> _action;
        private Timer _timer;
        private static int _serial = 0;
        #endregion

        #region Initialization
        /// <summary>
        /// Initializes a new instance of the <see cref="DragDropGestureRecognizer"/> class.
        /// </summary>
        public DragDropGestureRecognizer()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DragDropGestureRecognizer"/> class.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <remarks>
        /// This method signature works like traditional iOS UIGestureRecognizers.
        /// Using events instead is recommended.
        /// </remarks>
        public DragDropGestureRecognizer(Action<DragDropGestureRecognizer> action)
        {
            _action = action;
        }
        #endregion

        #region Events

        /// <summary>
        /// Occurs when dragging.
        /// </summary>
        public event EventHandler<DragDropEventArgs> Dragging;

        /// <summary>
        /// Handles the <see cref="E:Dragging" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="DragDropEventArgs"/> instance containing the event data.</param>
        protected void OnDragging(object sender, DragDropEventArgs e)
        {
            if (Dragging != null)
                Dragging(sender, e);
        }

        /// <summary>
        /// Occurs when dropped.
        /// </summary>
        public event EventHandler<DragDropEventArgs> Dropped;

        /// <summary>
        /// Handles the <see cref="E:Dropped" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="DragDropEventArgs"/> instance containing the event data.</param>
        protected void OnDropped(object sender, DragDropEventArgs e)
        {
            if (Dropped != null)
                Dropped(sender, e);
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets a value indicating whether a long press sub-gesture has been completed.
        /// </summary>
        /// <value><c>true</c> if a long press sub-gesture has been completed; otherwise, <c>false</c>.</value>
        public bool DidLongPress { get; private set; }

        /// <summary>
        /// Gets a value indicating whether a drag operation has occured since the long press sub-gesture was completed.
        /// </summary>
        /// <value><c>true</c> if a drag operation has occured; otherwise, <c>false</c>.</value>
        public bool DidDrag { get; private set; }

        /// <summary>
        /// Gets the point at which the gesture began.
        /// </summary>
        /// <value>The begin point</value>
        public CGPoint DownAt { get; private set; }

        /// <summary>
        /// Gets the point at which the last known drag operation occured.
        /// </summary>
        /// <value>The drag point.</value>
        public CGPoint DragAt { get; private set; }

        /// <summary>
        /// Gets the point at which the view was when the gesture began.
        /// </summary>
        /// <value>The view was at.</value>
        public CGPoint ViewWasAt { get; private set; }

        /// <summary>
        /// Gets the change in position since the gesture began.
        /// </summary>
        /// <value>The delta.</value>
        public CGPoint Delta
        {
            get { return new CGPoint(DragAt.X - DownAt.X, DragAt.Y - DownAt.Y); }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="DragDropGestureRecognizer"/> is active.
        /// </summary>
        /// <value><c>true</c> if active; otherwise, <c>false</c>.</value>
        public bool Active { get { return DidDrag; } }

        /// <summary>
        /// The current state of this UIGestureRecognizer. Read-only.
        /// </summary>
        /// <value>To be added.</value>
        /// <remarks>To be added.</remarks>
        public override UIGestureRecognizerState State
        {
            get { return base.State; }
            set
            {
                base.State = value;
                if(_action != null)
                    _action(this);
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Invoked when a touch gesture begins.
        /// </summary>
        /// <param name="touches">To be added.</param>
        /// <param name="evt">To be added.</param>
        /// <remarks>To be added.</remarks>
        public override void TouchesBegan(NSSet touches, UIEvent evt)
        {
            base.TouchesBegan(touches, evt);

            if (NumberOfTouches > 1)
            {
                State = UIGestureRecognizerState.Failed;
                return;
            }

            // A new touch cancels any outstanding Touches.

            if (_timer != null) _timer.Dispose();
            _timer = new Timer
            {
                AutoReset = false,
                Enabled = true,
                Interval = HoldToBeginThresholdMilliseconds
            };
            _timer.Elapsed += TimerOnElapsed;
            DownAt = GetTouchPoint();
            ViewWasAt = View.Center;
            State = UIGestureRecognizerState.Possible;
            Debug.WriteLine("DragDropGesture #{0}: Begin at {1},{2}", ++_serial, DownAt.X, DownAt.Y);
        }

        /// <summary>
        /// Invoked when the timer elapsed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="elapsedEventArgs">The <see cref="ElapsedEventArgs"/> instance containing the event data.</param>
        private void TimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            var timer = sender as Timer;
            // Could be from an previous timer that wasn't stopped in time.
            if (timer == _timer)
            {
                DidLongPress = true;
                _timer.Stop();
                _timer.Dispose();
                _timer = null;
                Debug.WriteLine("DragDropGesture #{0}: Held", _serial);
                State = UIGestureRecognizerState.Changed;
            }
        }

        /// <summary>
        /// Invoked when a touch gesture ends.
        /// </summary>
        /// <param name="touches">To be added.</param>
        /// <param name="evt">To be added.</param>
        /// <remarks>To be added.</remarks>
        public override void TouchesEnded(NSSet touches, UIEvent evt)
        {
            base.TouchesEnded(touches, evt);

            if (_timer != null)
                _timer.Dispose();

            _timer = null;
            if (DidDrag)
            {
                State = UIGestureRecognizerState.Recognized;
                OnDropped(this, new DragDropEventArgs(State, DragAt, Delta, ViewWasAt));
            }
            else
            {
                State = UIGestureRecognizerState.Failed;
            }
            Debug.WriteLine("DragDropGesture #{0}: {1}", _serial, State);
        }

        /// <summary>
        /// Invoked when a touch gesture is cancelled.
        /// </summary>
        /// <param name="touches">To be added.</param>
        /// <param name="evt">To be added.</param>
        /// <remarks>To be added.</remarks>
        public override void TouchesCancelled(NSSet touches, UIEvent evt)
        {
            base.TouchesCancelled(touches, evt);

            if (_timer != null)
                _timer.Dispose();

            _timer = null;
            State = UIGestureRecognizerState.Failed;
            Debug.WriteLine("DragDropGesture #{0}: Cancelled", _serial);
        }

        /// <summary>
        /// Inboked when a touch gesture moves.
        /// </summary>
        /// <param name="touches">To be added.</param>
        /// <param name="evt">To be added.</param>
        /// <remarks>To be added.</remarks>
        public override void TouchesMoved(NSSet touches, UIEvent evt)
        {
            base.TouchesMoved(touches, evt);
            if (State == UIGestureRecognizerState.Failed)
                return;

            // After long press:
            if (DidLongPress)
            {
                var dragat = GetTouchPoint();
                if (dragat == DragAt)
                    return; // Not noteworthy.

                DragAt = dragat;
                if (!StayedPut(DownAt, DragAt))
                {
                    Debug.WriteLine("DragDropGesture #{0}: Dragging at {1},{2}", _serial, DragAt.X, DragAt.Y);
                    DidDrag = true;
                    OnDragging(this, new DragDropEventArgs(State, DragAt, Delta, ViewWasAt));
                    State = UIGestureRecognizerState.Changed;
               }
            }

            // Before long press:
            else
            {
                if (StayedPut(GetTouchPoint(), DownAt))
                    return;

                if (_timer != null)
                    _timer.Dispose();
                _timer = null;
                State = UIGestureRecognizerState.Failed;
                Debug.WriteLine("DragDropGesture #{0}: Moved prematurely", _serial);
            }
        }

        /// <summary>
        /// Resets the state of the gesture recognizer.
        /// </summary>
        /// <remarks>To be added.</remarks>
        public override void Reset()
        {
            base.Reset();

            if (_timer != null)
            {
                _timer.Dispose();
                _timer.Stop();
            }
            _timer = null;

            State = UIGestureRecognizerState.Possible;
            DownAt = CGPoint.Empty;
            DragAt = CGPoint.Empty;
            DidLongPress = false;
            DidDrag = false;
            Debug.WriteLine("DragDropGesture #{0}: Reset", _serial);
        }

        /// <summary>
        /// Checks if movement has exceeded the movement threshold.
        /// </summary>
        /// <param name="current">The current point.</param>
        /// <param name="previous">The previous point.</param>
        /// <returns><c>true</c> if movement has exceeded the movement threshold, <c>false</c> otherwise.</returns>
        private bool StayedPut(CGPoint current, CGPoint previous)
        {
            return Distance(current, previous) < MovementThreshold;
        }

        /// <summary>
        /// Measures the distance between two points.
        /// </summary>
        /// <param name="point1">The point1.</param>
        /// <param name="point2">The point2.</param>
        /// <returns>System.Single.</returns>
        private float Distance(CGPoint point1, CGPoint point2)
        {
            var dx = point1.X - point2.X;
            var dy = point1.Y - point2.Y;
            return (float)Math.Sqrt(dx * dx + dy * dy);
        }

        /// <summary>
        /// Gets the touch point.
        /// </summary>
        /// <returns>CGPoint.</returns>
        private CGPoint GetTouchPoint()
        {
            return LocationInView(View.Superview);
        }
        #endregion
    }
}
