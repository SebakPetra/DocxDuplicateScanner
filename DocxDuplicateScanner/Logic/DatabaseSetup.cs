using System.Data.SQLite;
using System.IO;

namespace DocxDuplicateScanner.Logic
{
    public static class DatabaseSetup
    {
        public const string DatabaseFile = "people.db";

        public static void InitializeDatabase()
        {
            if (!File.Exists(DatabaseFile))
            {
                SQLiteConnection.CreateFile(DatabaseFile);
            }

            using var conn = new SQLiteConnection($"Data Source={DatabaseFile};Version=3;");
            conn.Open();

            string sql = @"
                CREATE TABLE IF NOT EXISTS People (
                    UniqueHash TEXT PRIMARY KEY,
                    Name TEXT,
                    Phone TEXT,
                    Address TEXT,
                    Files TEXT
                );";
            using var cmd = new SQLiteCommand(sql, conn);
            cmd.ExecuteNonQuery();
        }
    }
}
