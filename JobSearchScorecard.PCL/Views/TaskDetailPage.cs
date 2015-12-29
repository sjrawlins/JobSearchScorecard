using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Diagnostics;

namespace JobSearchScorecard
{
	public class TaskDetailPage : ContentPage
	{

		public TaskDetailPage (string fullDescription)
		{
			Title = fullDescription;

			NavigationPage.SetHasNavigationBar (this, true);

			var notesLabel = new Label { Text = "You can add notes about this Task Completion here:" };
			var notesEntry = new Editor ();

			// cannot get just the right "look" that will be satisfactory for both Android and iOS
			//notesEntry.BackgroundColor = new Color (0, 0x10, 0x10);

			notesEntry.SetBinding (Editor.TextProperty, "Notes");

			var saveButton = new Button { Text = "Save Notes", BorderWidth = 2, };
			saveButton.Clicked += (sender, e) => {
				var taskItem = (Task)BindingContext;
				App.Database.SaveTask (taskItem);
				this.Navigation.PopAsync ();
			};

			var deleteButton = new Button { Text = "Delete this entry and deduct points", BorderWidth = 2, };

			deleteButton.Clicked += (sender, e) => {
				var taskItem = (Task)BindingContext;
				App.Database.DeleteTask (taskItem.ID);
				this.Navigation.PopAsync ();
			};

			var cancelButton = new Button { Text = "Cancel", BorderWidth = 2, };
			cancelButton.Clicked += (sender, e) => {
				this.Navigation.PopAsync ();
			};

			var lineSeparator = new BoxView () { Color = Color.Black, WidthRequest = 100, HeightRequest = 2 };

			Content = new StackLayout {
				//Orientation = StackOrientation.Horizontal,
				//HorizontalOptions = LayoutOptions.Center,

				Padding = new Thickness (5),
				Children = {
					notesLabel, notesEntry,
					lineSeparator,
					saveButton, 
					deleteButton,
					//	speakNotes,
					cancelButton,
				}
			};

		}

	}
}