using System.ComponentModel.DataAnnotations;

namespace CostManagementAPI.DTOs
{
    public class GenerateReceiptRequest
    {
        [Required]
        public int InvoiceId { get; set; }

        [Required]
        public int PaymentId { get; set; }
    }
} 