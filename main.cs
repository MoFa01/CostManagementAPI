using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;


// Models
public class Invoice
{
    public int Id { get; set; }
    public string ClientId { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime IssueDate { get; set; }
    public DateTime DueDate { get; set; }
    public InvoiceStatus Status { get; set; }
    public List<Payment> Payments { get; set; } = new();

    public decimal PaidAmount => Payments.Sum(p => p.Amount);
    public decimal RemainingAmount => Amount - PaidAmount;
}

public class Payment
{
    public int Id { get; set; }
    public int InvoiceId { get; set; }
    public decimal Amount { get; set; }
    public PaymentMethod Method { get; set; }
    public DateTime PaymentDate { get; set; }
    public string? Reference { get; set; }
}

public class Receipt
{
    public int Id { get; set; }
    public int InvoiceId { get; set; }
    public int PaymentId { get; set; }
    public string ClientId { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public PaymentMethod Method { get; set; }
    public DateTime IssueDate { get; set; }
    public string ReceiptNumber { get; set; } = string.Empty;
}

public class InvoiceSummaryReport
{
    public DateTime GeneratedDate { get; set; }
    public Dictionary<string, object> Filters { get; set; } = new();
    public List<InvoiceSummaryItem> Items { get; set; } = new();
    public ReportStatistics Statistics { get; set; } = new();
}

public class InvoiceSummaryItem
{
    public int InvoiceId { get; set; }
    public string ClientId { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public decimal PaidAmount { get; set; }
    public decimal RemainingAmount { get; set; }
    public InvoiceStatus Status { get; set; }
    public DateTime IssueDate { get; set; }
    public DateTime DueDate { get; set; }
}

public class ReportStatistics
{
    public int TotalInvoices { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal TotalPaid { get; set; }
    public decimal TotalOutstanding { get; set; }
    public Dictionary<InvoiceStatus, int> StatusBreakdown { get; set; } = new();
}

public enum InvoiceStatus
{
    Draft,
    Pending,
    Paid,
    Unpaid,
    Overdue,
    Cancelled
}

public enum PaymentMethod
{
    Cash,
    Credit,
    Debit,
    BankTransfer,
    Check,
    PayPal,
    Other
}

// DTOs
public class LogPaymentRequest
{
    [Required]
    public int InvoiceId { get; set; }

    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Payment amount must be greater than 0")]
    public decimal Amount { get; set; }

    [Required]
    public PaymentMethod Method { get; set; }

    public string? Reference { get; set; }
}

public class UpdateInvoiceStatusRequest
{
    [Required]
    public int InvoiceId { get; set; }

    [Required]
    public InvoiceStatus Status { get; set; }
}

public class GenerateReceiptRequest
{
    [Required]
    public int InvoiceId { get; set; }

    [Required]
    public int PaymentId { get; set; }
}

public class InvoiceReportRequest
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public InvoiceStatus? Status { get; set; }
    public string? ClientId { get; set; }
}

// Singleton DataStore
public class DataStore
{
    private static readonly Lazy<DataStore> _instance = new(() => new DataStore());
    public static DataStore Instance => _instance.Value;

    private readonly Dictionary<int, Invoice> _invoices = new();
    private readonly Dictionary<int, Payment> _payments = new();
    private readonly Dictionary<int, Receipt> _receipts = new();
    private int _nextInvoiceId = 1;
    private int _nextPaymentId = 1;
    private int _nextReceiptId = 1;

    private DataStore()
    {
        SeedTestData();
    }

    public Dictionary<int, Invoice> Invoices => _invoices;
    public Dictionary<int, Payment> Payments => _payments;
    public Dictionary<int, Receipt> Receipts => _receipts;

    public int GetNextInvoiceId() => _nextInvoiceId++;
    public int GetNextPaymentId() => _nextPaymentId++;
    public int GetNextReceiptId() => _nextReceiptId++;

