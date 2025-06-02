using Microsoft.AspNetCore.Mvc;
using SWP391_SE1914_ManageHospital.Models.DTO.EntitiesDTO;

[Route("api/[controller]")]
[ApiController]
public class DoctorController : ControllerBase
{
    private readonly IDoctorService _doctorService;

    public DoctorController(IDoctorService doctorService)
    {
        _doctorService = doctorService;
    }

    // GET: api/doctor
    [HttpGet]
    public async Task<ActionResult<IEnumerable<DoctorDTO>>> GetDoctors()
    {
        var doctors = await _doctorService.GetAllDoctorsAsync();
        return Ok(doctors);
    }

    // GET: api/doctor/5
    [HttpGet("{id}")]
    public async Task<ActionResult<DoctorDTO>> GetDoctor(int id)
    {
        var doctor = await _doctorService.GetDoctorByIdAsync(id);
        if (doctor == null)
        {
            return NotFound();
        }
        return Ok(doctor);
    }

    // POST: api/doctor
    [HttpPost]
    public async Task<ActionResult<DoctorDTO>> CreateDoctor(DoctorDTO doctorDTO)
    {
        var createdDoctor = await _doctorService.CreateDoctorAsync(doctorDTO);
        return CreatedAtAction(nameof(GetDoctor), new { id = createdDoctor.Id }, createdDoctor);
    }

    // PUT: api/doctor/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateDoctor(int id, DoctorDTO doctorDTO)
    {
        var result = await _doctorService.UpdateDoctorAsync(id, doctorDTO);
        if (!result)
        {
            return BadRequest();
        }
        return NoContent();
    }

    // DELETE: api/doctor/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDoctor(int id)
    {
        var result = await _doctorService.DeleteDoctorAsync(id);
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }
}