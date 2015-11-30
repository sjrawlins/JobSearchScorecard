using System;
using SQLite;

namespace JobSearchScorecard
{
	public class Settings
	{
		[PrimaryKey, AutoIncrement]
		public int ID { get; set; } // unique row ID from Database, thus a DB/SQL concept only and not part of the app model
		public string EmailAddress;
		public string Name;
		public int GreenThreshold;
		public int StarIncrement;

		public Settings ()
		{
			Name = string.Empty;
			EmailAddress = string.Empty;
			GreenThreshold = 150;  // flip from red to green 
			StarIncrement = 50;  // gain a new star after each additional
		}
	}
}

