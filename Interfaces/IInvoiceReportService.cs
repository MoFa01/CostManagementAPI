using System.Threading.Tasks;
using CostManagementAPI.DTOs;
using CostManagementAPI.Models;

namespace CostManagementAPI.Interfaces
{
    public interface IInvoiceReportService
    {
        Task<InvoiceSummaryReport> GenerateInvoiceSummaryReportAsync(InvoiceReportRequest request);
    }
} 