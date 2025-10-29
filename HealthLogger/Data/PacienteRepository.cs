using Dapper;
using HealthLogger.Models;
using System.Collections.Generic;
using System.Linq;

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

        public static List<Paciente> ObtenerPacientes(bool incluirOcultos = false)
        {
            using var conn = Database.GetConnection();
            if (incluirOcultos)
                return conn.Query<Paciente>("SELECT * FROM Pacientes ORDER BY CreatedAt").ToList();

            return conn.Query<Paciente>("SELECT * FROM Pacientes WHERE Estado=1 ORDER BY Apellidos").ToList();
        }
        public static void OcultarPaciente(int id)
        {
            using var conn = Database.GetConnection();
            string sql = "UPDATE Pacientes SET Estado=0, UpdatedAt=@UpdatedAt WHERE Id=@Id";
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
