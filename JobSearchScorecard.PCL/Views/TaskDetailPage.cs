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

		public TaskDetailPage (string fullDescription, bool addNew)
		{
			Title = fullDescription;

			NavigationPage.SetHasNavigationBar (this, true);

			var notesLabel = new Label { Text = "Notes about this task (optional):" };
			var notesEntry = new Editor ();

			// cannot get just the right "look" that will be satisfactory for both Android and iOS
			//notesEntry.BackgroundColor = new Color (0, 0x10, 0x10);

			notesEntry.SetBinding (Editor.TextProperty, "Notes");

			var saveButton = new Button { Text = "Save", BorderWidth = 2, };
			saveButton.Clicked += (sender, e) => {
				var taskItem = (Task)BindingContext;
				App.Database.SaveTask (taskItem);
				this.Navigation.PopAsync ();
			};

			// Do not activate DELETE button if you are adding a new task
			var deleteButton = new Button { Text = "Delete", BorderWidth = 2, };
			if (addNew) {
				deleteButton.IsEnabled = false;
			} else {
				deleteButton.Clicked += (sender, e) => {
					var taskItem = (Task)BindingContext;
					App.Database.DeleteTask (taskItem.ID);
					this.Navigation.PopAsync ();
				};
			}
			var speakNotes = new Button { Text = "Listen", BorderWidth = 2, };
			speakNotes.Clicked += (sender, e) => {
				DependencyService.Get<ITextToSpeech> ().Speak (notesEntry.Text);
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
					speakNotes,
					cancelButton,
				}
			};

		}

	}
}