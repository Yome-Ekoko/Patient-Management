using Microsoft.AspNetCore.Mvc;
using Patient_Management.Core.Contract;
using Patient_Management.Core.DTO.Request;
using Patient_Management.Domain.QueryParameters;

namespace Patient_Management.Controllers
{ 
[Route("api/[controller]")]
[ApiController]
public class PatientRecordController : ControllerBase
{
    private readonly IPatientRecordService _recordService;

    public PatientRecordController(IPatientRecordService recordService)
    {
        _recordService = recordService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RecordRequest request)
    {
        var record = await _recordService.CreateRecordAsync(request);
        return Ok(record);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPatient(string id)
    {
        var record = await _recordService.GetRecordByIdAsync(id);
        if (record == null) return NotFound();
        return Ok(record);
    }

    [HttpGet("patient/{patientId}")]
    public async Task<IActionResult> GetPatientById(string patientId, [FromQuery] LogQueryParameters queryParameters)
    {
        var record = await _recordService.GetRecordForPatientAsync(queryParameters, patientId);
        return Ok(record);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePatient(string id, [FromBody] RecordRequest request)
    {
        var record = await _recordService.UpdateRecordAsync(id, request);
        if (record == null) return NotFound();
        return Ok(record);
    }
}

}

