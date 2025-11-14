using System.Data.SQLite;
using System.IO;

namespace HealthLogger.Data
{
    public static class Database
    {
        // Carpeta en Documentos del usuario
        private static readonly string FolderPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            "HealthLogger"
        );

        // Ruta completa al archivo de base de datos
        private static readonly string dbPath = Path.Combine(FolderPath, "clinicos.db");

        public static SQLiteConnection GetConnection()
        {
            // Crear carpeta si no existe
            if (!Directory.Exists(FolderPath))
                Directory.CreateDirectory(FolderPath);

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
                CI TEXT,
                FechaNacimiento TEXT,
                Sexo TEXT,
                Direccion TEXT,
                Telefono TEXT,
                Profesion TEXT,
                EstadoCivil TEXT,
                ContactoEmergencia TEXT,
                TelefonoEmergencia TEXT,
                MotivoConsulta TEXT,
                EnfermedadActual TEXT,
                Antecedentes TEXT,
                ExamenNeurologico TEXT,
                ImpresionDiagnostica TEXT,
                Conducta TEXT,
                Evolucion TEXT,
                CreatedAt TEXT NOT NULL,
                UpdatedAt TEXT NOT NULL,
                Estado INTEGER NOT NULL DEFAULT 0
            );";

            using var cmd = new SQLiteCommand(sql, conn);
            cmd.ExecuteNonQuery();
        }
    }
}
