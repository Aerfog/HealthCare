using HealthCare.Data;
using HealthCare.Models;
using Microsoft.EntityFrameworkCore;

namespace HealthCare.Services;

public class ProviderRepository
{
    private readonly ApplicationDbContext _dbContext;

    public ProviderRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Provider> GetProviderById(string id)
    {
        return await _dbContext.Providers.FindAsync(id);
    }

    public async Task<IEnumerable<Provider>> GetAllProviders()
    {
        return await _dbContext.Providers.ToListAsync();
    }

    public async Task AddProvider(Provider provider)
    {
        _dbContext.Providers.Add(provider);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateProvider(Provider provider)
    {
        _dbContext.Entry(provider).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteProvider(int id)
    {
        var provider = await _dbContext.Providers.FindAsync(id);
        if (provider != null)
        {
            _dbContext.Providers.Remove(provider);
            await _dbContext.SaveChangesAsync();
        }
    }
}