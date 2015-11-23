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
			Title = "Period History & Scores";

			var periodList = new ListView ();

			periodList.ItemTemplate = new DataTemplate (() => {
				var cell = new TextCell ();
				cell.SetBinding<Period> (TextCell.TextProperty, p => p.StartDT);
				cell.SetBinding<Period> (TextCell.DetailProperty, p => p.Score);
				return cell;
			});

			var periodHistory = App.Database.ListPeriods ();
			periodList.ItemsSource = periodHistory;
		
			var layout = new StackLayout ();
			layout.Children.Add (periodList);
			Content = layout;
		}
	}
}

