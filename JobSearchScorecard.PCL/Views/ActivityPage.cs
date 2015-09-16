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
				Text = act.FullName,
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
				if (!loggedTasks.Any()) {  // Not currently in DB, so it's eligible for completion
					label.Text = "Slide switch ON to record this task";
					Switch switcher = new Switch {
						HorizontalOptions = LayoutOptions.Center,
						VerticalOptions = LayoutOptions.CenterAndExpand
					};
					switcher.Toggled += (object sender, ToggledEventArgs e) => {
						Debug.WriteLine ("in Switch event");
						if (e.Value) {
							label.Text = "OK. This is a one-time-only score.";
						} else {
							label.Text = "Changed your mind? OK, it's off now";
						}
						;
					};
					stack.Children.Add (switcher);
				} else {  // this one-time-only task has already been recorded, so show proof
					var theTask = loggedTasks.First();
					label.Text = "Sorry, you already accomplished this task on: " +
						theTask.DT.Date.ToString ("D");  // D=long date format
				}
				theView.Content = stack;
			}
			else {  // could be many task entries here, so better have scrollable list
				label.Text = "Show all task completions in scrollable list";
				for (var i=0; i<100; i++)
					stack.Children.Add (new Button { Text = "Foo"});
				theView.Content = stack;
			};

			// Build the page.
			this.Content = theView;

		}
	}
}

