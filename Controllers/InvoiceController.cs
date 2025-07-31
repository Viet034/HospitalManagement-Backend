using Microsoft.AspNetCore.Mvc;
using SWP391_SE1914_ManageHospital.Mapper;
using SWP391_SE1914_ManageHospital.Mapper.Impl;
using SWP391_SE1914_ManageHospital.Models.DTO.ResponseDTO;
using SWP391_SE1914_ManageHospital.Models.Entities;
using SWP391_SE1914_ManageHospital.Service;
using SWP391_SE1914_ManageHospital.Service.Impl;
using System.Net;

namespace SWP391_SE1914_ManageHospital.Controllers;
[Route("api/[controller]")]
[ApiController]
public class InvoiceController : ControllerBase
{
    private readonly IInvoiceService _service; 
    private readonly IInvoiceMapper _mapper;  

    // Constructor nhận vào IInvoiceService và IInvoiceMapper
    public InvoiceController(IInvoiceService service, IInvoiceMapper mapper)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }


    [HttpGet("get-by-id/{userId}")]
    [ProducesResponseType(typeof(IEnumerable<Invoice>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> FindById(int userId)
    {
        try
        {
            var response = await _service.GetPaymentsByPatientIdAsync(userId);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("generate-invoice-details/{appointmentId}")]
    public async Task<IActionResult> GenerateInvoiceDetails(int appointmentId)
    {
        try
        {
            var result = await _service.GenerateInvoiceDetailsAsync(appointmentId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
          
    }

    // API tính tổng doanh thu
    [HttpGet("total-revenue")]
    [ProducesResponseType(typeof(decimal), 200)]
    [ProducesResponseType(typeof(string), 400)]
    public async Task<IActionResult> GetTotalRevenue()
    {
        try
        {
            var totalRevenue = await _service.GetTotalRevenueAsync();
            return Ok(totalRevenue);
        }
        catch (Exception ex)
        {
            return BadRequest($"An error occurred: {ex.Message}");
        }
    }


    [HttpGet("total-revenue-month")]
    [ProducesResponseType(typeof(decimal), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> GetTotalRevenueByMonth()
    {
        try
        {
            var totalRevenue = await _service.GetTotalRevenueByMonthAsync();
            return Ok(totalRevenue);
        }
        catch (Exception ex)
        {
            return BadRequest($"An error occurred: {ex.Message}");
        }
    }

    [HttpGet("total-revenue-year")]
    [ProducesResponseType(typeof(decimal), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> GetTotalRevenueByYear()
    {
        try
        {
            var totalRevenue = await _service.GetTotalRevenueByYearAsync();
            return Ok(totalRevenue);
        }
        catch (Exception ex)
        {
            return BadRequest($"An error occurred: {ex.Message}");
        }
    }

    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAllInvoices([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
    {
        try
        {
            var invoiceResponseDTOs = await _service.GetAllInvoicesAsync(startDate, endDate);
            return Ok(invoiceResponseDTOs);
        }
        catch (Exception ex)
        {
            return BadRequest($"Error occurred: {ex.Message}");
        }
    }



}
