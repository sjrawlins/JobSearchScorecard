using System;
using SQLite;

namespace JobSearchScorecard
{
	public class Settings
	{
		[PrimaryKey, AutoIncrement]
		public int ID { get; set; } // unique row ID from Database, thus a DB/SQL concept only and not part of the app model
		public string Name { get; set; } 
		public int GreenThreshold { get; set; } 
		public int StarIncrement { get; set; }

		public Settings()
		{
			Name = string.Empty;  // signals Alert to enter name first time in
			GreenThreshold = 100;
			StarIncrement = 50;
		}
			
	}
}

