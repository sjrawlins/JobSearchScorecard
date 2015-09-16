using System;
using System.Threading;
using Android.App;
using Android.OS;


namespace SplashScreen
{

	[Activity (MainLauncher = true, NoHistory = true)]
	public class SplashActivity : Android.App.Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			Thread.Sleep (100); // Simulate loading process on app startup (100ms)
			StartActivity (typeof(JobSearchScorecard.Droid.MainActivity));
		}
	}
}