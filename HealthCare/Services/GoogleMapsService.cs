using HealthCare.Models;
using Newtonsoft.Json;

namespace HealthCare.Services;

public class GoogleMapsService : IGoogleMapsService
{
    public async Task<IEnumerable<Place>> FindNearbyPharmacies(string apiKey, Location location)
    {
        string apiUrl = $"https://maps.googleapis.com/maps/api/place/nearbysearch/json?location={location.Latitude},{location.Longitude}&radius=5000&type=pharmacy&key={apiKey}";

        using (var client = new HttpClient())
        {
            var response = await client.GetAsync(apiUrl);
            if(response.IsSuccessStatusCode)
            {
                var jsonResult = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeAnonymousType(jsonResult, new { results = new List<Place>() });

                return result.results;
            }
            else
            {
                // Handle the error response appropriately
                throw new Exception("Failed to retrieve nearby pharmacies from the Google Maps API.");
            }
        }
    }
}