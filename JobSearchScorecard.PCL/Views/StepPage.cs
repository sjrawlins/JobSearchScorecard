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
			TableRoot tableRoot = new TableRoot ("TableRoot");
			TableSection ts = new TableSection ();

			var activityList = ActivityTable.Activities [step];

			foreach (var act in activityList) {
				ts.Add (new StepCell (act));
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