using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace JobSearchScorecard
{

	public class StartPage : ContentPage
	{
		private static bool firstTime = true;
		public List<Task> currentTasks;

		static Random myRandom = new Random();
		Label scoreBox;
		Label scorePeriod;
		Label welcomeLabel;

		private SettingsPage _settingsPage = null;

		string encourage = "OK";
		string[] encouragements = { 
			"Be good to yourself - get plenty of rest and exercise", 
			"You can do it!  Keep going", 
			"Keep to a regular schedule", "You are awesome", "Nice work",
			"Each step brings you closer to your new job",
			"You deserve a sanity break, so take a breather",
			"Your job is buried out there somewhere, keep digging",
			"Be confident, and remember that it's not all about you",
			"You're an Ace Job Seeker, but take it easy.  No more than 3 job applications in one day.",
			"Impressive job seeking.  Remember to give yourself time to breathe.",
			"Steady as she goes.  Slow and steady wins the race", 
			"You are fantastic! Tell your friends and family",
		};

		public StartPage ()
		{
			Title = "Job Search Scorecard";

			NavigationPage.SetHasBackButton (this, false);  // no back button from this screen!

			App.AppSettings = App.Database.GetSettings ();
			if (App.AppSettings == null) {
				App.AppSettings = new Settings ();
				App.Database.SaveSettings (App.AppSettings);
				_settingsPage = new SettingsPage (true);  // tell Settings that it's Name-entry only
				_settingsPage.BindingContext = App.AppSettings;  // first time Settings (no Cancel)
				this.Navigation.PushModalAsync (_settingsPage);
			} else {
				_settingsPage = new SettingsPage (false);
			}
			;

			welcomeLabel = new Label () {
				FontSize = 22,
				FontAttributes = FontAttributes.Italic,
				HorizontalOptions = LayoutOptions.CenterAndExpand,
			};
			welcomeLabel.Text = String.Format ("Welcome to Job Seeker Scorecard, friend!");


			MessagingCenter.Subscribe<SettingsPage> (this, "popped", (sender) => { 
				OnAppearing();
			});
				
			if (firstTime) {  // first time in this screen for this execution, but not necessarily first time in app
				firstTime = false;
				ActivityTable.BuildActivitiesDictionary ();  // build the table of potential tasks (aka Activity list) 
				//currentTasks = MockUpTasks ();  // if you wish to mock-up some completed tasks
				Debug.WriteLine ("Built Activities Dictionary with : " + Activity.UniqueCode + " unique entries");
				Debug.WriteLine ("Database Table Counts at Start-Up:");
				Debug.WriteLine ("[Task] has " + App.Database.GetTasks ().Count () + " rows");
				Debug.WriteLine ("[Period] has " + App.Database.GetPeriods ().Count () + " rows");

			}  // first-time
				
			scoreBox = new Label () {
				Text = "No Score",
				FontSize = 55,
				FontAttributes = FontAttributes.Bold,
				HorizontalOptions = LayoutOptions.Center
			};  // Text set in the OnAppearing override in order to grab latest total score

			scorePeriod = new Label () {
				Text = "for Unknown Period",
				FontAttributes = FontAttributes.Italic,
				FontSize = 12,
			};

			var buttonStyle = new Style (typeof(Button)) {
				Setters = {
					new Setter { Property = Button.TextColorProperty, Value = Color.White, },
					new Setter { Property = Button.BackgroundColorProperty, Value = Color.Teal, },
					new Setter { Property = Button.BorderRadiusProperty, Value = 5 },
					new Setter { Property = Button.HeightRequestProperty, Value = 50 }
				}
			};

			var btnShowSteps = new Button {
				Text = "Earn points using Job Search Steps",
				Style = buttonStyle,
			};
			btnShowSteps.Clicked += async (sender, e) => {
				await this.Navigation.PushAsync (new MainStepsPage ());
			};

			var btnSpeak = new Button { 
				Text = "Hear some Encouraging Words",
				Style = buttonStyle,
			};
			btnSpeak.Clicked += (sender, e) => {
				encourage = encouragements [myRandom.Next (0, encouragements.Length)];
				DependencyService.Get<ITextToSpeech> ().Speak (
					string.Format("{0}, {1}. You have {2}",  App.AppSettings.Name, encourage, scoreBox.Text));
			};

			// Two buttons: one for starting a new period, the other for wiping-out the database
			var btnNewPeriod = new Button {
				Text = "Start New Scoring Period",
				Style = buttonStyle,
			};
			btnNewPeriod.Clicked += async(sender, e) => {
				var action = await DisplayActionSheet ("Start a new period?", "No", "Yes");
				if (action.StartsWith ("N"))
					return;
				if (App.Database.SavePeriod () != 1) {
					throw new Exception ("Cannot create new Period!");
				}
				this.OnAppearing ();
			};

			var btnPeriods = new Button {
				Text = "Show Score Period History",
				Style = buttonStyle,
			};
			btnPeriods.Clicked += async (sender, e) => {
				await this.Navigation.PushAsync (new HistoryPage ());
			};


//			var btnFb = new Button {
//				Text = "Share my Score on Facebook",
//				Style = buttonStyle,
//			};
//			btnFb.Clicked  += (sender, e) => {
//				Device.OpenUri(new Uri("http://jobtransition.net"));
//			};

			var btnLaunchJTSG = new Button {
				Text = "Take me to Job Transition Website",
				Style = buttonStyle,
			};
			btnLaunchJTSG.Clicked += (sender, e) => {
				Device.OpenUri (new Uri ("http://jobtransition.net"));
			};

			var btnSettings = new Button {
				Text = "Update Settings",
				Style = buttonStyle,
			};
			btnSettings.Clicked += async (sender, e) => {					
				_settingsPage.BindingContext = App.AppSettings;
				await this.Navigation.PushModalAsync (_settingsPage);
			};

			ScrollView scrollView = new ScrollView ();
			var stackLayout = new StackLayout {
				Padding = new Thickness (8, 8),
				Children = {
					welcomeLabel,
					scoreBox,
					scorePeriod,
					btnShowSteps,
					btnSpeak,
					btnNewPeriod,
					btnPeriods,
//					btnFb,
					btnLaunchJTSG,
					btnSettings,
				}
			};
			scrollView.Content = stackLayout;
			this.Content = scrollView;
		}

		protected override void OnAppearing ()
		{
			int totalScore = 0;
			string welcomeBanner = string.Empty;

			base.OnAppearing ();

			totalScore = CalculateScore (App.Database.GetAllTasksWithinPeriod ());

			App.Database.UpdateCurrentScore (totalScore);
			scoreBox.Text = totalScore.ToString () + " points";

			// Score remains Red until you have passed the threshold
			Color scoreColor = Color.Red;
			if (totalScore == 0) {
				welcomeBanner = "Start earning points";
			} else if (totalScore < App.AppSettings.GreenThreshold) {
				welcomeBanner = String.Format ("Try to get above {0} points", App.AppSettings.GreenThreshold);
			} else {
				welcomeBanner = "Nice work! Keep going";
				scoreColor = Color.Green;
			}

			welcomeLabel.Text = String.Format ("{0}, {1}!", welcomeBanner, App.AppSettings.Name);

			scoreBox.TextColor = scoreColor;

			var startDateTime = App.Database.GetActivePeriod ().StartDT;
			scorePeriod.Text = "for the period beginning " + startDateTime.ToString ("dddd',' MMM d 'at' HH:mm tt");

		}

		protected override bool OnBackButtonPressed ()
		{
			//return base.OnBackButtonPressed ();
			// disable Back Button from this, the main screen
			return true;
		}


		public static int CalculateScore (IEnumerable<Task> tasksAccomplished)
		{
			int tempScore = 0;
			bool foundIt = false;
			Steps step;

			List<Activity> listAtStep;

			if (tasksAccomplished != null) {

				foreach (var t in tasksAccomplished) {
					foundIt = false;
					step = (Steps)t.Step;
					listAtStep = ActivityTable.Activities [step];
					foreach (var act in listAtStep) {
						if (act.SubStep == t.SubStep) {
							foundIt = true;
							tempScore += act.Score;
						}
					}
					if (!foundIt) {
						throw new Exception ("Cannot calculate score for Step" + step + " substep: " + t.SubStep);
					}
				}
			}
			;
			return tempScore;		
		}

		/*
				 Uncomment if you wish to fake-up some tasks in order to get a score
						public List<Task> MockUpTasks ()
						{
							var tmTask = new Task ();
							tmTask.Step = 1;
							tmTask.SubStep = 10;
							tmTask.Step = 10;
							tmTask.OneTimeOnly = 1;
							tmTask.DT = new DateTime (2013, 7, 8, 12, 30, 00);
							tmTask.Notes = "join TM!";
							// just for testing.  No database action, just create fake Tasks out of thin air
							// the first argument to Task, the DatabaseID, means nothing here
							var fakeList = new List<Task> () {
								new Task (1000, (int)Steps.All, 0, 10, 0, DateTime.Now, "pray!"),
								new Task (1008, (int)Steps.Attitude, 3, 5, 1, DateTime.Now.AddHours (-3), "Read and list 3 items"),
								new Task (1001, (int)Steps.Attitude, 4, 8, 0, DateTime.Now.AddMinutes (-5), "list negatives"),
								new Task (1002, (int)Steps.Attitude, 11, 15, 0, DateTime.Now.AddDays (-2), "new skills"),
								tmTask,
								new Task (1003, (int)Steps.Assessments, 12, 8, 0, DateTime.Now.AddDays (-2), "read docs"),
								new Task (1004, (int)Steps.MarketingStrategy, 27, 10, 0, DateTime.Now.AddDays (-2), "netw handout"),
								new Task (1005, (int)Steps.MarketingStrategy, 30, 20, 0, DateTime.Now.AddDays (-2), "face2face"),
							};
							if (!App.Database.GetTaskByUniqueStepNum (tmTask.SubStep).Any ()) {
								var rc = App.Database.SaveTask (tmTask);
								if (rc != 1) {
									throw new Exception ("SaveTask failed to insert test task");
								}
							}
							;
							return fakeList;
						}
				*/
	}
}