    private void SeedTestData()
    {
        var invoice1 = new Invoice
        {
            Id = GetNextInvoiceId(),
            ClientId = "CLIENT001",
            Amount = 1000m,
            IssueDate = DateTime.Now.AddDays(-30),
            DueDate = DateTime.Now.AddDays(-10),
            Status = InvoiceStatus.Overdue
        };

        var invoice2 = new Invoice
        {
            Id = GetNextInvoiceId(),
            ClientId = "CLIENT002",
            Amount = 500m,
            IssueDate = DateTime.Now.AddDays(-15),
            DueDate = DateTime.Now.AddDays(15),
            Status = InvoiceStatus.Unpaid
        };

        _invoices[invoice1.Id] = invoice1;
        _invoices[invoice2.Id] = invoice2;
    }
}

// Services
public interface IPaymentLoggingService
{
    Task<Payment> LogPaymentAsync(LogPaymentRequest request);
}

public interface IReceiptGeneratorService
{
    Task<Receipt> GenerateReceiptAsync(GenerateReceiptRequest request);
}

public interface IInvoiceStatusService
{
    Task<Invoice> UpdateInvoiceStatusAsync(UpdateInvoiceStatusRequest request);
    Task<InvoiceStatus> GetInvoiceStatusAsync(int invoiceId);
}

public interface IPaymentHistoryService
{
    Task<List<Payment>> GetPaymentHistoryAsync(int invoiceId);
}

public interface IInvoiceReportService
{
    Task<InvoiceSummaryReport> GenerateInvoiceSummaryReportAsync(InvoiceReportRequest request);
}

public class PaymentLoggingService : IPaymentLoggingService
{
    private readonly DataStore _dataStore = DataStore.Instance;

    public async Task<Payment> LogPaymentAsync(LogPaymentRequest request)
    {
        if (!_dataStore.Invoices.TryGetValue(request.InvoiceId, out var invoice))
            throw new ArgumentException($"Invoice with ID {request.InvoiceId} not found");

        var payment = new Payment
        {
            Id = _dataStore.GetNextPaymentId(),
            InvoiceId = request.InvoiceId,
            Amount = request.Amount,
            Method = request.Method,
            PaymentDate = DateTime.Now,
            Reference = request.Reference
        };

        _dataStore.Payments[payment.Id] = payment;
        invoice.Payments.Add(payment);

        // Update invoice status based on payment
        if (invoice.PaidAmount >= invoice.Amount)
        {
            invoice.Status = InvoiceStatus.Paid;
        }
        else if (invoice.Status == InvoiceStatus.Overdue || invoice.Status == InvoiceStatus.Unpaid)
        {
            invoice.Status = invoice.DueDate < DateTime.Now ? InvoiceStatus.Overdue : InvoiceStatus.Unpaid;
        }

        return await Task.FromResult(payment);
    }
}

public class ReceiptGeneratorService : IReceiptGeneratorService
{
    private readonly DataStore _dataStore = DataStore.Instance;

    public async Task<Receipt> GenerateReceiptAsync(GenerateReceiptRequest request)
    {
        if (!_dataStore.Invoices.TryGetValue(request.InvoiceId, out var invoice))
            throw new ArgumentException($"Invoice with ID {request.InvoiceId} not found");

        if (!_dataStore.Payments.TryGetValue(request.PaymentId, out var payment))
            throw new ArgumentException($"Payment with ID {request.PaymentId} not found");

        if (payment.InvoiceId != request.InvoiceId)
            throw new ArgumentException("Payment does not belong to the specified invoice");

        var receipt = new Receipt
        {
            Id = _dataStore.GetNextReceiptId(),
            InvoiceId = request.InvoiceId,
            PaymentId = request.PaymentId,
            ClientId = invoice.ClientId,
            Amount = payment.Amount,
            Method = payment.Method,
            IssueDate = DateTime.Now,
            ReceiptNumber = $"RCP-{DateTime.Now:yyyyMMdd}-{_dataStore.GetNextReceiptId():D4}"
        };

        _dataStore.Receipts[receipt.Id] = receipt;

        return await Task.FromResult(receipt);
    }
}

public class InvoiceStatusService : IInvoiceStatusService
{
    private readonly DataStore _dataStore = DataStore.Instance;

