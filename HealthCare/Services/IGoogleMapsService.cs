using HealthCare.Models;

namespace HealthCare.Services;

public interface IGoogleMapsService
{
    Task<IEnumerable<Place>> FindNearbyPharmacies(string apiKey, Location location);
}