using System;

namespace CostManagementAPI.Models
{
    public static class PaymentMethods
    {
        public static readonly string[] ValidValues = new[]
        {
            "Cash", "Credit", "Debit", "BankTransfer", "Check", "PayPal"
        };
        
        public static bool IsValid(string method)
        {
            return Array.Exists(ValidValues, m => m.Equals(method, StringComparison.OrdinalIgnoreCase));
        }
    }
} 