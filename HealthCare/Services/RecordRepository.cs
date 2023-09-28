using HealthCare.Data;
using HealthCare.Models;
using Microsoft.EntityFrameworkCore;

namespace HealthCare.Services;

public class RecordRepository
{
    private readonly ApplicationDbContext _dbContext;

    public RecordRepository(ApplicationDbContext dbContext) 
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Record>> GetAllRecords()
    {
        return await _dbContext.Records.Include(p => p.Patient)
            .ToListAsync();
    }
    
    public async Task<IEnumerable<Record>> GetRecordsByPatient(string id)
    {
        return await _dbContext.Records.Where(t => t.PatientId.Equals(id, StringComparison.CurrentCultureIgnoreCase))
            .Include(p => p.Patient)
            .ToListAsync();
    }

    public async Task<Record> GetRecordById(int id)
    {
        return await _dbContext.Records.FindAsync(id);
    }

    public async Task<int> AddRecord(Record record)
    {
        _dbContext.Records.Add(record);
        await _dbContext.SaveChangesAsync();
        return record.Id;
    }

    public async Task UpdateRecord(Record record)
    {
        _dbContext.Entry(record).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteRecord(int id)
    {
        var record = await _dbContext.Records.FindAsync(id);
        if(record != null)
        {
            _dbContext.Records.Remove(record);
            await _dbContext.SaveChangesAsync();
        }
    }
}