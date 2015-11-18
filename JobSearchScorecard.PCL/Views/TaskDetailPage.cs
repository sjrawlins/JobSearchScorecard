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
		Editor notesEntry = null;

		public TaskDetailPage (string pageTitle)
		{
			Title = pageTitle;

			NavigationPage.SetHasNavigationBar (this, true);

			var notesLabel = new Label { Text = "Notes about this task (optional):" };
			notesEntry = new Editor ();

			// cannot get just the right "look" that will be satisfactory for both Android and iOS
			//notesEntry.BackgroundColor = new Color (0, 0x10, 0x10);

			notesEntry.SetBinding (Editor.TextProperty, "Notes");

			var saveButton = new Button { Text = "Save", BorderWidth = 2, };
			saveButton.Clicked += (sender, e) => {
				var taskItem = (Task)BindingContext;
				App.Database.SaveTask(taskItem);
				this.Navigation.PopAsync();
			};

			var deleteButton = new Button { Text = "Delete", BorderWidth = 2, };
			deleteButton.Clicked += (sender, e) => {
				var taskItem = (Task)BindingContext;
				App.Database.DeleteTask(taskItem.ID);
				this.Navigation.PopAsync();
			};

			var cancelButton = new Button { Text = "Cancel", BorderWidth = 2, };
			cancelButton.Clicked += (sender, e) => {
				//var taskItem = (Task)BindingContext;   WHY EVEN HAVE THIS LINE ON A CANCEL???
				this.Navigation.PopAsync();
			};

			var lineSeparator = new BoxView () { Color = Color.Black, WidthRequest = 100, HeightRequest = 2 };

			Content = new StackLayout {
				//VerticalOptions = LayoutOptions.StartAndExpand,
				Padding = new Thickness(20),
				Children = {
					saveButton, deleteButton, cancelButton,
					lineSeparator,
					notesLabel, notesEntry,
				}
			};

		}

	}
}