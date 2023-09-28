using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using HealthCare.Models;
using HealthCare.Services;
using Microsoft.Identity.Web;
using Newtonsoft.Json;

[Authorize(Roles = "Patient")]
[ApiController]
[Route("api/[controller]")]
public class PatientController : ControllerBase
{
    private readonly AppointmentRepository _appointmentRepository;
    private readonly RecordRepository _recordRepository;
    private readonly IGoogleMapsService _googleMapsService;
    private readonly IConfiguration _configuration;

    public PatientController(AppointmentRepository appointmentRepository, RecordRepository recordRepository, IGoogleMapsService googleMapsService, IConfiguration configuration)
    {
        _appointmentRepository = appointmentRepository;
        _recordRepository = recordRepository;
        _googleMapsService = googleMapsService;
        _configuration = configuration;
    }

    [HttpGet("appointments/{id}")]
    public async Task<ActionResult<Appointment>> GetAppointmentById(int id)
    {
        var appointment = await _appointmentRepository.GetAppointmentById(id);
        if (appointment == null)
        {
            return NotFound();
        }

        // Check if the appointment belongs to the current patient
        if (appointment.PatientId != GetCurrentPatientId())
        {
            return Unauthorized();
        }

        return Ok(appointment);
    }

    [HttpGet("pharmacies")]
    public async Task<ActionResult<IEnumerable<Place>>> FindNearbyPharmacies()
    {
        string apiKey = _configuration["GoogleMaps:ApiKey"];

        // Get the current patient's location
        var location = await GetCurrentPatientLocation();

        // Use the Google Maps API service to find nearby pharmacies
        var pharmacies = await _googleMapsService.FindNearbyPharmacies(apiKey, location);

        return Ok(pharmacies);
    }

    [HttpPost("records")]
    public async Task<ActionResult<Record>> CreateRecord(Record record)
    {
        record.PatientId = GetCurrentPatientId();
        await _recordRepository.AddRecord(record);
        return CreatedAtAction(nameof(GetRecordById), new { id = record.Id }, record);
    }

    [HttpGet("records/{id}")]
    public async Task<ActionResult<Record>> GetRecordById(int id)
    {
        var record = await _recordRepository.GetRecordById(id);
        if (record == null)
        {
            return NotFound();
        }

        // Check if the record belongs to the current patient
        if (record.PatientId != GetCurrentPatientId())
        {
            return Unauthorized();
        }

        return Ok(record);
    }

    private string GetCurrentPatientId()
    {
        var user = User.GetObjectId();
        return user;
    }
    
    private async Task<Location> GetCurrentPatientLocation()
    {
        string apiKey = _configuration["GoogleMaps:ApiKey"];
        string address = ""; // Replace with the logic to retrieve the patient's address from your data store or input

        // Use the Google Maps Geocoding API to obtain the location coordinates for the given address
        string apiUrl = $"https://maps.googleapis.com/maps/api/geocode/json?address={Uri.EscapeDataString(address)}&key={apiKey}";

        using (var client = new HttpClient())
        {
            var response = await client.GetAsync(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                var jsonResult = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<GeocodingResponse>(jsonResult);

                // Check if the API returned any results
                if (result.Status == "OK" && result.Results.Any())
                {
                    var location = result.Results.First().Geometry.Location;

                    // Create a new Location object using the retrieved latitude and longitude
                    var currentLocation = new Location
                    {
                        Latitude = location.Latitude,
                        Longitude = location.Longitude
                    };

                    return currentLocation;
                }
            }

            // Handle error cases, such as when the API response was not successful or no results were found
            throw new Exception("Failed to retrieve the current patient's location from the Google Maps API.");
        }
    }
    
    public class GeocodingResponse
    {
        public string Status { get; set; }
        public IEnumerable<GeocodingResult> Results { get; set; }
    }

    public class GeocodingResult
    {
        public Geometry Geometry { get; set; }
    }

    public class Geometry
    {
        public Location Location { get; set; }
    }
}