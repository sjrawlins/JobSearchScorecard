using System;
using System.Diagnostics;
using Xamarin.Forms;
using System.Collections.Generic;

namespace JobSearchScorecard
{
	public class App : Application
	{

		// look-up via ushort code
		//        public static Page GetMainPage()
		//        {
		//            return new ContentPage
		//            {
		//                Content = new Label
		//                {
		//                    Text = "Hello, Forms !",
		//                    TextColor = Color.Red,
		//                    VerticalOptions = LayoutOptions.CenterAndExpand,
		//                    HorizontalOptions = LayoutOptions.CenterAndExpand,
		//                },
		//            };
		//        }
		public App ()
		{   
			//MainPage = new MainTabs ();

			MainPage = new StartPage { Title = "App Lifecycle Sample" }; // your page here
		}

		public static Page GetMainPage ()
		{
			return new StartPage ();
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

