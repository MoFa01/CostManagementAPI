using System;
using System.Threading.Tasks;
using CostManagementAPI.DTOs;
using CostManagementAPI.Interfaces;
using CostManagementAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CostManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReceiptController : ControllerBase
    {
        private readonly IReceiptGeneratorService _receiptGeneratorService;

        public ReceiptController(IReceiptGeneratorService receiptGeneratorService)
        {
            _receiptGeneratorService = receiptGeneratorService;
        }

        [HttpPost("generate")]
        public async Task<ActionResult<Receipt>> GenerateReceipt([FromBody] GenerateReceiptRequest request)
        {
            try
            {
                var receipt = await _receiptGeneratorService.GenerateReceiptAsync(request);
                return Ok(receipt);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
} 