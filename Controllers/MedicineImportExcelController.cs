using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using SWP391_SE1914_ManageHospital.Models.DTO.RequestDTO.ImportMedicineEX;
using SWP391_SE1914_ManageHospital.Models.Entities;
using SWP391_SE1914_ManageHospital.Service;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;

namespace SWP391_SE1914_ManageHospital.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MedicineImportExcelController : ControllerBase
    {
        private readonly IMedicineImportExcelService _service;
        private readonly ISupplierService _supplierService;

        public MedicineImportExcelController(IMedicineImportExcelService service, ISupplierService supplierService )
        {
            _service = service;
            _supplierService = supplierService;
        }

        // Endpoint để import medicines từ Excel
        [HttpPost("import-medicines")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ImportMedicinesFromExcel(IFormFile file, bool continueImport)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return BadRequest("Vui lòng chọn file Excel để upload.");
                }

                var importRequest = new MedicineImportRequest
                {
                    ImportCode = "AUTO_" + DateTime.UtcNow.Ticks,
                    ImportName = "Import từ Excel",
                    SupplierId = 0, 
                    Details = new List<MedicineImportDetailRequest>()
                };

                using (var stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream);
                    using (var workbook = new XLWorkbook(stream))
                    {
                        var worksheet = workbook.Worksheet(1);
                        var rowCount = worksheet.LastRowUsed().RowNumber();
                       
                        string supplierName = worksheet.Cell(2, 10).GetString();
                        var suppliers = await _supplierService.SearchSupplierByKeyAsync(supplierName);
                        var supplier = suppliers.FirstOrDefault();
                        if (supplier == null)
                        {
                            return BadRequest("Not Found Supplier with name: " + supplierName);
                        }

                        importRequest.SupplierId = supplier.Id;


                        for (int row = 2; row <= rowCount; row++)
                        {
                            var detail = new MedicineImportDetailRequest
                            {
                                MedicineCode = worksheet.Cell(row, 1).GetString(),
                                MedicineName = worksheet.Cell(row, 2).GetString(),
                                UnitName = worksheet.Cell(row, 3).GetString(),
                                BatchNumber = worksheet.Cell(row, 4).GetString(),
                                Quantity = int.TryParse(worksheet.Cell(row, 5).GetString(), out int q) ? q : 0,
                                UnitPrice = decimal.TryParse(worksheet.Cell(row, 6).GetString(), out decimal p) ? p : 0,
                                ManufactureDate = DateTime.TryParse(worksheet.Cell(row, 7).GetString(), out var mfg) ? mfg : DateTime.MinValue,
                                ExpiryDate = DateTime.TryParse(worksheet.Cell(row, 8).GetString(), out var exp) ? exp : DateTime.MinValue,
                                CategoryName = worksheet.Cell(row, 9).GetString()  
                            };

                            importRequest.Details.Add(detail);
                        }
                    }
                }

                var success = await _service.ImportMedicinesAsync(importRequest, continueImport);
                return success ? Ok("Import thành công.") : BadRequest("Import thất bại.");
            }
            catch (Exception ex)
            {
                return BadRequest("Lỗi: " + ex.Message);
            }
        }

        // Endpoint để nhập thủ công (bằng tay)
        [HttpPost("import-by-hand")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ImportMedicinesByHand([FromBody] MedicineImportRequest request, bool continueImport)
        {
            try
            {
                // Kiểm tra nếu các thông tin đầu vào hợp lệ
                if (request == null || request.Details == null || !request.Details.Any())
                {
                    return BadRequest("Dữ liệu nhập vào không hợp lệ.");
                }

                // Thực hiện quá trình import
                var success = await _service.ImportMedicinesAsync(request, continueImport);
                return success ? Ok("Import thành công.") : BadRequest("Import thất bại.");
            }
            catch (Exception ex)
            {
                return BadRequest("Lỗi: " + ex.Message);
            }
        }

        // Endpoint để xác nhận tiếp tục import
        [HttpPost("confirm-import")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public IActionResult ConfirmImport([FromBody] ConfirmImportRequest request)
        {
            if (request.ContinueImport)
            {
                return Ok(new { message = "Import sẽ tiếp tục." });
            }
            else
            {
                return BadRequest(new { message = "Import bị hủy bỏ." });
            }
        }
    }
}
