using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Diagnostics;

namespace JobSearchScorecard
{
	public class ActivityPage : ContentPage
	{
		ListView listCurrentTasks;
		ListView listTaskHistory;
		Activity theAct;

		public ActivityPage (Activity act)
		{
			theAct = act;
			Title = act.FullName + " (worth " + act.Score.ToString () + " points)";
			Label oneTimeLabel = new Label () { FontAttributes = FontAttributes.Italic, HorizontalOptions = LayoutOptions.CenterAndExpand,};
			if (act.OneTimeOnly) {
				oneTimeLabel.Text = "One-time only, regardless of period";
			}

			// There might be multiple task-completions here, so use a scrolling ListView
			listCurrentTasks = new ListView ();
			listCurrentTasks.ItemTemplate = new DataTemplate (() => {
				var cell = new TextCell ();
				cell.Detail = string.Format("Worth {0} points in current period", theAct.Score);
				cell.SetBinding<Task> (TextCell.TextProperty, t => t.DT);
				return cell;
			});
			listCurrentTasks.ItemSelected += (sender, e) => {
				var theTask = (Task)e.SelectedItem;
				var taskDetailPage = new TaskDetailPage ("Edit current task");
				taskDetailPage.BindingContext = theTask;
				Navigation.PushAsync (taskDetailPage);
			};

			// There might be multiple task-completions here, so use a scrolling ListView
			listTaskHistory = new ListView ();
			listTaskHistory.ItemTemplate = new DataTemplate (() => {
				var cell = new TextCell ();
				cell.DetailColor = Color.Red;
				cell.Detail = "past period, not in current score";
				cell.SetBinding<Task> (TextCell.TextProperty, t => t.DT);
				return cell;
			});
			listTaskHistory.ItemSelected += (sender, e) => {
				var theTask = (Task)e.SelectedItem;
				var taskDetailPage = new TaskDetailPage ("Edit history");
				taskDetailPage.BindingContext = theTask;
				Navigation.PushAsync (taskDetailPage);
			};
				
			var layout = new StackLayout ();
			layout.Children.Add (oneTimeLabel);
			layout.Children.Add (listCurrentTasks);
			var lineSeparator = new BoxView () { Color = Color.Blue, WidthRequest = 100, HeightRequest = 8 };
			layout.Children.Add (lineSeparator);
			layout.Children.Add (listTaskHistory);
			layout.HorizontalOptions = LayoutOptions.Center;
			Content = layout;

		}

		protected override void OnAppearing ()
		{
			IEnumerable<Task> currentTasks;
			IEnumerable<Task> pastPeriodTasks;
			base.OnAppearing ();
			// reset the 'resume' id, since we just want to re-start here
			//((App)App.Current).ResumeAtTodoId = -1;
			var startDateTime = App.Database.GetActivePeriod ().StartDT;

			currentTasks = App.Database.GetAllTasksWithinPeriod ().Where (t => t.SubStep == theAct.SubStep);
			pastPeriodTasks = App.Database.GetTasksBySubStep (theAct.SubStep).Where (t => t.DT < startDateTime);
			var anyAtAll = currentTasks.Any () || pastPeriodTasks.Any ();

			ToolbarItems.Clear ();  // for Android only (somehow the "Add" buttons would accumulate without this statement)

			// Determining whether or not to place an "Add" action button in the urh corner.
			// First: don't add it if it's already there!  Next up:
			// if 1-time-only task, then you can only create it once (regardless of time period), so don't show Add button if already completed
			if (!ToolbarItems.Any () && !theAct.OneTimeOnly || !anyAtAll) {
				ToolbarItem tbi = null;
				tbi = new ToolbarItem ("Add", null, () => {
					var newTask = new Task ((int)theAct.Step, theAct.SubStep, theAct.Score, (theAct.OneTimeOnly ? 1 : 0), DateTime.Now, string.Empty);
					var taskDetailPage = new TaskDetailPage ("Add new (" + theAct.Score.ToString () + " pts)");
					taskDetailPage.BindingContext = newTask;
					Navigation.PushAsync (taskDetailPage);
				}
				);
				ToolbarItems.Add (tbi);
			}

			listCurrentTasks.ItemsSource = currentTasks;
			listTaskHistory.ItemsSource = pastPeriodTasks;

		}
	}
}

