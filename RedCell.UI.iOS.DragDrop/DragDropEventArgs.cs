using System;
using CoreGraphics;
using UIKit;

namespace RedCell.UI.iOS
{
    /// <summary>
    /// Class DragDropEventArgs.
    /// </summary>
    public class DragDropEventArgs : EventArgs
    {
        #region Initialization
        /// <summary>
        /// Initializes a new instance of the <see cref="DragDropEventArgs"/> class.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <param name="point">The point.</param>
        public DragDropEventArgs(UIGestureRecognizerState state, CGPoint point)
        {
            State = state;
            Point = point;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the state.
        /// </summary>
        /// <value>The state.</value>
        public UIGestureRecognizerState State { get; private set; }

        /// <summary>
        /// Gets the point.
        /// </summary>
        /// <value>The point.</value>
        public CGPoint Point { get; private set; }
        #endregion
    }
}