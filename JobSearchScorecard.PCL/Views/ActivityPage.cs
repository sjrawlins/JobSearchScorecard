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
		string fullDescription;
		Label tapToDeleteOrUpdate;
		Button btnAdd;

		public ActivityPage (Activity act)
		{
			theAct = act;
			Title = act.FullName;
			fullDescription = string.Format ("{0}, worth {1} points", act.FullName, act.Score);
			Label subTitle = new Label () {
				FontAttributes = FontAttributes.Italic,
				HorizontalOptions = LayoutOptions.CenterAndExpand,
			};
			if (act.OneTimeOnly) {
				subTitle.Text = string.Format ("Worth {0} points (one time only)", act.Score);
			} else {
				subTitle.Text = string.Format ("Worth {0} points (each)", act.Score);
			}

			var listTemplate = new DataTemplate (() => {
				var cell = new TextCell ();
				cell.SetBinding<Task> (TextCell.TextProperty, t => t.DT);
				cell.SetBinding<Task> (TextCell.DetailProperty, t => t.Notes);
				return cell;
			});

			// There might be multiple task-completions here, so use a scrolling ListView
			listCurrentTasks = new ListView ();
			listCurrentTasks.ItemTemplate = listTemplate;
			listCurrentTasks.ItemSelected += HandleSelect;

			// There might be multiple task-completions here, so use a scrolling ListView
			listTaskHistory = new ListView ();
			listTaskHistory.ItemTemplate = listTemplate;
			listTaskHistory.ItemSelected += HandleSelect;
		
			btnAdd = new Button () { Text = "Tap to Record Completion", BorderWidth = 2, };
			btnAdd.Clicked += HandleAdd;

			var layout = new StackLayout ();
			layout.Children.Add (subTitle);
			layout.Children.Add (btnAdd);
			tapToDeleteOrUpdate = new Label { Text = "Tap below to Delete or Add Notes", TextColor = Color.Teal, };
			tapToDeleteOrUpdate.IsVisible = false;
			layout.Children.Add (tapToDeleteOrUpdate);

			layout.Children.Add (listCurrentTasks);
			var lineSeparator = new BoxView () { Color = Color.Blue, WidthRequest = 100, HeightRequest = 8 };
			layout.Children.Add (lineSeparator);
			layout.Children.Add (new Label { Text = "History (not included in current score):", TextColor = Color.Teal, });
			layout.Children.Add (listTaskHistory);
			//layout.HorizontalOptions = LayoutOptions.Center;
			Content = layout;

		}

		void HandleSelect (object sender, SelectedItemChangedEventArgs e)
		{
			var theTask = (Task)e.SelectedItem;
			var taskDetailPage = new TaskDetailPage (fullDescription);
			taskDetailPage.BindingContext = theTask;
			Navigation.PushAsync (taskDetailPage);
		}

		void HandleAdd (object sender, EventArgs ea)
		{
			var newTask = new Task ((int)theAct.Step, theAct.SubStep, theAct.Score, (theAct.OneTimeOnly ? 1 : 0), DateTime.Now, null);
			App.Database.SaveTask (newTask);
			this.OnAppearing ();
		}

		protected override void OnAppearing ()
		{
			IEnumerable<Task> currentTasks;
			IEnumerable<Task> pastPeriodTasks;
			base.OnAppearing ();

			var startDateTime = App.Database.GetActivePeriod ().StartDT;

			currentTasks = App.Database.GetCurrentTasksBySubStep (theAct.SubStep);
			// Reveal (or hide) the "Tap below" label depending on whether or not there are current-period tasks 

	        tapToDeleteOrUpdate.IsVisible = currentTasks.Any ();

			pastPeriodTasks = App.Database.GetTasksBySubStep (theAct.SubStep).Where (t => t.DT < startDateTime);

			// Tricky.  Show the "Add" button only if it is "allowed".  A 1-time-only task completion can
			// only be recorded once, in current (or history)
			btnAdd.IsEnabled = (!currentTasks.Any() && !pastPeriodTasks.Any()) || !theAct.OneTimeOnly;

			listCurrentTasks.ItemsSource = currentTasks;
			listTaskHistory.ItemsSource = pastPeriodTasks;

		}
	}
}

