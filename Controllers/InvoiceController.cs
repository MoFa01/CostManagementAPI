using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CostManagementAPI.Data;
using CostManagementAPI.DTOs;
using CostManagementAPI.Interfaces;
using CostManagementAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CostManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceStatusService _invoiceStatusService;
        private readonly IPaymentHistoryService _paymentHistoryService;
        private readonly IInvoiceReportService _invoiceReportService;

        public InvoiceController(
            IInvoiceStatusService invoiceStatusService,
            IPaymentHistoryService paymentHistoryService,
            IInvoiceReportService invoiceReportService)
        {
            _invoiceStatusService = invoiceStatusService;
            _paymentHistoryService = paymentHistoryService;
            _invoiceReportService = invoiceReportService;
        }

        [HttpPost]
        public ActionResult<Invoice> CreateInvoice([FromBody] CreateInvoiceRequest request)
        {
            try
            {
                if (request.DueDate < request.IssueDate)
                {
                    return BadRequest("Due date cannot be earlier than issue date");
                }

                var invoice = new Invoice
                {
                    Id = DataStore.Instance.GetNextInvoiceId(),
                    ClientId = request.ClientId,
                    Amount = request.Amount,
                    IssueDate = request.IssueDate,
                    DueDate = request.DueDate,
                    Status = request.Status
                };

                DataStore.Instance.Invoices[invoice.Id] = invoice;
                return CreatedAtAction(nameof(GetInvoice), new { invoiceId = invoice.Id }, invoice);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{invoiceId}")]
        public ActionResult<Invoice> GetInvoice(int invoiceId)
        {
            if (!DataStore.Instance.Invoices.TryGetValue(invoiceId, out var invoice))
                return NotFound($"Invoice with ID {invoiceId} not found");
                
            return Ok(invoice);
        }

        [HttpPut("status")]
        public async Task<ActionResult<Invoice>> UpdateStatus([FromBody] UpdateInvoiceStatusRequest request)
        {
            try
            {
                var invoice = await _invoiceStatusService.UpdateInvoiceStatusAsync(request);
                return Ok(invoice);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{invoiceId}/status")]
        public async Task<ActionResult<string>> GetStatus(int invoiceId)
        {
            try
            {
                var status = await _invoiceStatusService.GetInvoiceStatusAsync(invoiceId);
                return Ok(status);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{invoiceId}/payment-history")]
        public async Task<ActionResult<List<Payment>>> GetPaymentHistory(int invoiceId)
        {
            try
            {
                var history = await _paymentHistoryService.GetPaymentHistoryAsync(invoiceId);
                return Ok(history);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("report")]
        public async Task<ActionResult<InvoiceSummaryReport>> GenerateReport([FromBody] InvoiceReportRequest request)
        {
            var report = await _invoiceReportService.GenerateInvoiceSummaryReportAsync(request);
            
            if (report.Items.Count == 0)
            {
                string errorMessage = "No invoices found";
                
                if (!string.IsNullOrEmpty(request.ClientId))
                    errorMessage += $" for client '{request.ClientId}'";
                    
                if (request.StartDate.HasValue || request.EndDate.HasValue)
                {
                    errorMessage += " in the specified date range";
                    if (request.StartDate.HasValue)
                        errorMessage += $" from {request.StartDate.Value:yyyy-MM-dd}";
                    if (request.EndDate.HasValue)
                        errorMessage += $" to {request.EndDate.Value:yyyy-MM-dd}";
                }
                
                if (!string.IsNullOrEmpty(request.Status))
                    errorMessage += $" with status '{request.Status}'";
                    
                return NotFound(errorMessage);
            }
            
            return Ok(report);
        }

        [HttpGet]
        public ActionResult<List<Invoice>> GetAllInvoices()
        {
            var invoices = DataStore.Instance.Invoices.Values.ToList();
            return Ok(invoices);
        }
    }
} 