    public async Task<Invoice> UpdateInvoiceStatusAsync(UpdateInvoiceStatusRequest request)
    {
        if (!_dataStore.Invoices.TryGetValue(request.InvoiceId, out var invoice))
            throw new ArgumentException($"Invoice with ID {request.InvoiceId} not found");

        invoice.Status = request.Status;

        return await Task.FromResult(invoice);
    }

    public async Task<InvoiceStatus> GetInvoiceStatusAsync(int invoiceId)
    {
        if (!_dataStore.Invoices.TryGetValue(invoiceId, out var invoice))
            throw new ArgumentException($"Invoice with ID {invoiceId} not found");

        return await Task.FromResult(invoice.Status);
    }
}

public class PaymentHistoryService : IPaymentHistoryService
{
    private readonly DataStore _dataStore = DataStore.Instance;

    public async Task<List<Payment>> GetPaymentHistoryAsync(int invoiceId)
    {
        if (!_dataStore.Invoices.TryGetValue(invoiceId, out var invoice))
            throw new ArgumentException($"Invoice with ID {invoiceId} not found");

        return await Task.FromResult(invoice.Payments.OrderByDescending(p => p.PaymentDate).ToList());
    }
}

public class InvoiceReportService : IInvoiceReportService
{
    private readonly DataStore _dataStore = DataStore.Instance;

    public async Task<InvoiceSummaryReport> GenerateInvoiceSummaryReportAsync(InvoiceReportRequest request)
    {
        var invoices = _dataStore.Invoices.Values.AsQueryable();

        // Apply filters
        if (request.StartDate.HasValue)
            invoices = invoices.Where(i => i.IssueDate >= request.StartDate.Value);

        if (request.EndDate.HasValue)
            invoices = invoices.Where(i => i.IssueDate <= request.EndDate.Value);

        if (request.Status.HasValue)
            invoices = invoices.Where(i => i.Status == request.Status.Value);

        if (!string.IsNullOrEmpty(request.ClientId))
            invoices = invoices.Where(i => i.ClientId == request.ClientId);

        var filteredInvoices = invoices.ToList();

        var report = new InvoiceSummaryReport
        {
            GeneratedDate = DateTime.Now,
            Filters = new Dictionary<string, object>
            {
                ["StartDate"] = request.StartDate?.ToString("yyyy-MM-dd") ?? "N/A",
                ["EndDate"] = request.EndDate?.ToString("yyyy-MM-dd") ?? "N/A",
                ["Status"] = request.Status?.ToString() ?? "All",
                ["ClientId"] = request.ClientId ?? "All"
            },
            Items = filteredInvoices.Select(i => new InvoiceSummaryItem
            {
                InvoiceId = i.Id,
                ClientId = i.ClientId,
                Amount = i.Amount,
                PaidAmount = i.PaidAmount,
                RemainingAmount = i.RemainingAmount,
                Status = i.Status,
                IssueDate = i.IssueDate,
                DueDate = i.DueDate
            }).ToList(),
            Statistics = new ReportStatistics
            {
                TotalInvoices = filteredInvoices.Count,
                TotalAmount = filteredInvoices.Sum(i => i.Amount),
                TotalPaid = filteredInvoices.Sum(i => i.PaidAmount),
                TotalOutstanding = filteredInvoices.Sum(i => i.RemainingAmount),
                StatusBreakdown = filteredInvoices.GroupBy(i => i.Status)
                    .ToDictionary(g => g.Key, g => g.Count())
            }
        };

        return await Task.FromResult(report);
    }
}

// Controllers
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
    public async Task<ActionResult<InvoiceStatus>> GetStatus(int invoiceId)
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
        return Ok(report);
    }

    [HttpGet]
    public ActionResult<List<Invoice>> GetAllInvoices()
    {
        var invoices = DataStore.Instance.Invoices.Values.ToList();
        return Ok(invoices);
    }
}