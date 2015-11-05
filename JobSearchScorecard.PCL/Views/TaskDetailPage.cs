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
		public TaskDetailPage (Task theTask)
		{
			this.SetBinding (ContentPage.TitleProperty, "Name");

			NavigationPage.SetHasNavigationBar (this, true);

			var notesLabel = new Label { Text = "Notes" };
			var notesEntry = new Entry ();
			notesEntry.SetBinding (Entry.TextProperty, "Notes");

			var saveButton = new Button { Text = "Save" };
			saveButton.Clicked += (sender, e) => {
				var taskItem = (Task)BindingContext;
				App.Database.SaveTask(taskItem);
				this.Navigation.PopAsync();
			};

			var deleteButton = new Button { Text = "Delete" };
			deleteButton.Clicked += (sender, e) => {
				var taskItem = (Task)BindingContext;
				Debug.WriteLine("Delete the DB task with ID="+taskItem.ID);
				App.Database.DeleteTask(taskItem.ID);
				this.Navigation.PopAsync();
			};

			var cancelButton = new Button { Text = "Cancel" };
			cancelButton.Clicked += (sender, e) => {
				//var taskItem = (Task)BindingContext;   WHY EVEN HAVE THIS LINE ON A CANCEL???
				this.Navigation.PopAsync();
			};

			Content = new StackLayout {
				VerticalOptions = LayoutOptions.StartAndExpand,
				Padding = new Thickness(20),
				Children = {
					notesLabel, notesEntry,
					saveButton, deleteButton, cancelButton,
				}
			};

		}
	}
}