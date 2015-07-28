using System;
using System.Threading;
using Android.App;
using Android.OS;
using WooddaleScorecard.Droid;

namespace SplashScreen
{

    [Activity (Theme = "@style/Theme.Splash", MainLauncher = true, NoHistory = true)]
    public class SplashActivity : Activity
    {
        protected override void OnCreate (Bundle bundle)
        {
            base.OnCreate (bundle);
            Thread.Sleep (100); // Simulate loading process on app startup (100ms)
            StartActivity (typeof(MainActivity));
        }
    }
}


