using System;
using SQLite;

namespace JobSearchScorecard
{
	public class Task
	{
		[PrimaryKey, AutoIncrement]
		public int ID { get; set; }  // unique row ID from Database, thus a DB/SQL concept only and not part of the app model
		public int Step { get; set; } // Step 0 = ALL, etc.
		public int SubStep { get; set; } // also uniquely identifies the activity, even across Steps
		public int OneTimeOnly { get; set; } // >0 means this is a once-in-a-lifetime task
		public DateTime DT { get; set; }  // records the Date AND Time task was accomplished (which falls into a unique Period)
		public string Notes { get; set; } // TBD not sure if I'll use this, but thinking of place for users to stash notes

		public Task ()
		{
		}

		public Task (int dbID, int step, int subStep, int oneTime, DateTime dtStamp, string notes)
		{
			ID = dbID;
			Step = step;
			SubStep = subStep;
			OneTimeOnly = oneTime;
			DT = dtStamp;
			Notes = notes;
		}
	}
}

