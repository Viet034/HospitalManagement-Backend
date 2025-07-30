using Microsoft.AspNetCore.Mvc;
using SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.AppointmentVer2;
using SWP391_SE1914_ManageHospital.Models.Entities;
using SWP391_SE1914_ManageHospital.Service;
using System.Net;

namespace SWP391_SE1914_ManageHospital.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApointmentControllerVer2 : ControllerBase
    {
        private readonly IAppoinrmentServicever2 _service;

        public ApointmentControllerVer2(IAppoinrmentServicever2 service)
        {
            _service = service;
        }

        [HttpPost("cretate-appointmentv2")]
        [ProducesResponseType(typeof(IEnumerable<Appointment>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IEnumerable<Appointment>), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateAppointment([FromBody] AppointmentCreate create)
        {
            try
            {
                var response = await _service.CreateAppoiment(create);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
    }
}
