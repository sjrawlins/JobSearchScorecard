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

		public ActivityPage (Activity act)
		{
			Title = act.FullName + " (worth " + act.Score.ToString () + " points)";

//			Label header = new Label {
//				Text = act.FullName + " (worth " + act.Score.ToString () + " points)",
//				FontAttributes = FontAttributes.Bold,
//				FontSize = Device.GetNamedSize (NamedSize.Large, typeof(Label)),
//				HorizontalOptions = LayoutOptions.Center
//			};
//			stack.Children.Add (header);
		
//			label = new Label {
//				FontSize = Device.GetNamedSize (NamedSize.Medium, typeof(Label)),
//				HorizontalOptions = LayoutOptions.Center,
//				//VerticalOptions = LayoutOptions.CenterAndExpand
//			};
//			stack.Children.Add (label);

//			if (act.OneTimeOnly) {  // One-time-only tasks: you only get one Task completion, one score
//				// is the one-time-only Task in the DB (i.e. has it already been completed)?
//				var loggedTasks = App.Database.GetTaskByUniqueStepNum (act.SubStep);
//				if (loggedTasks == null) {
//					throw new ArgumentNullException ("null loggedTasks in ActivityPage.cs");
//				}
//				Switch switcher = new Switch () {
//					HorizontalOptions = LayoutOptions.Center,
//					VerticalOptions = LayoutOptions.CenterAndExpand
//				};
//				if (!loggedTasks.Any ()) {  // Not currently in DB, so it's eligible for completion
//					label.Text = "Slide switch ON to record this task";
//					switcher.IsToggled = false;
//					switcher.Toggled += (object sender, ToggledEventArgs e) => {
//						Debug.WriteLine ("in Switch event");
//						if (e.Value) {
//							label.Text = "Done. Remember, this is a one-time-only score.";
//						} else {
//							label.Text = "OK, it's off now";
//						}
//						;
//					};
//					App.Database.SaveTask (new Task ((int)act.Step, act.SubStep, act.Score, 1, DateTime.Now, "Inserted 1-time only task from UI ActivityPage"));
//				} else {  // this one-time-only task has already been recorded, so show proof
//					switcher.IsToggled = true;
//					var theTask = loggedTasks.First ();
//					stack.Children.Add (new Label () { Text = "Slide switch OFF to remove this task" });
//					label.Text = "Task Completion recorded on: " +
//					theTask.DT.Date.ToString ("f");  // C# preset format
//				}
//				stack.Children.Add (switcher);
//				theView.Content = stack;
//			} else {  
			// Allow multiple task-completions here, so must use a scrolling ListView
			var listView = new ListView ();
			listView.ItemsSource = App.Database.GetAllTasksWithinPeriod ().Where (t => t.SubStep == act.SubStep);
			listView.ItemTemplate = new DataTemplate (() => {
				var cell = new TextCell ();
				cell.SetBinding<Task> (TextCell.TextProperty, t => t.DT);
				return cell;
				//(typeof (TaskCell)
			});
			listView.ItemSelected += (sender, e) => {
				var theTask = (Task)e.SelectedItem;
				var taskDetailPage = new TaskDetailPage (theTask);
				taskDetailPage.BindingContext = theTask;
				Navigation.PushAsync (taskDetailPage);
			};

			var layout = new StackLayout ();
			layout.Children.Add (listView);
			layout.VerticalOptions = LayoutOptions.FillAndExpand;
			Content = layout;

			#region toolbar
			ToolbarItem tbi = null;
			tbi = new ToolbarItem ("+", null, () => {
				var newTask = new Task((int) act.Step, act.SubStep, act.Score, (act.OneTimeOnly ? 1 : 0), DateTime.Now, "placeholder");
				//new Task (1008, (int)Steps.Attitude, 3, 5, 1, DateTime.Now.AddHours (-3), "Read and list 3 items")
				var taskDetailPage = new TaskDetailPage (newTask);
				taskDetailPage.BindingContext = newTask;
				Navigation.PushAsync (taskDetailPage);
			}
			);
			ToolbarItems.Add (tbi);
			#endregion

		}

	}
}

