using HealthCare.Data;
using HealthCare.Models;
using Microsoft.EntityFrameworkCore;

namespace HealthCare.Services;

public class AppointmentRepository
{
    private readonly ApplicationDbContext _dbContext;

    public AppointmentRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Appointment> GetAppointmentById(int id)
    {
        return await _dbContext.Appointments
            .Include(a => a.Patient)
            .Include(a => a.Provider)
            .Include(a => a.Medications.Select(x => x.Medication).ToList())
            .FirstAsync(a => a.Id == id);
    }

    public async Task<IEnumerable<Appointment>> GetAllAppointments()
    {
        return await _dbContext.Appointments
            .Include(a => a.Patient)
            .Include(a => a.Provider)
            .Include(a => a.Medications.Select(x => x.Medication).ToList())
            .ToListAsync();
    }

    public async Task AddAppointment(Appointment appointment)
    {
        _dbContext.Appointments.Add(appointment);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAppointment(Appointment appointment)
    {
        _dbContext.Entry(appointment).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAppointment(int id)
    {
        var appointment = await _dbContext.Appointments.FindAsync(id);
        if (appointment != null)
        {
            _dbContext.Appointments.Remove(appointment);
            await _dbContext.SaveChangesAsync();
        }
    }
}