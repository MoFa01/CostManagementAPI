using System.Collections.Generic;
using System.Threading.Tasks;
using CostManagementAPI.Models;

namespace CostManagementAPI.Interfaces
{
    public interface IPaymentHistoryService
    {
        Task<List<Payment>> GetPaymentHistoryAsync(int invoiceId);
    }
} 