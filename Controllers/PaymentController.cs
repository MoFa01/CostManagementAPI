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
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentLoggingService _paymentLoggingService;

        public PaymentController(IPaymentLoggingService paymentLoggingService)
        {
            _paymentLoggingService = paymentLoggingService;
        }

        [HttpPost("log")]
        public async Task<ActionResult<Payment>> LogPayment([FromBody] LogPaymentRequest request)
        {
            try
            {
                var payment = await _paymentLoggingService.LogPaymentAsync(request);
                return Ok(payment);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
} 