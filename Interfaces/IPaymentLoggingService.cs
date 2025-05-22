using System.Threading.Tasks;
using CostManagementAPI.DTOs;
using CostManagementAPI.Models;

namespace CostManagementAPI.Interfaces
{
    public interface IPaymentLoggingService
    {
        Task<Payment> LogPaymentAsync(LogPaymentRequest request);
    }
} 