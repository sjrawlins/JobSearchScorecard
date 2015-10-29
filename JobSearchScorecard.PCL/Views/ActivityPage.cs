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
		Label label;
		public ActivityPage (Activity act)
		{
			var theView = new ScrollView ();
			var stack = new StackLayout {
				Padding = new Thickness (20),
			};

			Label header = new Label
			{
				Text = act.FullName + " (worth " + act.Score.ToString() + " points)",
				FontAttributes = FontAttributes.Bold,
				FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
				HorizontalOptions = LayoutOptions.Center
			};
			stack.Children.Add (header);
		
			label = new Label
			{
				FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
				HorizontalOptions = LayoutOptions.Center,
				//VerticalOptions = LayoutOptions.CenterAndExpand
			};
			stack.Children.Add (label);

			if (act.OneTimeOnly)  // One-time-only tasks: you only get one Task completion, one score
			{
				// is the one-time-only Task in the DB (i.e. has it already been completed)?
				var loggedTasks = App.Database.GetTaskByUniqueStepNum(act.SubStep);
				if (loggedTasks == null) {
					throw new ArgumentNullException ("null loggedTasks in ActivityPage.cs");
				}
				Switch switcher = new Switch() {
					HorizontalOptions = LayoutOptions.Center,
					VerticalOptions = LayoutOptions.CenterAndExpand
				};
				if (!loggedTasks.Any()) {  // Not currently in DB, so it's eligible for completion
					label.Text = "Slide switch ON to record this task";
					switcher.IsToggled = false;
					switcher.Toggled += (object sender, ToggledEventArgs e) => {
						Debug.WriteLine ("in Switch event");
						if (e.Value) {
							label.Text = "Done. Remember, this is a one-time-only score.";
						} else {
							label.Text = "OK, it's off now";
						}
						;
					};
					App.Database.SaveTask(new Task((int)act.Step, act.SubStep, act.Score, 1, DateTime.Now, "Inserted 1-time only task from UI ActivityPage"));
				} else {  // this one-time-only task has already been recorded, so show proof
					switcher.IsToggled = true;
					var theTask = loggedTasks.First();
					stack.Children.Add(new Label() { Text = "Slide switch OFF to remove this task"} );
					label.Text = "Task Completion recorded on: " +
						theTask.DT.Date.ToString ("f");  // C# preset format
				}
				stack.Children.Add (switcher);
				theView.Content = stack;
			}
			else {  // could be many task entries here, so better have scrollable list
				var taskList = App.Database.GetAllTasksWithinPeriod().Where(t => t.SubStep == act.SubStep);
				if (taskList == null) {
					throw new NullReferenceException ("There should at least be an IEnumerable of Tasks!");
				}
				bool existingTasks = taskList.Any();

				///TEST
				/// 
				ToolbarItems.Add(new ToolbarItem {
					Name = "Add",
					Order = ToolbarItemOrder.Primary,
					Command = new Command(() => Navigation.PushAsync(new AddTaskPage(act)))
				});
				////Test
				ToolbarItems.Add (new ToolbarItem {
				    Name = "Add2",
					Order = ToolbarItemOrder.Secondary,
					Command = new Command (() => 
						App.Database.SaveTask(new Task((int)act.Step, act.SubStep, act.Score, 0, DateTime.Now, "Inserted from UI ActivityPage")))
				});

				if (existingTasks) {
					label.Text = "Completed tasks:";
					foreach (var t in taskList) {
						stack.Children.Add (new Label () { Text = t.DT.ToString ("f"), });
					}
					ToolbarItems.Add (new ToolbarItem ("Delete", "Delete.png", async () => {
						Debug.WriteLine ("delete a task");
					}
					));
				} else {
					label.Text = "Add a task completion";
				}

			    stack.Children.Add (new Button { Text = "Dummy Placeholder"});
				theView.Content = stack;
			};

			// Build the page.
			this.Content = theView;

		}
	}
}

