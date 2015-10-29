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
		public StepPage ()
		{
			Title = "Unknown Step";
		}

		public StepPage (Steps step)
		{
			Title = StepNames.LookUpStepNameGivenCode (step);
			Debug.WriteLine ("Screen for step: " + step);
			TableRoot tableRoot = new TableRoot ("TableRoot");
			TableSection ts = new TableSection ();

			// Define command for the items in the TableView.
			var navigateCommand = 
				new Command<Activity> (async (Activity a) => {
					Debug.WriteLine ("navigate to Activity =" + a.FullName);
					Page page = new ActivityPage (a);
					await this.Navigation.PushAsync (page);
				});
					
			var activityList = ActivityTable.Activities [step];
			foreach (var act in activityList) {
				var points = "(" + act.Score.ToString () + " points)";
				ts.Add (new TextCell {
					Text = act.FullName,
					Detail = points,
					DetailColor = Color.Accent,
					Command = navigateCommand, CommandParameter = act, 
				});
			}
			tableRoot.Add (ts);

			TableView tableView = new TableView {
				Intent = TableIntent.Form,
				Root = tableRoot,
			};

			// Build the page.
			this.Content = new StackLayout {
				Children = {
					tableView
				}
			};
		}
	}
}