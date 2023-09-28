using HealthCare.Data;
using HealthCare.Models;
using Microsoft.EntityFrameworkCore;

namespace HealthCare.Services;

public class PatientRepository
{
    private readonly ApplicationDbContext _dbContext;

    public PatientRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Patient> GetPatientById(int id)
    {
        return await _dbContext.Patients.FindAsync(id);
    }

    public async Task<IEnumerable<Patient>> GetAllPatients()
    {
        return await _dbContext.Patients.ToListAsync();
    }

    public async Task AddPatient(Patient patient)
    {
        _dbContext.Patients.Add(patient);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdatePatient(Patient patient)
    {
        _dbContext.Entry(patient).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeletePatient(int id)
    {
        var patient = await _dbContext.Patients.FindAsync(id);
        if (patient != null)
        {
            _dbContext.Patients.Remove(patient);
            await _dbContext.SaveChangesAsync();
        }
    }
}