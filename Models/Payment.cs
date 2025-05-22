using System;

namespace CostManagementAPI.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public decimal Amount { get; set; }
        public string Method { get; set; } = string.Empty;
        public DateTime PaymentDate { get; set; }
        public string? Reference { get; set; }
    }
} 