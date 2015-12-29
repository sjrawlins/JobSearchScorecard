using System;
using System.Diagnostics;
using Xamarin.Forms;
using System.Collections.Generic;


namespace JobSearchScorecard
{
	public class App : Application
	{
		public static INavigation Navigation { get; private set; }
		public static ScorecardDatabase Database;
		public static Settings AppSettings;

		public App ()
		{    
			Database = new ScorecardDatabase ();
			AppSettings = null;

			var startPage = new StartPage();
			var rootPage = new NavigationPage(startPage);
			App.Navigation = rootPage.Navigation;
			MainPage = rootPage;
			//MainPage = new NavigationPage (new StartPage ());
		}

		protected override void OnStart ()
		{
			Debug.WriteLine ("OnStart");// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			Debug.WriteLine ("OnSleep");// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			Debug.WriteLine ("OnResume");// Handle when your app resumes
		}


	}
}

