using Dapper;
using HealthLogger.Models;

namespace HealthLogger.Data
{
    public static class PacienteRepository
    {
        public static void AgregarPaciente(Paciente paciente)
        {
            paciente.CreatedAt = DateTime.UtcNow;
            paciente.UpdatedAt = paciente.CreatedAt;
            paciente.Estado = 1;

            using var conn = Database.GetConnection();

            string sql = @"INSERT INTO Pacientes (Nombres, Apellidos, CI, FechaNacimiento, Sexo, Telefono,
                ContactoEmergencia, TelefonoEmergencia, Antecedentes, Alergias, Observaciones,
                CreatedAt, UpdatedAt, Estado)
                VALUES (@Nombres, @Apellidos, @CI, @FechaNacimiento, @Sexo, @Telefono,
                @ContactoEmergencia, @TelefonoEmergencia, @Antecedentes, @Alergias, @Observaciones,
                @CreatedAt, @UpdatedAt, @Estado)";
            conn.Execute(sql, paciente);
        }

        public static List<Paciente> ObtenerPacientes(bool incluirOcultos)
        {
            using var conn = Database.GetConnection();
            if (incluirOcultos)
                // Mostrar TODOS (visibles + ocultos)
                return conn.Query<Paciente>(
                    "SELECT Id, Nombres, Apellidos, CI, Sexo, Telefono, FechaNacimiento, Estado, CreatedAt, UpdatedAt FROM Pacientes ORDER BY UpdatedAt DESC"
                ).ToList();

            // Mostrar solo los activos
            return conn.Query<Paciente>(
                "SELECT Id, Nombres, Apellidos, CI, Sexo, Telefono, FechaNacimiento, Estado, CreatedAt, UpdatedAt FROM Pacientes WHERE Estado = 1 ORDER BY UpdatedAt DESC"
            ).ToList();
        }

        public static void OcultarPaciente(int id)
        {
            using var conn = Database.GetConnection();
            string sql = "UPDATE Pacientes SET Estado=0, UpdatedAt=@UpdatedAt WHERE Id=@Id";
            conn.Execute(sql, new { UpdatedAt = DateTime.UtcNow, Id = id });
        }
        public static void DesocultarPaciente(int id)
        {
            using var conn = Database.GetConnection();
            string sql = "UPDATE Pacientes SET Estado=1, UpdatedAt=@UpdatedAt WHERE Id=@Id";
            conn.Execute(sql, new { UpdatedAt = DateTime.UtcNow, Id = id });
        }
        public static Paciente ObtenerPacientePorId(int id)
        {
            using var conn = Database.GetConnection();

            string sql = @"SELECT * FROM Pacientes WHERE Id = @Id LIMIT 1";
            return conn.QueryFirstOrDefault<Paciente>(sql, new { Id = id });
        }
        public static void ActualizarPaciente(Paciente paciente)
        {
            using var conn = Database.GetConnection();

            paciente.UpdatedAt = DateTime.UtcNow;

            string sql = @"
        UPDATE Pacientes SET
            Nombres = @Nombres,
            Apellidos = @Apellidos,
            CI = @CI,
            FechaNacimiento = @FechaNacimiento,
            Sexo = @Sexo,
            Telefono = @Telefono,
            ContactoEmergencia = @ContactoEmergencia,
            TelefonoEmergencia = @TelefonoEmergencia,
            Antecedentes = @Antecedentes,
            Alergias = @Alergias,
            Observaciones = @Observaciones,
            UpdatedAt = @UpdatedAt
        WHERE Id = @Id";

            conn.Execute(sql, new
            {
                paciente.Nombres,
                paciente.Apellidos,
                paciente.CI,
                paciente.FechaNacimiento,
                paciente.Sexo,
                paciente.Telefono,
                paciente.ContactoEmergencia,
                paciente.TelefonoEmergencia,
                paciente.Antecedentes,
                paciente.Alergias,
                paciente.Observaciones,
                paciente.UpdatedAt,
                paciente.Id
            });
        }

    }
}