using System;
using System.Collections.Generic;

namespace CostManagementAPI.Models
{
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
        public string Status { get; set; } = string.Empty;
        public DateTime IssueDate { get; set; }
        public DateTime DueDate { get; set; }
    }

    public class ReportStatistics
    {
        public int TotalInvoices { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalPaid { get; set; }
        public decimal TotalOutstanding { get; set; }
        public Dictionary<string, int> StatusBreakdown { get; set; } = new();
    }
} 