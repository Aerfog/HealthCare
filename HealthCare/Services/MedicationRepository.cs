using HealthCare.Data;
using HealthCare.Models;
using Microsoft.EntityFrameworkCore;

namespace HealthCare.Services;

public class MedicationRepository
{
    private readonly ApplicationDbContext _dbContext;

    public MedicationRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Medication> GetMedicationById(int id)
    {
        return await _dbContext.Medications.FindAsync(id);
    }

    public async Task<IEnumerable<Medication>> GetAllMedications()
    {
        return await _dbContext.Medications.ToListAsync();
    }

    public async Task AddMedication(Medication medication)
    {
        _dbContext.Medications.Add(medication);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateMedication(Medication medication)
    {
        _dbContext.Entry(medication).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteMedication(int id)
    {
        var medication = await _dbContext.Medications.FindAsync(id);
        if (medication != null)
        {
            _dbContext.Medications.Remove(medication);
            await _dbContext.SaveChangesAsync();
        }
    }
}