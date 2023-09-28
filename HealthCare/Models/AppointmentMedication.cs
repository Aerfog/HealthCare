namespace HealthCare.Models;

public class AppointmentMedication
{
    public int AppointmentId { get; set; }
    public Appointment Appointment { get; set; }

    public int MedicationId { get; set; }
    public Medication Medication { get; set; }
}