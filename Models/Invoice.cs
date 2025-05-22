using System;
using System.Collections.Generic;
using System.Linq;

namespace CostManagementAPI.Models
{
    public class Invoice
    {
        public int Id { get; set; }
        public string ClientId { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime DueDate { get; set; }
        
        private string _status = string.Empty;
        public string Status 
        { 
            get => _status;
            set 
            {
                if (!IsValidInvoiceStatus(value))
                    throw new ArgumentException($"Invalid invoice status: {value}. Valid values are: Draft, Pending, Paid, Unpaid, Overdue, Cancelled");
                _status = value;
            }
        }
        
        public List<Payment> Payments { get; set; } = new();

        public decimal PaidAmount => Payments.Sum(p => p.Amount);
        public decimal RemainingAmount => Amount - PaidAmount;
        
        public static bool IsValidInvoiceStatus(string status)
        {
            return status == "Draft" || 
                   status == "Pending" || 
                   status == "Paid" || 
                   status == "Unpaid" || 
                   status == "Overdue" || 
                   status == "Cancelled";
        }
        
        public static readonly string[] ValidStatusValues = new[] 
        {
            "Draft", "Pending", "Paid", "Unpaid", "Overdue", "Cancelled"
        };
    }
} 