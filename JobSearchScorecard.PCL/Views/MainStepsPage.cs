using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace JobSearchScorecard
{
	public class MainStepsPage : ContentPage
	{
		private Command<Steps> navigateCommand;

		public MainStepsPage ()
		{
			Title = "Pick a Category";

			// Define command for the items in the TableView.
			navigateCommand = 
				new Command<Steps> (async (Steps step) => {
					Debug.WriteLine ("navigate to StepPage for step:" + step);
					Page page = new StepPage (step);
					await this.Navigation.PushAsync (page);
				});

			RenderTable ();
		}

		protected override void OnAppearing ()
		{
			//base.OnAppearing ();
			RenderTable();

		}

		private void RenderTable()
		{
			string strCatScore;
			int intCatScore;
			var stepsSection = new TableSection ();
			foreach (var s in StepNames.StepDictionary) {
				var currentTasksForThisStep = App.Database.GetCurrentTasksByStep (s.Key); 
				intCatScore = StartPage.CalculateScore (currentTasksForThisStep);
				strCatScore = intCatScore > 0 ? string.Format ("({0} points earned)", intCatScore) : string.Empty;
				stepsSection.Add (new TextCell { Text = s.Value, Detail = strCatScore, DetailColor = Color.Red,
					Command = navigateCommand, CommandParameter = s.Key,
				});
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