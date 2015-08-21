using System;
using SQLite;

namespace JobSearchScorecard
{
	public interface ISQLite
	{
		SQLiteConnection GetConnection ();
	}
}