using CoreGraphics;
using Foundation;
using UIKit;

namespace RedCell.UI.iOS.DragDrop.Demo
{
    public class RootViewController : UIViewController
    {
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
            red.AddGestureRecognizer(new DragDropGestureRecognizer());
            View.AddSubview(red);

            var green = new UIView
            {
                Frame = new CGRect(100, 250, 100, 100),
                BackgroundColor = UIColor.Green
            };
            green.AddGestureRecognizer(new DragDropGestureRecognizer());
            View.AddSubview(green);

            var blue = new UIView
            {
                Frame = new CGRect(100, 400, 100, 100),
                BackgroundColor = UIColor.Blue
            };
            blue.AddGestureRecognizer(new DragDropGestureRecognizer());
            View.AddSubview(blue);
        }
    }
}