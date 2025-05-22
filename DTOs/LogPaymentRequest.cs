using System;
using System.ComponentModel.DataAnnotations;
using CostManagementAPI.Models;

namespace CostManagementAPI.DTOs
{
    public class LogPaymentRequest
    {
        [Required]
        public int InvoiceId { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Payment amount must be greater than 0")]
        public decimal Amount { get; set; }

        [Required]
        [CustomValidation(typeof(LogPaymentRequest), nameof(ValidateMethod))]
        public string Method { get; set; } = string.Empty;

        public string? Reference { get; set; }
        
        public static ValidationResult ValidateMethod(string method, ValidationContext context)
        {
            if (!PaymentMethods.IsValid(method))
            {
                return new ValidationResult(
                    $"Invalid payment method: {method}. Valid values are: {string.Join(", ", PaymentMethods.ValidValues)}");
            }
            
            return ValidationResult.Success;
        }
    }
} 