using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Diagnostics;

namespace JobSearchScorecard
{
	public class AddTaskPage : ContentPage
	{
		public AddTaskPage (Activity act)
		{
			Title = act.FullName;

			TableRoot tableRoot = new TableRoot ("TableRoot");
			TableSection ts = new TableSection ();
		
			var notesText = new EntryCell ();
			ts.Add (notesText);  // 1-line text entry for NOTES

			tableRoot.Add (ts);

			TableView tableView = new TableView {
				Intent = TableIntent.Form,
				Root = tableRoot,
			};

			Button storeIt = new Button ();
			storeIt.Text = "SAVE";
			storeIt.Command = new Command (() => 
				App.Database.SaveTask (new Task ((int)act.Step, act.SubStep, act.Score, 0, DateTime.Now, notesText.Text)));

			// Build the page.
			this.Content = new StackLayout {
				Children = {
					tableView,
					//notesText,
					//storeIt,
				}
			};
		}
	}
}