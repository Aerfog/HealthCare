using HealthCare.Models;
using HealthCare.Services;
using Microsoft.Identity.Web;

namespace HealthCare.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

[Authorize(Roles = "Provider")]
[ApiController]
[Route("api/[controller]")]
public class ProviderController : ControllerBase
{
    private readonly AppointmentRepository _appointmentRepository;
    private readonly RecordRepository _recordRepository;

    public ProviderController(AppointmentRepository appointmentRepository, RecordRepository recordRepository)
    {
        _appointmentRepository = appointmentRepository;
        _recordRepository = recordRepository;
    }

    [HttpGet("appointments")]
    public async Task<ActionResult<IEnumerable<Appointment>>> GetAppointments()
    {
        var appointments = await _appointmentRepository.GetAllAppointments();
        return Ok(appointments);
    }
    
    [HttpGet("records")]
    public async Task<ActionResult<IEnumerable<Appointment>>> GetRecords()
    {
        var records = await _recordRepository.GetAllRecords();
        return Ok(records);
    }
    
    [HttpGet("records/{id}")]
    public async Task<ActionResult<IEnumerable<Appointment>>> GetRecordById(int id)
    {
        var record = await _recordRepository.GetRecordById(id);
        return Ok(record);
    }

    [HttpPost("appointments")]
    public async Task<ActionResult<Appointment>> CreateAppointment(Appointment appointment)
    {
        await _appointmentRepository.AddAppointment(appointment);
        return CreatedAtAction(nameof(GetAppointmentById), new { id = appointment.Id }, appointment);
    }

    [HttpGet("appointments/{id}")]
    public async Task<ActionResult<Appointment>> GetAppointmentById(int id)
    {
        var appointment = await _appointmentRepository.GetAppointmentById(id);
        
        if (appointment.ProviderId != GetCurrentProviderId())
        {
            return Unauthorized();
        }

        return Ok(appointment);
    }

    [HttpPut("appointments/{id}")]
    public async Task<IActionResult> UpdateAppointment(int id, Appointment appointment)
    {
        if (id != appointment.Id)
        {
            return BadRequest();
        }
        
        if (appointment.ProviderId != GetCurrentProviderId())
        {
            return Unauthorized();
        }

        await _appointmentRepository.UpdateAppointment(appointment);
        return NoContent();
    }

    [HttpDelete("appointments/{id}")]
    public async Task<IActionResult> DeleteAppointment(int id)
    {
        var appointment = await _appointmentRepository.GetAppointmentById(id);

        // Check if the appointment belongs to the current provider
        if (appointment.ProviderId != GetCurrentProviderId())
        {
            return Unauthorized();
        }

        await _appointmentRepository.DeleteAppointment(id);
        return NoContent();
    }

    [HttpPost("records/{id}/")]
    public async Task<ActionResult<Appointment>> AnswerRecord(int recordId)
    {
        var record = await _recordRepository.GetRecordById(recordId);
        var provider = GetCurrentProviderId();
        var appointment = new Appointment()
        {
            Id = record.Id,
            Medications = new List<AppointmentMedication>(),
            ProviderId = provider,
            Patient = record.Patient,
            PatientId = record.PatientId,
            Description = record.Description,
            DateTime = DateTime.Now,
            Provider = await GetCurrentProvider(null ,provider)
        };

        await _recordRepository.DeleteRecord(recordId);
        return await CreateAppointment(appointment);
    }
    
    private string GetCurrentProviderId()
    {
        var result = User.GetObjectId();
        return result; // Replace with the actual implementation.
    }
    
    private async Task<Provider> GetCurrentProvider([FromServices]ProviderRepository providerRepository, string id)
    {
        return await providerRepository.GetProviderById(id);
    }
}