using System.ComponentModel;

namespace HealthLogger.Models
{
    public class Paciente : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private DateTime _fechaNacimiento;
        public int Id { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string CI { get; set; }
        public DateTime FechaNacimiento
        {
            get => _fechaNacimiento;
            set
            {
                if (_fechaNacimiento != value)
                {
                    _fechaNacimiento = value;
                    OnPropertyChanged(nameof(FechaNacimiento));
                    OnPropertyChanged(nameof(Edad)); // recalcula Edad cuando cambia FechaNacimiento
                }
            }
        }
        public string Sexo { get; set; }
        public string Telefono { get; set; }
        public string ContactoEmergencia { get; set; }
        public string TelefonoEmergencia { get; set; }
        public string Antecedentes { get; set; }
        public string Alergias { get; set; }
        public string Observaciones { get; set; }

        public int Edad => (int)((DateTime.Now - FechaNacimiento).TotalDays / 365.25);
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        private int estado;
        public int Estado
        {
            get => estado;
            set
            {
                if (estado != value)
                {
                    estado = value;
                    OnPropertyChanged(nameof(Estado));
                }
            }
        }
        protected void OnPropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
