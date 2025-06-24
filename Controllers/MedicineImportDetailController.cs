using System.Net;
using Microsoft.AspNetCore.Mvc;
using SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.MedicineImport;
using SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.MedicineImportDetail;
using SWP391_SE1914_ManageHospital.Models.Entities;
using SWP391_SE1914_ManageHospital.Service;

namespace SWP391_SE1914_ManageHospital.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicineImportDetailController : ControllerBase
    {
        private readonly IMedicineImportDetailService _service;

        public MedicineImportDetailController(IMedicineImportDetailService service)
        {
            _service = service;
        }

        [HttpPost("add-medicine-import-detail")]
        [ProducesResponseType(typeof(IEnumerable<MedicineImportDetail>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AddMedicineImportdetail([FromBody] MedicineImportDetailCreate create)
        {
            try
            {
                var res = await _service.CreateMedicineImportDetail(create);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-all")]
        [ProducesResponseType(typeof(IEnumerable<MedicineImportDetail>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<IEnumerable<MedicineImportDetail>>> GetAllMID()
        {
            try
            {
                var res = await _service.GetAllMedicineImportDetailAsync();
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("find-batch-number/{batchnumber}")]
        [ProducesResponseType(typeof(IEnumerable<MedicineImportDetail>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> FindByBatchNumber(string batchnumber)
        {
            try
            {
                var res = await _service.SearchMedicineImportDetailAsync(batchnumber);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update/{id}")]
        [ProducesResponseType(typeof(IEnumerable<MedicineImportDetail>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateMID([FromBody] MedicineImportDetailUpdate detail, int id)
        {
            try
            {
                var res = await _service.UpdateMedicineImportDetail(detail, id);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
