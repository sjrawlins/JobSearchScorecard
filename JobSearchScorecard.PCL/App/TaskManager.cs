using System;
using System.Collections.Generic;

namespace JobSearchScorecard
{
	/// <summary>
	/// Manager classes are an abstraction on the data access layers
	/// </summary>
	public static class TaskManager
	{
		static TaskManager ()
		{

		}

		public static DateTime GetStartDate()
		{
			return DatabaseADO.GetStartDate ();
		}
		public static Task GetTask (int id)
		{
			return DatabaseADO.GetTask (id);
		}

		public static IList<Task> GetTasks ()
		{
			return new List<Task> (DatabaseADO.GetTasks ());
		}

		public static int SaveTask (Task item)
		{
			return DatabaseADO.SaveTask (item);
		}

		public static int DeleteTask (int id)
		{
			return DatabaseADO.DeleteTask (id);
		}
	}
}