using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SWP391_SE1914_ManageHospital.Data;
using SWP391_SE1914_ManageHospital.Models.Entities;

namespace SWP391_SE1914_ManageHospital.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LaboratoryStaffController : ControllerBase
    {
        private readonly ApplicationDBContext _context;

        public LaboratoryStaffController(ApplicationDBContext context)
        {
            _context = context;
        }

        // GET: api/LaboratoryStaff
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LaboratoryStaff>>> GetAll()
        {
            return await _context.LaboratoryStaffs.ToListAsync();
        }

        // GET: api/LaboratoryStaff/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LaboratoryStaff>> Get(int id)
        {
            var staff = await _context.LaboratoryStaffs.FindAsync(id);
            if (staff == null) return NotFound();
            return staff;
        }

        // POST: api/LaboratoryStaff
        [HttpPost]
        public async Task<ActionResult<LaboratoryStaff>> Create(LaboratoryStaff staff)
        {
            staff.CreatedAt = DateTime.UtcNow;
            _context.LaboratoryStaffs.Add(staff);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = staff.Id }, staff);
        }

        // PUT: api/LaboratoryStaff/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, LaboratoryStaff updatedStaff)
        {
            if (id != updatedStaff.Id) return BadRequest();

            _context.Entry(updatedStaff).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/LaboratoryStaff/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var staff = await _context.LaboratoryStaffs.FindAsync(id);
            if (staff == null) return NotFound();

            _context.LaboratoryStaffs.Remove(staff);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }


}
