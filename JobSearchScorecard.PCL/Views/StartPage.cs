﻿using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace JobSearchScorecard
{
	public class StartPage : ContentPage
	{

		public List<Task> currentTasks;

		public StartPage ()
		{
			Title = "Job Search Scorecard";
			currentTasks = MockUpTasks ();  // for now, mock-up some tasks completed
			ActivityTable.BuildActivitiesDictionary ();
			Debug.WriteLine ("Built Activities Dictionary with : " + Activity.UniqueCode + " unique entries");


			// Define command for the items in the TableView.
			Command<Steps> navigateCommand = 
				new Command<Steps> (async (Steps step) => {
					System.Diagnostics.Debug.WriteLine ("navigate delegate, pageType=" + step);
					Page page = new StepPage (step);
					await this.Navigation.PushAsync (page);
				});

			var stepsSection = new TableSection ("Steps");
			foreach (var s in StepNames.stepList) {
				stepsSection.Add (new TextCell 
					{ Text = s, Command = navigateCommand, CommandParameter = StepNames.LookUpStepCodeGivenName (s), });
			};

			// Give current score a color coding
			int score = CalculateScore (currentTasks);
			string detailLine = "You're doing well, keep at it!";
			Color scoreColor = Color.Green;
			if (score < 50) {
				scoreColor = Color.Red;
				detailLine = "Uh, oh.  Need to pick it up!";
			};

			var scoreSection = new TableSection ("Score for period beginning " + App.Database.GetActivePeriod ().StartDT);
			scoreSection.Add (new TextCell () {
				Text = score.ToString (),
				TextColor = scoreColor,
				DetailColor = scoreColor,
				Detail = detailLine, 
			});


			var btnNewPeriod = new Button {
				Text = "Start New Period",
				VerticalOptions = LayoutOptions.CenterAndExpand,
				HorizontalOptions = LayoutOptions.StartAndExpand,
				BackgroundColor = Color.Aqua,
			};
			btnNewPeriod.Clicked += async(sender, e) => {
				var action = await DisplayActionSheet ("Start a new period?", "No! Cancel", "YES!");
				if (action.StartsWith("N")) return;
				if (App.Database.SavePeriod () != 1) {
					throw new Exception ("Cannot create new Period!");
				}
			};

			var btnClearDB = new Button { 
				Text = "Clear Entire Database",
				VerticalOptions = LayoutOptions.CenterAndExpand,
				HorizontalOptions = LayoutOptions.StartAndExpand,
				BackgroundColor = Color.Red,
			};
			btnClearDB.Clicked += async (sender, e) => {
				var action = await DisplayActionSheet ("Are you sure you want to delete all data?", 
					"No! Cancel", "YES! Blow it away!");
				if (action.StartsWith("N")) return;
				App.Database.DeleteAll();
			};

			var layout = new StackLayout () { Orientation = StackOrientation.Horizontal, };
			layout.Children.Add (btnNewPeriod);
			var newCalendarPage = new Image { Aspect = Aspect.AspectFit };
			newCalendarPage.Source = ImageSource.FromFile ("newperiod.jpeg");
			layout.Children.Add (newCalendarPage);

			var layout2 = new StackLayout () { Orientation = StackOrientation.Horizontal, };
			layout2.Children.Add (btnClearDB);
			var stopSignSource = new Image { Aspect = Aspect.AspectFit, };
			stopSignSource.Source = ImageSource.FromFile ("stopsign.png");
			layout2.Children.Add (stopSignSource);


			var table = new TableView ();
			table.Intent = TableIntent.Settings;
			table.Root = new TableRoot () {
				stepsSection,
				scoreSection,
				new TableSection("Resets") {
					new ViewCell() { View = layout },
					new ViewCell() { View = layout2 },
				}
			};
			Content = table;
		}

		public List<Task> MockUpTasks ()
		{
			// just for testing.  No database action, just create fake Tasks out of thin air
			// the first argument to Task, the DatabaseID, means nothing here
			var fakeList = new List<Task> () {
				new Task (1000, (int)Steps.All, 0, 0, DateTime.Now, "pray!"),
				new Task (1008, (int)Steps.Attitude, 3, 1, DateTime.Now.AddHours(-3), "Read and list 3 items"),
				new Task (1001, (int)Steps.Attitude, 4, 0, DateTime.Now.AddMinutes (-5), "list negatives"),
				new Task (1002, (int)Steps.Attitude, 11, 0, DateTime.Now.AddDays (-2), "new skills"),
				new Task (1003, (int)Steps.Assessments, 12, 0, DateTime.Now.AddDays (-2), "read docs"),
				new Task (1004, (int)Steps.MarketingStrategy, 27, 0, DateTime.Now.AddDays (-2), "netw handout"),
				new Task (1005, (int)Steps.MarketingStrategy, 30, 0, DateTime.Now.AddDays (-2), "face2face"),
			};
			return fakeList;
		}

		public int CalculateScore (List<Task> tasksAccomplished)
		{
			int tempScore = 0;
			bool foundIt = false;
			Steps step;

			List<Activity> listAtStep;

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
			};
			return tempScore;		
		}


	}
}
