using System;

namespace JobSearchScorecard
{
	public class Task
	{
		public int ID;  // unique row ID from Database, thus a DB/SQL concept only and not part of the app model
		public int Code; // Activity Code, used in Dictionary look-up
		public DateTime DT;  // records the Date AND TIME task was accomplished
		public string Notes;

		public Task ()
		{
		}

		public Task (int dbID, int activityCode, DateTime dtStamp, string notes)
		{
			ID = dbID;
			Code = activityCode;
			DT = dtStamp;
			Notes = notes;
		}
	}
}

