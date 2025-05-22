using System;
using System.ComponentModel.DataAnnotations;
using CostManagementAPI.Models;

namespace CostManagementAPI.DTOs
{
    public class CreateInvoiceRequest
    {
        [Required]
        public string ClientId { get; set; } = string.Empty;
        
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Invoice amount must be greater than 0")]
        public decimal Amount { get; set; }
        
        [Required]
        public DateTime IssueDate { get; set; }
        
        [Required]
        public DateTime DueDate { get; set; }
        
        [Required]
        [CustomValidation(typeof(CreateInvoiceRequest), nameof(ValidateStatus))]
        public string Status { get; set; } = "Draft";
        
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