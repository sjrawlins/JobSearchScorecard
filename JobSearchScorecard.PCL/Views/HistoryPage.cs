using System;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Linq;

namespace JobSearchScorecard
{
	public class HistoryPage : ContentPage
	{

		public HistoryPage ()
		{
			Title = "Score History";

			var periodList = new ListView ();
			var lblDesc = new Label ();
			lblDesc.Text = "Score / For Period Beginning";

			periodList.ItemTemplate = new DataTemplate (() => {
				var cell = new TextCell ();
				cell.SetBinding<Period> (TextCell.TextProperty, p => p.Score);
				cell.SetBinding<Period> (TextCell.DetailProperty, p => p.StartDT);
				return cell;
			});

			var periodHistory = App.Database.ListPeriods ();
			periodList.ItemsSource = periodHistory;
		
			var layout = new StackLayout ();
			layout.Children.Add (lblDesc);
			layout.Children.Add (periodList);
			Content = layout;
		}
	}
}

