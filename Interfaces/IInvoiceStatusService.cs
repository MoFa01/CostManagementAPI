using System.Threading.Tasks;
using CostManagementAPI.DTOs;
using CostManagementAPI.Models;

namespace CostManagementAPI.Interfaces
{
    public interface IInvoiceStatusService
    {
        Task<Invoice> UpdateInvoiceStatusAsync(UpdateInvoiceStatusRequest request);
        Task<string> GetInvoiceStatusAsync(int invoiceId);
    }
} 