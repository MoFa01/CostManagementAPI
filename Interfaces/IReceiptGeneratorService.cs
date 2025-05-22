using System.Threading.Tasks;
using CostManagementAPI.DTOs;
using CostManagementAPI.Models;

namespace CostManagementAPI.Interfaces
{
    public interface IReceiptGeneratorService
    {
        Task<Receipt> GenerateReceiptAsync(GenerateReceiptRequest request);
    }
} 