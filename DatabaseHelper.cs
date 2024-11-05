using Microsoft.Data.Sqlite;

namespace GettyImagesScraper
{
    public static class DatabaseHelper
    {
        private static readonly string SqliteConnectionString = "Data Source=ScrapedImages.db;";

        public static void InitialiseDatabase()
        {
            using (var connection = new SqliteConnection(SqliteConnectionString))
            {
                connection.Open();
                string createTableQuery = @"
                    CREATE TABLE IF NOT EXISTS Images (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Url TEXT NOT NULL,
                        Category TEXT NOT NULL
                    )";
                using (var cmd = new SqliteCommand(createTableQuery, connection))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static async Task SaveUrlsAsync(List<string> urls, string category)
        {
            using (var connection = new SqliteConnection(SqliteConnectionString))
            {
                connection.Open();

                foreach (var url in urls)
                {
                    string query = "INSERT INTO Images (Url, Category) VALUES (@url, @category)";
                    using (var cmd = new SqliteCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@url", url);
                        cmd.Parameters.AddWithValue("@category", category);
                        await cmd.ExecuteNonQueryAsync();
                    }
                }
            }
        }

        public static async Task<List<Image>> GetAllImagesAsync()
        {
            var images = new List<Image>();

            using (var connection = new SqliteConnection(SqliteConnectionString))
            {
                await connection.OpenAsync();
                string query = "SELECT Id, Url, Category FROM Images";

                using (var cmd = new SqliteCommand(query, connection))
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var id = reader.GetInt32(0);
                        var url = reader.GetString(1);
                        var category = reader.GetString(2);
                        images.Add(new Image(id, url, category));
                    }
                }
            }

            return images;
        }
    }
}