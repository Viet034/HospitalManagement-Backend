using Microsoft.AspNetCore.Mvc;
using SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO;
using SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;
using SWP391_SE1914_ManageHospital.Services.Impl;
using SWP391_SE1914_ManageHospital.Services;


using System.Threading.Tasks;
using SWP391_SE1914_ManageHospital.Models.DTO.EntitiesDTO;

namespace SWP391_SE1914_ManageHospital.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorService _doctorService;
        private int id;

        public DoctorController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        [HttpGet("{Get_All}")]
        public async Task<ActionResult<IEnumerable<DoctorResponseDTO>>> GetDoctors()
        {
            var doctors = await _doctorService.GetAllDoctorsAsync();
            return Ok(doctors);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DoctorResponseDTO>> GetDoctor(int id)
        {
            var doctor = await _doctorService.GetDoctorByIdAsync(id);
            if (doctor == null)
            {
                return NotFound();
            }
            return Ok(doctor);
        }

        [HttpPost]
        public async Task<ActionResult<DoctorResponseDTO>> CreateDoctor(DoctorCreate doctorDTO)
        {
            var createdDoctor = await _doctorService.CreateDoctorAsync(doctorDTO);
            return CreatedAtAction(nameof(GetDoctor), new { id = createdDoctor.Id }, createdDoctor);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDoctor(int id, DoctorUpdate doctorDTO)
        {
            doctorDTO.Id = id;
            var result = await _doctorService.UpdateDoctorAsync(id, doctorDTO);
            if (!result)
            {
                return BadRequest();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDoctor(DoctorDelete doctorDTO)
        {
            doctorDTO.Id = id;
            var result = await _doctorService.DeleteDoctorAsync(doctorDTO);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}