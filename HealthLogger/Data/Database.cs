using System.Data.SQLite;
using System.IO;

namespace HealthLogger.Data
{
    public static class Database
    {
        private static readonly string dbPath = Path.Combine(
            Directory.GetCurrentDirectory(), "Data", "clinicos.db");

        public static SQLiteConnection GetConnection()
        {
            string directory = Path.GetDirectoryName(dbPath)!;
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            bool crearTabla = !File.Exists(dbPath);
            if (crearTabla)
                SQLiteConnection.CreateFile(dbPath);

            var conn = new SQLiteConnection($"Data Source={dbPath};Version=3;");
            conn.Open();

            if (crearTabla)
                CrearTabla(conn);

            return conn;
        }

        private static void CrearTabla(SQLiteConnection conn)
        {
            string sql = @"
            CREATE TABLE IF NOT EXISTS Pacientes (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Nombres TEXT NOT NULL,
                Apellidos TEXT NOT NULL,
                CI TEXT UNIQUE,
                FechaNacimiento TEXT,
                Sexo TEXT,
                Telefono TEXT,
                ContactoEmergencia TEXT,
                TelefonoEmergencia TEXT,
                Antecedentes TEXT,
                Alergias TEXT,
                Observaciones TEXT,
                CreatedAt TEXT NOT NULL,
                UpdatedAt TEXT NOT NULL,
                Estado INTEGER NOT NULL DEFAULT 1
            );";

            using var cmd = new SQLiteCommand(sql, conn);
            cmd.ExecuteNonQuery();
        }
    }
}