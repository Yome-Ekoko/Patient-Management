using Microsoft.AspNetCore.Mvc;
using Patient_Management.Core.Contract;
using Patient_Management.Core.DTO.Request;
using Patient_Management.Domain.QueryParameters;

namespace Patient_Management.Controllers
{

   [Route("api/[controller]")]
[ApiController]
public class PatientController : ControllerBase
{
    private readonly IPatientService _patientService;

    public PatientController(IPatientService patientService)
    {
        _patientService = patientService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterPatientRequest request)
    {
        var patient = await _patientService.RegisterPatient(request);
        return Ok(patient);
    }

    [HttpPost("patients")]
    public async Task<IActionResult> GetPatients([FromBody] LogQueryParameters queryParameters, CancellationToken cancellationToken)
    {
        var patients = await _patientService.GetPatients(queryParameters, cancellationToken);
        return Ok(patients);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPatientById(string id)
    {
        var patient = await _patientService.GetPatientById(id);
        if (patient == null) return NotFound();
        return Ok(patient);
    }

    [HttpPost("update")]
    public async Task<IActionResult> UpdatePatient([FromBody] UpdatePatientRequest request)
    {
        var updatedPatient = await _patientService.UpdatePatient(request);
        if (updatedPatient == null) return NotFound();
        return Ok(updatedPatient);
    }
}

}
