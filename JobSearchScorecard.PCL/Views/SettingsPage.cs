using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Diagnostics;

namespace JobSearchScorecard
{
	public class SettingsPage : ContentPage
	{
		Settings settings;
		public SettingsPage ()
		{
			Title = "Update App Settings";

			settings = App.Database.GetSettings ();
			if (settings == null)
				settings = new Settings ();  // 1st time

			var nameEntry = new Entry { Placeholder = "Your name here", };
			nameEntry.SetBinding (Entry.TextProperty, "Name");

			var emailAddr = new Entry { Keyboard = Keyboard.Email, Placeholder = "your email address here", };
				//BindingContext = settings.EmailAddress, };
			emailAddr.SetBinding (Entry.TextProperty, "EmailAddress");
			
			var saveButton = new Button { Text = "Save", BorderWidth = 2, };
			saveButton.Clicked += (sender, e) => {
				//var settings = (Settings)BindingContext;
				App.Database.SaveSettings (settings);
				this.Navigation.PopAsync ();
			};

			var cancelButton = new Button { Text = "Cancel", BorderWidth = 2, };
			cancelButton.Clicked += (sender, e) => {
				this.Navigation.PopAsync ();
			};

			Content = new StackLayout {
				Padding = new Thickness (5),
				Children = {
					nameEntry,
					emailAddr,
					saveButton, 
					cancelButton,
				}
			};
		}
	}
}

