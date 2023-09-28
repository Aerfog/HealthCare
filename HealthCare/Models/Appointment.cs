namespace HealthCare.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public string PatientId { get; set; }
        public string ProviderId { get; set; }
        public DateTime DateTime { get; set; }
        public string Description { get; set; }
        public Patient Patient { get; set; }
        public Provider Provider { get; set; }
        public ICollection<AppointmentMedication> Medications { get; set; }
    }
}