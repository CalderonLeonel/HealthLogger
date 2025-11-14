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

            string sql = @"INSERT INTO Pacientes (Nombres, Apellidos, CI, FechaNacimiento, Sexo, Direccion, Telefono,
                Profesion, EstadoCivil, ContactoEmergencia, TelefonoEmergencia, MotivoConsulta, EnfermedadActual,
                Antecedentes, ExamenNeurologico, ImpresionDiagnostica, Conducta, Evolucion,
                CreatedAt, UpdatedAt, Estado)
                VALUES (@Nombres, @Apellidos, @CI, @FechaNacimiento, @Sexo, @Direccion, @Telefono,
                @Profesion, @EstadoCivil, @ContactoEmergencia, @TelefonoEmergencia, @MotivoConsulta, @EnfermedadActual,
                @Antecedentes, @ExamenNeurologico, @ImpresionDiagnostica, @Conducta, @Evolucion,
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
            Direccion = @Direccion,
            Telefono = @Telefono,
            Profesion = @Profesion,
            EstadoCivil = @EstadoCivil,
            ContactoEmergencia = @ContactoEmergencia,
            TelefonoEmergencia = @TelefonoEmergencia,
            MotivoConsulta = @MotivoConsulta,
            EnfermedadActual = @EnfermedadActual,
            Antecedentes = @Antecedentes,
            ExamenNeurologico = @ExamenNeurologico,
            ImpresionDiagnostica = @ImpresionDiagnostica,
            Conducta = @Conducta,
            Evolucion = @Evolucion,
            UpdatedAt = @UpdatedAt
        WHERE Id = @Id";

            conn.Execute(sql, new
            {
                paciente.Nombres,
                paciente.Apellidos,
                paciente.CI,
                paciente.FechaNacimiento,
                paciente.Direccion,
                paciente.Sexo,
                paciente.Telefono,
                paciente.Profesion,
                paciente.EstadoCivil,
                paciente.ContactoEmergencia,
                paciente.TelefonoEmergencia,
                paciente.MotivoConsulta,
                paciente.EnfermedadActual,
                paciente.Antecedentes,
                paciente.ExamenNeurologico,
                paciente.ImpresionDiagnostica,
                paciente.Conducta,
                paciente.Evolucion,
                paciente.UpdatedAt,
                paciente.Id
            });
        }

    }
}