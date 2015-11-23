using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Diagnostics;

namespace JobSearchScorecard
{
	public class StepPage : ContentPage
	{
		TableSection ts;
		Command<Activity> navigateCommand;
		Label lblStepTotal;
		Steps theStep;
		TableRoot tableRoot;
		TableView tableView;

		public StepPage ()
		{
			Title = "Unknown Step";
		}

		public StepPage (Steps step)
		{
			Title = StepNames.LookUpStepNameGivenCode (step);
			Debug.WriteLine ("Screen for step: " + step);
			theStep = step;

			lblStepTotal = new Label () { 
				FontSize = 14, FontAttributes = FontAttributes.Italic, HorizontalOptions = LayoutOptions.Center,};

			tableRoot = new TableRoot ("TableRoot");
			ts = new TableSection ();

			tableView = new TableView {
				Intent = TableIntent.Form,
				Root = tableRoot,
			};

			// Define command for the items in the TableView.
			navigateCommand = 
				new Command<Activity> (async (Activity a) => {
					Debug.WriteLine ("navigate to Activity =" + a.FullName);
					Page page = new ActivityPage (a);
					await this.Navigation.PushAsync (page);
				});
					
			ListSubTasks ();

			Title = string.Format("{0} ({1} points earned)", StepNames.LookUpStepNameGivenCode (theStep), ListSubTasks ());
		}
		protected override void OnAppearing ()
		{
			// re-calc the subscore
			base.OnAppearing ();
			Title = string.Format("{0} ({1} points earned)", StepNames.LookUpStepNameGivenCode (theStep), ListSubTasks ());

		}

		private int ListSubTasks()
		{
			string detailLine;
			ts.Clear ();
			tableRoot.Clear ();
			var totalStepScore = 0;
			var activityList = ActivityTable.Activities [theStep];
			foreach (var act in activityList) {
				int subScore= StartPage.CalculateScore(App.Database.GetCurrentTasksBySubStep (act.SubStep));
				totalStepScore = totalStepScore + subScore;
				if (act.OneTimeOnly) {
					if (subScore > 0) {
						detailLine = string.Format ("(You've earned your 1-time {0} points)", subScore);
					} else {
						detailLine = string.Format ("(1-time chance for {0} points)", act.Score);
					}
				} else {
					detailLine = string.Format ("({0} points each, earned {1} so far)", act.Score, subScore);
				}
				//detailLine = string.Format ("(worth {0} each, earned {1} points so far{2})", act.Score, subScore, act.OneTimeOnly ? ", One Time Only" : string.Empty);
				ts.Add (new TextCell {
					Text = act.FullName,
					Detail = detailLine,
					Command = navigateCommand, CommandParameter = act, 
				});
			};
			tableRoot.Add (ts);
			tableView.Root = tableRoot;
			// Build the page.
			this.Content = new StackLayout {
				Children = {
					lblStepTotal,
					tableView
				}
			};
			return totalStepScore;
		}
	}
}