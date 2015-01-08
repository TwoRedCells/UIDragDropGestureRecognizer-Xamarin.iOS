using CoreGraphics;
using UIKit;

namespace RedCell.UI.iOS.DragDrop.Demo
{
    /// <summary>
    /// Class RootViewController.
    /// </summary>
    public class RootViewController : UIViewController
    {
        /// <summary>
        /// Called after the controller’s <see cref="P:UIKit.UIViewController.View" /> is loaded into memory.
        /// </summary>
        /// <remarks>This method is called after <c>this</c> <see cref="T:UIKit.UIViewController" />'s <see cref="P:UIKit.UIViewController.View" /> and its entire view hierarchy have been loaded into memory. This method is called whether the <see cref="T:UIKit.UIView" /> was loaded from a .xib file or programmatically.</remarks>
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // Main background
            View.BackgroundColor = UIColor.White;

            // Configure drag/drop globally.
            // These is exaggerated for demonstration.
            DragDropGestureRecognizer.HoldToBeginThresholdMilliseconds = 2000;
            DragDropGestureRecognizer.MovementThreshold = 50;

            // Make some boxes.
            var red = new UIView
            {
                Frame = new CGRect(100, 100, 100, 100),
                BackgroundColor = UIColor.Red
            };
             var green = new UIView
            {
                Frame = new CGRect(100, 250, 100, 100),
                BackgroundColor = UIColor.Green
            };
            var blue = new UIView
            {
                Frame = new CGRect(100, 400, 100, 100),
                BackgroundColor = UIColor.Blue
            };
            var boxes = new[] { red, green, blue };

            // Add the gesture recognizer and and to view.
            foreach (var box in boxes)
            {
                var dd = new DragDropGestureRecognizer();
                dd.Dragging += OnDragging;
                box.AddGestureRecognizer(dd);
                View.AddSubview(box);
            }
        }

        private void OnDragging(object sender, DragDropEventArgs e)
        {
            var dd = sender as DragDropGestureRecognizer;
            var view = dd.View;

            // Reposition box.
            var x = e.ViewWasAt.X + e.Delta.X;
            var y = e.ViewWasAt.Y + e.Delta.Y;
            view.Center = new CGPoint(x, y);
        }
    }
}