using System;
using System.Diagnostics;
using SQLite;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;


namespace JobSearchScorecard
{
	public class ScorecardDatabase 
	{
		static object locker = new object ();

		SQLiteConnection database;

		public ScorecardDatabase()
		{
			database = DependencyService.Get<ISQLite> ().GetConnection ();
			// create the tables
			database.CreateTable<Task>();
			database.CreateTable<Period> ();
			if (database.Table<Period> ().Count() == 0) {
				// only insert the data if it doesn't already exist
				var newPeriod = new Period ();
				database.Insert (newPeriod);
			}
		}

		public Period GetActivePeriod()
		{
			IEnumerable<Period> periodRows;

			lock (locker) {
				periodRows = database.Query<Period> ("select * from [Period] where [EndDt] > ?", DateTime.Now);
				if (periodRows == null) {
					throw new Exception ("Current period not found in database");
				}
				return periodRows.First ();
				// For some reason the following did not work for me (never got to the bottom of it & decided to
				// re-write the query as Query<Period> instead of Table
				//var query = database.Table<Period> ().Where (p => p.EndDT > DateTime.Now);
				//return query;
			}
		}
		public IEnumerable<Task> GetTasks ()
		{
			lock (locker) {
				return (from i in database.Table<Task>() select i).ToList();
			}
		}

		public IEnumerable<Task> GetTasksWithinPeriod ()
		{
			lock (locker) {
				return database.Query<Task> ("SELECT * FROM [Task]");  //  WHERE [DT]");  
				// need sub-query for active period to know   WHERE [DT] BETWEEN start_date and max_date
			}
		}

		public Task GetTask (int id) 
		{
			lock (locker) {
				return database.Table<Task>().FirstOrDefault(x => x.ID == id);
			}
		}

		public int SaveTask (Task item) 
		{
			lock (locker) {
				if (item.ID != 0) {
					database.Update(item);
					return item.ID;
				} else {
					return database.Insert(item);
				}
			}
		}

		public int DeleteTask(int id)
		{
			lock (locker) {
				return database.Delete<Task>(id);
			}
		}

		public int DeleteCurrentPeriodTasks()
		{
			// delete all rows in Task table EXCEPT for once-in-a-lifetime
			lock (locker) {
				return database.Execute ("DELETE FROM [Task] WHERE [OneTimeOnly] = 0");
			}
		}

		public void DeleteAll()
		{
			lock (locker) {
				database.DeleteAll<Task> ();
				database.DeleteAll<Period> ();
			}
		}
			
		public int SavePeriod()
		{
			int sqlReturn = 0;
			lock (locker) {
				// For Debug purposes, show how many rows in [Period] now
				var countPeriod = database.Table<Period> ().Count();
				Debug.WriteLine ("Before adding new cycle, [Period] has " + countPeriod + " rows");

				var setNow = DateTime.Now;
				sqlReturn = database.Execute (
					                "update [Period] set [EndDt] = ? where [EndDt] > ? ", setNow, setNow);
				if (sqlReturn > 1) {
					throw new Exception("Failed to properly UPDATE row to end current [Period]. RC=" + sqlReturn);
				}
				var newPeriod = new Period ();
				sqlReturn = database.Insert (newPeriod);
				if (sqlReturn != 1) {
					throw new Exception("Failed to properly INSERT new row in [Period]. RC=" + sqlReturn);
				}
				countPeriod = database.Table<Period> ().Count();
				Debug.WriteLine ("[Period] now has " + countPeriod + " rows");
				return sqlReturn;
			}
		}
	}
}