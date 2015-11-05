using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace JobSearchScorecard
{
	public class TaskCell : ViewCell
	{
		public DateTime TaskDate;
		public TaskCell ()
		{
			var taskDate = new Label {
				Text = TaskDate.ToString ("f"),
				YAlign = TextAlignment.Center,
			};

			var layout = new StackLayout {
				Padding = new Thickness(20, 0, 0, 0),
				Orientation = StackOrientation.Horizontal,
				HorizontalOptions = LayoutOptions.StartAndExpand,
				Children = {
					taskDate,
				}
			};
			View = layout;
		}

		protected override void OnBindingContextChanged ()
		{
			// Fixme : this is happening because the View.Parent is getting 
			// set after the Cell gets the binding context set on it. Then it is inheriting
			// the parents binding context.
			View.BindingContext = BindingContext;
			base.OnBindingContextChanged ();
		}
	}
}

