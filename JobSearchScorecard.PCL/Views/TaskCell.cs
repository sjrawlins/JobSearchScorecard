using System;
using Xamarin.Forms;

namespace JobSearchScorecard
{
	public class StepCell : ViewCell
	{
		public StepCell (Activity act)
		{
			var label = new Label {
				Text = act.FullName,
				YAlign = TextAlignment.Center

			};
			//label.SetBinding (Label.TextProperty, "Name");

			var layout = new StackLayout {
				Padding = new Thickness(20, 0, 0, 0),
				Orientation = StackOrientation.Horizontal,
				HorizontalOptions = LayoutOptions.StartAndExpand,
				Children = {label,}
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

