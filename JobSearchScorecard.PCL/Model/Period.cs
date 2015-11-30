using System;
using SQLite;

namespace JobSearchScorecard
{
	// Period over which scores are summed.  When user chooses to start a new period, create a
	// new row in this table.  There should always be one, and only one, row whose End is in the future,
	// and that row represents the current period.
	public class Period
	{
		[PrimaryKey, AutoIncrement]
		public int ID { get; set; } // unique row ID from Database, thus a DB/SQL concept only and not part of the app model
		public DateTime StartDT { get; set; }
		public DateTime EndDT { get; set; }
		public int Score { get; set; }  // Score for the entire period

		public Period ()
		{
			var startNow = DateTime.Now;
			StartDT = startNow;
			EndDT = DateTime.MaxValue;
			Score = 0;
		}
	}
}

