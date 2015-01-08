using Foundation;
using UIKit;

namespace RedCell.UI.iOS.DragDrop.Demo
{
    /// <summary>
    /// Class AppDelegate.
    /// </summary>
    /// <remarks>
    /// The UIApplicationDelegate for the application. This class is responsible for launching the
    /// User Interface of the application, as well as listening (and optionally responding) to
    /// application events from iOS.
    /// </remarks>
    [Register("AppDelegate")]
    public partial class AppDelegate : UIApplicationDelegate
    {
        /// <summary>
        /// Finisheds the launching.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <param name="options">The options.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Window = new UIWindow(UIScreen.MainScreen.Bounds);
            Window.RootViewController = new RootViewController();
            Window.MakeKeyAndVisible();

            return true;
        }
        
        // class-level declarations
        /// <summary>
        /// The window used to display the app on the device's main screen.
        /// </summary>
        /// <value><para>(More documentation for this node is coming)</para>
        /// <para tool="nullallowed">This value can be <see langword="null" />.</para></value>
        /// <remarks>To be added.</remarks>
        public override UIWindow Window
        {
            get;
            set;
        }

        /// <summary>
        /// Called when the application is about to enter the background, be suspended, or when the user receives an interruption such as a phone call or text.
        /// </summary>
        /// <param name="application">Reference to the UIApplication that invoked this delegate method.</param>
        /// <altmember cref="M:UIKit.UIApplicationDelegate.OnActivated" />
        /// <remarks>Because iOS applications should be designed to be long-lived, with many transitions between foreground processing, suspension or background processing, or interrupted, this method may be a good place to release expensive resources or otherwise ensure that the application is in a consistent, restorable state.</remarks>
        public override void OnResignActivation(UIApplication application)
        {
        }

        /// <summary>
        /// Called when the app enters the backgrounded state.
        /// </summary>
        /// <param name="application">Reference to the UIApplication that invoked this delegate method.</param>
        /// <altmember cref="M:UIKit.UIApplicationDelegate.WillEnterForeground" />
        /// <altmember cref="M:UIKit.UIApplication.WillTerminate" />
        /// <remarks>Application are allocated approximately 5 seconds to complete this method. Application developers should use this time to save user data and tasks, and remove sensitive information from the screen.</remarks>
        public override void DidEnterBackground(UIApplication application)
        {
        }

        /// <summary>
        /// Called prior to the application returning from a backgrounded state.
        /// </summary>
        /// <param name="application">Reference to the UIApplication that invoked this delegate method.</param>
        /// <altmember cref="M:UIKit.UIApplicationDelegate.DidEnterBackground" />
        /// <remarks>Immediately after this call, the application will call <see cref="M:MonoTouchUIKit.UIApplicationDelegate.OnActivated" />.</remarks>
        public override void WillEnterForeground(UIApplication application)
        {
        }

        /// <summary>
        /// Called if the application is being terminated due to memory constraints or directly by the user.
        /// </summary>
        /// <param name="application">Reference to the UIApplication that invoked this delegate method.</param>
        /// <altmember cref="M:UIKit.UIApplicationDelegate.OnResignActivation" />
        /// <altmember cref="M:UIKit.UIApplicationDelegate.WillEnterBackground" />
        /// <remarks><para>iOS applications are expected to be long-lived, with many transitions between activated and non-activated states (see <see cref="M:UIKit.UIApplicationDelegate.OnActivated" />, <see cref="M:UIKit.UIApplicationDelegate.OnResignActivation" />) and are typically only terminated by user command or, rarely, due to memory exhaustion (see <see cref="M:UIKit.UIApplicationDelegate.ReceiveMemoryWarning" />).</para>
        /// <para>
        ///   <img href="UIApplicationDelegate.Lifecycle.png" />
        /// </para></remarks>
        public override void WillTerminate(UIApplication application)
        {
        }
    }
}
