using System;

namespace CostManagementAPI.Models
{
    public class Receipt
    {
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public int PaymentId { get; set; }
        public string ClientId { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Method { get; set; } = string.Empty;
        public DateTime IssueDate { get; set; }
        public string ReceiptNumber { get; set; } = string.Empty;
    }
} 