namespace HealthCare.Models;

public class Record
{
    public int Id { get; set; }
    public DateTime CreationDate { get; set; }
    public string PatientId { get; set; }
    public Patient Patient { get; set; }
    public string Description { get; set; }
}