using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace JobSearchScorecard
{
	public class MainStepsPage : ContentPage
	{
		public MainStepsPage ()
		{
			Title = "Main Step Categories";

			// Define command for the items in the TableView.
			Command<Steps> navigateCommand = 
				new Command<Steps> (async (Steps step) => {
					Debug.WriteLine ("navigate to StepPage for step:" + step);
					Page page = new StepPage (step);
					await this.Navigation.PushAsync (page);
				});

			var stepsSection = new TableSection ();
			foreach (var s in StepNames.stepList) {
				stepsSection.Add (new TextCell 
					{ Text = s, Command = navigateCommand, CommandParameter = StepNames.LookUpStepCodeGivenName (s), });
			};

			this.Content = new TableView {
				Root = new TableRoot {
					stepsSection,
				},
				Intent = TableIntent.Menu,
			};

		}
	}
}