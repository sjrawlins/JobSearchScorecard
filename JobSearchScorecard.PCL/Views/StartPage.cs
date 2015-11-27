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

		Label scoreBox;
		Label scorePeriod;

		public StartPage ()
		{
			Title = "Job Search Scorecard";
			if (firstTime) {
				firstTime = false;
				ActivityTable.BuildActivitiesDictionary ();  // build the table of potential tasks (aka Activity list) 
				//currentTasks = MockUpTasks ();  // if you wish to mock-up some completed tasks
				Debug.WriteLine ("Built Activities Dictionary with : " + Activity.UniqueCode + " unique entries");
			}
			Debug.WriteLine ("Database now has " + App.Database.GetTasks ().Count () + " rows in <Task>");
			Debug.WriteLine ("Database now has " + App.Database.GetPeriods ().Count () + " rows in <Period>");

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
				Text = "Speak Score",
				Style = buttonStyle,
			};
			btnSpeak.Clicked += (sender, e) => {
				DependencyService.Get<ITextToSpeech> ().Speak (scoreBox.Text);
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

			var lblResets = new Label () {
				Text = "WARNING: The following button removes all data - the equivalent of a fresh install of the app",
			}; 
			var btnClearDB = new Button { 
				Text = "Clear Entire Database",
				Style = buttonStyle,
			};
			btnClearDB.Clicked += async (sender, e) => {
				var action = await DisplayActionSheet ("Are you sure you want to delete all data?", 
					             "No", "Yes, wipe clean!");
				if (action.StartsWith ("N"))
					return;
				App.Database.DeleteAll ();
				this.OnAppearing ();
			};
				
			this.Content = new StackLayout {
				Padding = new Thickness (8, 8),
				Children = {
					scoreBox,
					scorePeriod,
					btnShowSteps,
					btnSpeak,
					btnNewPeriod,
					btnPeriods,
//					btnFb,
					btnLaunchJTSG,
					new BoxView { Color = Color.Transparent, HeightRequest = 5 },
					lblResets,
					btnClearDB,
				}
			};
		}


		protected override void OnAppearing ()
		{
			int totalScore = 0;

			base.OnAppearing ();

			totalScore = CalculateScore (App.Database.GetAllTasksWithinPeriod ());
			App.Database.UpdateCurrentScore (totalScore);
			scoreBox.Text = totalScore.ToString () + " points";

			Color scoreColor = Color.Green;
			if (totalScore < 100) {
				scoreColor = Color.Red;
			}
			;

			scoreBox.TextColor = scoreColor;

			var startDateTime = App.Database.GetActivePeriod ().StartDT;
			scorePeriod.Text = "for the period beginning " + startDateTime.ToString ("dddd',' MMM d 'at' HH:mm tt");

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

