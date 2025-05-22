using System;
using System.ComponentModel.DataAnnotations;
using CostManagementAPI.Models;

namespace CostManagementAPI.DTOs
{
    public class UpdateInvoiceStatusRequest
    {
        [Required]
        public int InvoiceId { get; set; }

        [Required]
        [CustomValidation(typeof(UpdateInvoiceStatusRequest), nameof(ValidateStatus))]
        public string Status { get; set; } = string.Empty;
        
        public static ValidationResult ValidateStatus(string status, ValidationContext context)
        {
            if (!Invoice.IsValidInvoiceStatus(status))
            {
                return new ValidationResult(
                    $"Invalid invoice status: {status}. Valid values are: {string.Join(", ", Invoice.ValidStatusValues)}");
            }
            
            return ValidationResult.Success;
        }
    }
} 