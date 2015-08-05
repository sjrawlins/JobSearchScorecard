using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;

using JobSearchScorecard;

using Xamarin.Forms;

namespace JobScorecard.iOS
{
    [Register ("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        UIWindow window;

        public override bool FinishedLaunching (UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init ();

			// Also Init the Xamarin Test Cloud to enable UI Test Automation
			Xamarin.Calabash.Start();

            window = new UIWindow(UIScreen.MainScreen.Bounds);

            window.RootViewController =  App.GetMainPage().CreateViewController();

            window.MakeKeyAndVisible();

            return true;
            // return base.FinishedLaunching (app, options);
        }
    }
}

