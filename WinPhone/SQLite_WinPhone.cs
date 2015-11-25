using JobSearchScorecard;
using System.IO;
using Windows.Storage;
using Xamarin.Forms;

[assembly: Dependency(typeof(SQLite_WinPhone))]

namespace JobSearchScorecard
{

public class SQLite_WinPhone : ISQLite
    {
        public SQLite_WinPhone() { }
        public SQLite.SQLiteConnection GetConnection()
        {
            var sqliteFilename = "JobSearchScorecard.db3";
            string path = Path.Combine(ApplicationData.Current.LocalFolder.Path, sqliteFilename);
            // Create the connection
            var conn = new SQLite.SQLiteConnection(path);
            // Return the database connection
            return conn;
        }
    }
}
