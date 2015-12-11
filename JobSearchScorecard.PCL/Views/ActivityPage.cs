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

		public ActivityPage (Activity act)
		{
			theAct = act;
			Title = act.FullName;
			fullDescription = string.Format ("{0}, worth {1} points", act.FullName, act.Score);
			Label subTitle = new Label () {
				FontAttributes = FontAttributes.Italic,
				HorizontalOptions = LayoutOptions.CenterAndExpand,
			};
			subTitle.Text = string.Format ("Worth {0} points {1}", act.Score, act.OneTimeOnly ? "(one-time only)" : string.Empty);

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
		
			var btnAdd = new Button () { Text = "Add a new task completion", };
			btnAdd.Clicked += HandleAdd;
			var btnSpeakTask = new Button { Text = "Listen", BorderWidth = 2, };
			btnSpeakTask.Clicked += (sender, e) => {
				DependencyService.Get<ITextToSpeech> ().Speak (fullDescription);
			};
			var layout = new StackLayout ();
			layout.Children.Add (subTitle);
			layout.Children.Add (btnAdd);
			layout.Children.Add (btnSpeakTask);
			layout.Children.Add (listCurrentTasks);
			var lineSeparator = new BoxView () { Color = Color.Blue, WidthRequest = 100, HeightRequest = 8 };
			layout.Children.Add (lineSeparator);
			layout.Children.Add (new Label { Text = "History (not included in current score):", TextColor = Color.Teal, });
			layout.Children.Add (listTaskHistory);
			layout.HorizontalOptions = LayoutOptions.Center;
			Content = layout;

		}
			
		void HandleSelect (object sender, SelectedItemChangedEventArgs e)
		{
			var theTask = (Task)e.SelectedItem;
			var taskDetailPage = new TaskDetailPage (fullDescription, false);
			taskDetailPage.BindingContext = theTask;
			Navigation.PushAsync (taskDetailPage);
		}

		void HandleAdd (object sender, EventArgs ea)
		{
			var newTask = new Task ((int)theAct.Step, theAct.SubStep, theAct.Score, (theAct.OneTimeOnly ? 1 : 0), DateTime.Now, null);
			var taskDetailPage = new TaskDetailPage (fullDescription, true);
			taskDetailPage.BindingContext = newTask;
			Navigation.PushAsync (taskDetailPage);
		}

		protected override void OnAppearing ()
		{
			IEnumerable<Task> currentTasks;
			IEnumerable<Task> pastPeriodTasks;
			base.OnAppearing ();
			// reset the 'resume' id, since we just want to re-start here
			//((App)App.Current).ResumeAtTodoId = -1;
			var startDateTime = App.Database.GetActivePeriod ().StartDT;

			currentTasks = App.Database.GetCurrentTasksBySubStep (theAct.SubStep);
			pastPeriodTasks = App.Database.GetTasksBySubStep (theAct.SubStep).Where (t => t.DT < startDateTime);
			var anyAtAll = currentTasks.Any () || pastPeriodTasks.Any ();

			ToolbarItems.Clear ();  // for Android only (somehow the "Add" buttons would accumulate without this statement)

			// Determining whether or not to place an "Add" action button in the urh corner.
			// First: don't add it if it's already there!  Next up:
			// if 1-time-only task, then you can only create it once (regardless of time period), so don't show Add button if already completed
//			if (!ToolbarItems.Any () && !theAct.OneTimeOnly || !anyAtAll) {
//				ToolbarItem tbi = null;
//				tbi = new ToolbarItem ("Add", null, HandleAdd
//
////					() => {
////					var newTask = new Task ((int)theAct.Step, theAct.SubStep, theAct.Score, (theAct.OneTimeOnly ? 1 : 0), DateTime.Now, null);
////					var taskDetailPage = new TaskDetailPage (fullDescription, true);
////					taskDetailPage.BindingContext = newTask;
////					Navigation.PushAsync (taskDetailPage);
////				}
//				);
//				ToolbarItems.Add (tbi);
//			}

			listCurrentTasks.ItemsSource = currentTasks;
			listTaskHistory.ItemsSource = pastPeriodTasks;

		}
	}
}

