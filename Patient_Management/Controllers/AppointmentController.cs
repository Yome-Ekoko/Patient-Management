using Microsoft.AspNetCore.Mvc;
using Patient_Management.Core.Contract;
using Patient_Management.Core.DTO.Request;
using Patient_Management.Domain.QueryParameters;
using System;

namespace Patient_Management.Controllers
{
  

        [Route("api/[controller]")]
        [ApiController]
        public class AppointmentsController : ControllerBase
        {
            private readonly IAppointmentService _appointmentService;

            public AppointmentsController(IAppointmentService appointmentService)
            {
                _appointmentService = appointmentService;
            }

            [HttpPost("book")]
            public async Task<IActionResult> BookAppointment([FromBody] AppointmentRequest request)
            {
                var appointment = await _appointmentService.BookAppointmentAsync(request);
                return CreatedAtAction(nameof(GetAppointment), new { id = appointment.Id }, appointment);
            }

            [HttpGet("{id}")]
            public async Task<IActionResult> GetAppointment(string id)
            {
                var appointment = await _appointmentService.GetAppointmentAsync(id);
                if (appointment == null) return NotFound();

                return Ok(appointment);
            }

            [HttpGet("patient/{patientId}")]
            public async Task<IActionResult> GetAppointmentsForPatient([FromQuery] LogQueryParameters queryParameters, string patientId)
            {
                var appointments = await _appointmentService.GetAppointmentsForPatientAsync(queryParameters,patientId);
                return Ok(appointments);
            }

            [HttpPut("{id}")]
            public async Task<IActionResult> UpdateAppointment(string id, [FromBody] AppointmentRequest request)
            {
                var updated = await _appointmentService.UpdateAppointmentAsync(id, request);
                return Ok(updated);
            }

            [HttpPut("{id}/complete")]
            public async Task<IActionResult> CompleteAppointment(string id)
            {
                var success = await _appointmentService.CompleteAppointmentAsync(id);
                return success ? Ok(new { message = "Appointment completed" }) : NotFound();
            }

            [HttpPut("{id}/cancel")]
            public async Task<IActionResult> CancelAppointment(string id)
            {
                var success = await _appointmentService.CancelAppointmentAsync(id);
                return success ? Ok(new { message = "Appointment cancelled" }) : NotFound();
            }
        }
    }
    
