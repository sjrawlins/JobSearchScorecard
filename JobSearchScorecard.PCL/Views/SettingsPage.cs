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
		Entry nameEntry = null;

		public SettingsPage (bool firstTimePrompt)
		{
			if (firstTimePrompt) {
				Title = "Welcome.  Please enter your name and tap 'Save'";
			} else {
				Title = "Update Settings";
			}
				
			// see if the following puts the "back arrow" on Droid:
			//NavigationPage.SetHasNavigationBar (this, true);
			Padding = new Thickness (5, Device.OnPlatform (20, 0, 0), 5, 0);

			var entryStyle = new Style (typeof(Entry)) {
				Setters = {
					new Setter { Property = Entry.FontAttributesProperty, Value = FontAttributes.Bold, },
					new Setter { Property = Entry.FontSizeProperty, Value = 22, },
				}
			};

			var nameLabel = new Label () {
				Text = "Name:", 
				//WidthRequest = 200
			};
			nameEntry = new Entry { Style = entryStyle,  WidthRequest = 200, };
			nameEntry.SetBinding (Entry.TextProperty, "Name");
			if (String.IsNullOrWhiteSpace (nameEntry.Text))
				nameEntry.Placeholder = "Your name here";

			var nameStack = new StackLayout () {
				Orientation = StackOrientation.Horizontal,
				Children = { nameLabel, nameEntry }
			};
					
			var greenThresholdLabel = new Label () {
				Text = "Red-to-green Score threshold:", 
				//WidthRequest = 200
			};
			var greenThresholdEntry = new Entry { Keyboard = Keyboard.Numeric, Style = entryStyle, };
			greenThresholdEntry.SetBinding (Entry.TextProperty, "GreenThreshold");
			var greenStack = new StackLayout () {
				Orientation = StackOrientation.Horizontal,
				Children = { greenThresholdLabel, greenThresholdEntry }
			};

//			var starIncrementLabel = new Label () {
//				Text = "Star increment:", 
//				//WidthRequest = 200
//			};
//			var starIncrementEntry = new Entry { Keyboard = Keyboard.Numeric, Style = entryStyle, };
//			starIncrementEntry.SetBinding (Entry.TextProperty, "StarIncrement");
//			var starIncrementStack = new StackLayout () {
//				Orientation = StackOrientation.Horizontal,
//				Children = { starIncrementLabel, starIncrementEntry }
//			};
			
			var saveButton = new Button { Text = "Save", BorderWidth = 2, };
			saveButton.Clicked += (sender, e) => {
				var settings = (Settings)BindingContext;
				App.Database.SaveSettings (settings);
				if (firstTimePrompt) {
					var startPage = new StartPage();
					var rootPage = new NavigationPage(startPage);
					this.Navigation.PushModalAsync (rootPage);
					//this.Navigation.PushModalAsync (new StartPage ());
				} else {
					MessagingCenter.Send<SettingsPage>(this, "popped");
					this.Navigation.PopModalAsync();
				}
			};

			var cancelButton = new Button { Text = "Cancel", BorderWidth = 2, };
			cancelButton.Clicked += (sender, e) => {
				this.Navigation.PopModalAsync();
				//this.Navigation.PopAsync ();
			};
				
			var resetButton = new Button { 
				Text = "Clear Entire Database", TextColor = Color.Red, BorderWidth = 2, 
			};
			resetButton.Clicked += async (sender, e) => {
				var action = await DisplayActionSheet ("Are you sure you want to delete all data?", 
					             "No", "Yes, wipe clean!");
				if (action.StartsWith ("N"))
					return;
				App.Database.DeleteAll ();
				MessagingCenter.Send<SettingsPage>(this, "popped");
				this.OnAppearing ();
			};

			if (firstTimePrompt) {
				nameEntry.Focus ();
				Content = new StackLayout {
					Children = {
						nameStack,
						saveButton,
					}
				};
			} else {
				Content = new StackLayout {
					//Padding = new Thickness (5),
					Children = {
						nameStack,
						greenStack,
						//starIncrementStack,
						saveButton, 
						cancelButton,
						resetButton,
					}
				};
			}
		}

		protected override void OnAppearing ()
		{
			base.OnAppearing ();

			var settings = (Settings)BindingContext;

			if (String.IsNullOrWhiteSpace (settings.Name)) {

				Debug.WriteLine ("SettingsPage.OnAppearing: force name prompt");
				nameEntry.Focus ();
				DisplayAlert ("Welcome, Job Seeker!", "Let's get started.  Please enter your Name and tap Save", "OK, Got it");
			}
		}

		protected override bool OnBackButtonPressed ()
		{
			return base.OnBackButtonPressed ();
		}
	}
}

