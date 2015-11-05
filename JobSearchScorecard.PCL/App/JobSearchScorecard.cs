using System;
using System.Diagnostics;
using Xamarin.Forms;
using System.Collections.Generic;


namespace JobSearchScorecard
{
	public class App : Application
	{
		static ScorecardDatabase database = null;
		public static INavigation Navigation { get; private set; }

		public App ()
		{
			MainPage = new NavigationPage(new StartPage ());
		}

		public static ScorecardDatabase Database {
			get { 
				if (database == null) {
					database = new ScorecardDatabase ();
				}
				return database; 
			}
		}

//		public static Page GetMainPage ()
//		{
//			//return new StartPage ();
//			var rootPage = new NavigationPage (new StartPage ());
//			Navigation = rootPage.Navigation;
//			return rootPage;
//		}

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

