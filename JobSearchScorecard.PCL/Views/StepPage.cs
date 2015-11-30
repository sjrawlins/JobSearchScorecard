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
		}

		public StepPage (Steps step)
		{
			Title = string.Format("{0}: Pick a Task", StepNames.LookUpStepNameGivenCode (step));
			Debug.WriteLine ("Screen for step: " + step);
			theStep = step;

			lblStepTotal = new Label () { 
				FontSize = 14, FontAttributes = FontAttributes.Italic, HorizontalOptions = LayoutOptions.Center,
				TextColor = Color.Red,
			};

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

			//Title = string.Format("{0} ({1} points earned)", StepNames.LookUpStepNameGivenCode (theStep), ListSubTasks ());
		}

		protected override void OnAppearing ()
		{
			// re-calc the subscore
			base.OnAppearing ();
			ListSubTasks ();
		}

		private void ListSubTasks ()
		{
			string detailLine;
			int stepTotal = 0;

			ts.Clear ();
			tableRoot.Clear ();

			var activityList = ActivityTable.Activities [theStep];
			foreach (var act in activityList) {
				int subScore = StartPage.CalculateScore (App.Database.GetCurrentTasksBySubStep (act.SubStep));
				stepTotal += subScore;
				detailLine = string.Empty;
				if (subScore > 0) {
					if (act.OneTimeOnly) {
						detailLine = string.Format ("({0} points, 1-time only task)", subScore);
					} else {
						detailLine = string.Format ("({0} points earned)", subScore);
					}
				}
			
				ts.Add (new TextCell {
					Text = string.Format("{0} (worth {1} points)", act.FullName, act.Score),
					Detail = detailLine,
					DetailColor = Color.Red,
					Command = navigateCommand, CommandParameter = act, 
				});
			}
			;
			tableRoot.Add (ts);
			tableView.Root = tableRoot;
			if (stepTotal > 0)
				lblStepTotal.Text = string.Format ("{0} points earned for this step", stepTotal);

			// Build the page.
			this.Content = new StackLayout {
				Children = {
					lblStepTotal,
					tableView
				}
			};
		}
	}
}