using System;
using System.ComponentModel.DataAnnotations;
using CostManagementAPI.Models;

namespace CostManagementAPI.DTOs
{
    public class InvoiceReportRequest
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        
        [CustomValidation(typeof(InvoiceReportRequest), nameof(ValidateStatus))]
        public string? Status { get; set; }
        public string? ClientId { get; set; }
        
        public static ValidationResult ValidateStatus(string status, ValidationContext context)
        {
            if (status != null && !Invoice.IsValidInvoiceStatus(status))
            {
                return new ValidationResult(
                    $"Invalid invoice status: {status}. Valid values are: {string.Join(", ", Invoice.ValidStatusValues)}");
            }
            
            return ValidationResult.Success;
        }
    }
} 