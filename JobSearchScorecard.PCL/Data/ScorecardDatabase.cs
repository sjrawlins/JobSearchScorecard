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
		public IEnumerable<Period> GetPeriods ()
		{
			lock (locker) {
				return (from i in database.Table<Period>() select i).ToList();
			}
		}
		public Period GetActivePeriod()
		{
			IEnumerable<Period> periodRows;

			lock (locker) {
				periodRows = database.Query<Period> ("select * from [Period] where [EndDt] > ?", DateTime.Now);
				if (periodRows == null) {
					throw new Exception ("In GetActivePeriod: Current period not found in database");
				}
				return periodRows.First ();
				// For some reason the following did not work for me (never got to the bottom of it & decided to
				// re-write the query as Query<Period> instead of Table
				//var query = database.Table<Period> ().Where (p => p.EndDT > DateTime.Now);
				//return query;
			}
		}
		public int UpdateCurrentScore(int score)
		{
			var currentPeriod = GetActivePeriod ();
			currentPeriod.Score = score;
			lock (locker) {
				return database.Update (currentPeriod);
			}
		}
		public IEnumerable<Task> GetTasks ()
		{
			lock (locker) {
				return (from i in database.Table<Task>() select i).ToList();
			}
		}

		public IEnumerable<Task> GetTasksBySubStep (int subStepNum)
		{
			if (subStepNum < 0 || subStepNum >= Activity.UniqueCode) {
				throw new IndexOutOfRangeException ("Impossible subStepNum=" + subStepNum);
			}
			lock (locker) {
				return database.Table<Task> ().Where (t => t.SubStep == subStepNum);
			}
		}

		public IEnumerable<Task> GetCurrentTasksBySubStep (int subStepNum)
		{
			if (subStepNum < 0 || subStepNum >= Activity.UniqueCode) {
				throw new IndexOutOfRangeException ("Impossible subStepNum=" + subStepNum);
			}
			return GetAllTasksWithinPeriod ().Where (t => t.SubStep == subStepNum);
		}
			
		public IEnumerable<Task> GetAllTasksWithinPeriod ()
		{
			lock (locker) {
				var currentPeriod = database.Query<Period> ("select * from [Period] where [EndDt] > ?", DateTime.Now);
				if (currentPeriod == null) {
					throw new Exception ("In GetTasksWithinPeriod: SELECT currentPeriod returned null List");
				}
				if (currentPeriod.Any ()) {
					// There should really only be one such row at this point, but a List needs a First...
					var periodStartDate = currentPeriod.First ().StartDT;
					return database.Query<Task> ("SELECT * FROM [Task] WHERE [DT] > ?", periodStartDate);
				} else {
					// the app does not function without a Current Period
					throw new Exception ("In GetTasksWithinPeriod: No Current Period");
				}
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
				//Debug.WriteLine ("About to delete task with ID=" + id);
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
				// but there MUST always be a current period, so create it in the otherwise empty DB
				var newPeriod = new Period ();
				database.Insert (newPeriod);
			}
		}
			
		public int SavePeriod()
		{
			int sqlReturn = 0;
			lock (locker) {
				// For Debug purposes, show how many rows in DB, both tables, before and after
				var countPeriod = database.Table<Period> ().Count();
				var countTasks = database.Table<Task> ().Count();
				Debug.WriteLine ("Before starting new period, COUNT[Period]=" + countPeriod + " COUNT[Task]=" + countTasks);

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
				countTasks = database.Table<Task> ().Count();
				Debug.WriteLine ("After adding new period, COUNT[Period]=" + countPeriod + " COUNT[Task]=" + countTasks);

				return sqlReturn;
			}
		}
		public IEnumerable<Period> ListPeriods()
		{
			lock (locker) {
				return database.Query<Period> ("SELECT * FROM [Period] ORDER By [StartDT] DESC");
			}
		}

	}
}