using System;
using System.Linq;
using System.Collections.Generic;

using Mono.Data.Sqlite;
using System.IO;
using System.Data;
using System.Threading.Tasks;

namespace JobSearchScorecard.Database
{
	/// <summary>
	/// Use ADO.NET to create the [Tasks] table and create,read,update,delete data
	/// </summary>
	public class ScorecardDatabase
	{
		static object locker = new object ();

		public SqliteConnection connection;
		public string path;

		protected static string dbLocation;

		public static string DatabaseFilePath {
			get { 
				var sqliteFilename = "JobSearchScorecardDatabase.db";
				#if NETFX_CORE
				var path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, sqliteFilename);
				#else

				#if SILVERLIGHT
				// Windows Phone expects a local path, not absolute
				var path = sqliteFilename;
				#else

				#if __ANDROID__
				// Just use whatever directory SpecialFolder.Personal returns
				string libraryPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal); ;
				#else
				// we need to put in /Library/ on iOS5.1 to meet Apple's iCloud terms
				// (they don't want non-user-generated data in Documents)
				string documentsPath = Environment.GetFolderPath (Environment.SpecialFolder.Personal); // Documents folder
				string libraryPath = Path.Combine (documentsPath, "..", "Library"); // Library folder
				#endif
				var path = Path.Combine (libraryPath, sqliteFilename);
				#endif

				#endif
				return path;	
			}
		}

		/// <summary>
		/// Initializes a new instance of the Scorecard Database. 
		/// if the database doesn't exist, it will create the database and all the tables.
		/// </summary>
		public ScorecardDatabase (string dbPath)
		{
			var output = "Initialize Scorecard Database, dbPath=" + dbPath;
			path = dbPath;  // store full file path in public field
			// create the tables
			bool exists = File.Exists (dbPath);

			if (!exists) {
				connection = new SqliteConnection ("Data Source=" + dbPath);

				connection.Open ();
				var commands = new[] {
					"CREATE TABLE [Tasks] (_id INTEGER PRIMARY KEY ASC, Code INTEGER, DateTimeDone DATETIME, " +
					"Notes NTEXT);",
					"CREATE TABLE [Periods] (_id INTEGER PRIMARY KEY ASC, Starting DATETIME, Ending DATETIME);",
					"INSERT INTO [Periods] ([Starting], [Ending]) VALUES (datetime(), date('9999-12-31'))",
				};
				foreach (var command in commands) {
					using (var c = connection.CreateCommand ()) {
						try {
							c.CommandText = command;
							c.ExecuteNonQuery ();
						} catch (Exception ex) {
							throw new Exception("Exception from top", ex);
						}
					}
				}
			} else {
				// already exists, do nothing. 
			}
			Console.WriteLine (output);
		}

		/// <summary>Convert from DataReader to Task object</summary>
		Task FromReader (SqliteDataReader r)
		{
			var t = new Task ();
			t.ID = Convert.ToInt32 (r ["_id"]);
			t.Code = Convert.ToInt32 (r ["Code"]);
			t.DT = Convert.ToDateTime (r ["DateTimeDone"]);
			t.Notes = r ["Notes"].ToString ();
			return t;
		}

		public DateTime GetStartDate()
		{
			var t = new DateTime ();
			lock (locker) {
				connection = new SqliteConnection ("Data Source=" + path);
				connection.Open ();
				using (var command = connection.CreateCommand ()) {
					command.CommandText = "SELECT datetime([Starting],'localtime') AS [Start] from [Periods] WHERE [Ending] > datetime()";
					var r = command.ExecuteReader ();
					while (r.Read ()) {
						t = Convert.ToDateTime (r ["Start"]);
						break;
					}
				}
				connection.Close ();
			}
			return t;
		}
		public IEnumerable<Task> GetItems ()
		{
			var tl = new List<Task> ();

			lock (locker) {
				connection = new SqliteConnection ("Data Source=" + path);
				connection.Open ();
				using (var contents = connection.CreateCommand ()) {
					contents.CommandText = "SELECT [_id], [Code], [DateTimeDone], [Notes] from [Tasks]";
					var r = contents.ExecuteReader ();
					while (r.Read ()) {
						tl.Add (FromReader (r));
					}
				}
				connection.Close ();
			}
			return tl;
		}

		public Task GetItem (int id)
		{
			var t = new Task ();
			lock (locker) {
				connection = new SqliteConnection ("Data Source=" + path);
				connection.Open ();
				using (var command = connection.CreateCommand ()) {
					command.CommandText = "SELECT [_id], [Code], [DateTimeDone], [Notes] from [Tasks] WHERE [_id] = ?";
					command.Parameters.Add (new SqliteParameter (DbType.Int32) { Value = id });
					var r = command.ExecuteReader ();
					while (r.Read ()) {
						t = FromReader (r);
						break;
					}
				}
				connection.Close ();
			}
			return t;
		}

		public int SaveItem (Task item)
		{
			int r;
			lock (locker) {
				if (item.ID != 0) {
					connection = new SqliteConnection ("Data Source=" + path);
					connection.Open ();
					using (var command = connection.CreateCommand ()) {
						command.CommandText = "UPDATE [Tasks] SET [Code] = ?, [DateTimeDone] = ?, " +
						"[Notes] = ? WHERE [_id] = ?;";
						command.Parameters.Add (new SqliteParameter (DbType.Int32) { Value = item.Code });
						command.Parameters.Add (new SqliteParameter (DbType.DateTime) { Value = item.DT });
						command.Parameters.Add (new SqliteParameter (DbType.String) { Value = item.Notes });
						command.Parameters.Add (new SqliteParameter (DbType.Int32) { Value = item.ID });
						r = command.ExecuteNonQuery ();
					}
					connection.Close ();
					return r;
				} else {
					connection = new SqliteConnection ("Data Source=" + path);
					connection.Open ();
					using (var command = connection.CreateCommand ()) {
						command.CommandText = "INSERT INTO [Tasks] ([Code], [DateTimeDone], [Notes]) VALUES (? ,?, ?)";
						command.Parameters.Add (new SqliteParameter (DbType.String) { Value = item.Code });
						command.Parameters.Add (new SqliteParameter (DbType.DateTime) { Value = item.DT });
						command.Parameters.Add (new SqliteParameter (DbType.String) { Value = item.Notes });
						r = command.ExecuteNonQuery ();
					}
					connection.Close ();
					return r;
				}

			}
		}

		public int DeleteItem (int id)
		{
			lock (locker) {
				int r;
				connection = new SqliteConnection ("Data Source=" + path);
				connection.Open ();
				using (var command = connection.CreateCommand ()) {
					command.CommandText = "DELETE FROM [Tasks] WHERE [_id] = ?;";
					command.Parameters.Add (new SqliteParameter (DbType.Int32) { Value = id });
					r = command.ExecuteNonQuery ();
				}
				connection.Close ();
				return r;
			}
		}
	}
}