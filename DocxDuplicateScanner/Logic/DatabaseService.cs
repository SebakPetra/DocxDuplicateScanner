using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using DocxDuplicateScanner.Models;

namespace DocxDuplicateScanner.Logic
{
    public class DatabaseService
    {
        private static DatabaseService _instance;
        public static DatabaseService Instance => _instance ??= new DatabaseService();

        private string connectionString = $"Data Source={DatabaseSetup.DatabaseFile};Version=3;";

        private DatabaseService()
        {
            DatabaseSetup.InitializeDatabase();
        }

        /// <summary>
        /// Mentés vagy frissítés: ha már létezik UniqueHash, akkor a Files mezőt frissíti, 
        /// hozzáadva az új fájlokat. Ha nincs, új rekord jön létre.
        /// </summary>
        public void SaveOrUpdatePeople(List<Person> people)
        {
            using var conn = new SQLiteConnection(connectionString);
            conn.Open();

            foreach (var person in people)
            {
                // Ellenőrzés, hogy létezik-e már
                string checkSql = "SELECT Files FROM People WHERE UniqueHash=@hash";
                using var checkCmd = new SQLiteCommand(checkSql, conn);
                checkCmd.Parameters.AddWithValue("@hash", person.UniqueHash);
                var existingFiles = checkCmd.ExecuteScalar() as string;

                if (existingFiles != null)
                {
                    // Frissítés: hozzáadjuk az új fájlokat, duplikációk nélkül
                    var allFiles = existingFiles.Split(',').Select(f => f.Trim()).ToList();
                    allFiles.AddRange(person.Files);
                    allFiles = allFiles.Distinct().ToList();

                    string updateSql = "UPDATE People SET Files=@files WHERE UniqueHash=@hash";
                    using var updateCmd = new SQLiteCommand(updateSql, conn);
                    updateCmd.Parameters.AddWithValue("@files", string.Join(", ", allFiles));
                    updateCmd.Parameters.AddWithValue("@hash", person.UniqueHash);
                    updateCmd.ExecuteNonQuery();
                }
                else
                {
                    // Új rekord beszúrása
                    string insertSql = "INSERT INTO People (UniqueHash, Name, Phone, Address, Files) VALUES (@hash,@name,@phone,@address,@files)";
                    using var insertCmd = new SQLiteCommand(insertSql, conn);
                    insertCmd.Parameters.AddWithValue("@hash", person.UniqueHash);
                    insertCmd.Parameters.AddWithValue("@name", person.Name);
                    insertCmd.Parameters.AddWithValue("@phone", person.Phone);
                    insertCmd.Parameters.AddWithValue("@address", person.Address);
                    insertCmd.Parameters.AddWithValue("@files", string.Join(", ", person.Files));
                    insertCmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Duplikáció keresés az adatbázisban az újonnan behúzott emberekhez képest
        /// </summary>
        public List<Person> FindDuplicates(List<Person> newPeople)
        {
            var duplicates = new List<Person>();
            using var conn = new SQLiteConnection(connectionString);
            conn.Open();

            foreach (var person in newPeople)
            {
                string sql = "SELECT Name, Phone, Address, Files FROM People WHERE UniqueHash=@hash";
                using var cmd = new SQLiteCommand(sql, conn);
                cmd.Parameters.AddWithValue("@hash", person.UniqueHash);
                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    duplicates.Add(new Person
                    {
                        Name = reader["Name"].ToString(),
                        Phone = reader["Phone"].ToString(),
                        Address = reader["Address"].ToString(),
                        Files = reader["Files"].ToString().Split(',').Select(f => f.Trim()).ToList()
                    });
                }
            }

            return duplicates;
        }

        /// <summary>
        /// Összes rekord lekérése
        /// </summary>
        public List<Person> GetAllRecords()
        {
            var people = new List<Person>();
            using var conn = new SQLiteConnection(connectionString);
            conn.Open();

            string sql = "SELECT Name, Phone, Address, Files FROM People";
            using var cmd = new SQLiteCommand(sql, conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                people.Add(new Person
                {
                    Name = reader["Name"].ToString(),
                    Phone = reader["Phone"].ToString(),
                    Address = reader["Address"].ToString(),
                    Files = reader["Files"].ToString().Split(',').Select(f => f.Trim()).ToList()
                });
            }
            return people;
        }

        /// <summary>
        /// Összes rekord törlése
        /// </summary>
        public void DeleteAllRecords()
        {
            using var conn = new SQLiteConnection(connectionString);
            conn.Open();
            string sql = "DELETE FROM People";
            using var cmd = new SQLiteCommand(sql, conn);
            cmd.ExecuteNonQuery();
        }
    }
}
