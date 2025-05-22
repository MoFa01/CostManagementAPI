using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CostManagementAPI.Data;
using CostManagementAPI.DTOs;
using CostManagementAPI.Interfaces;
using CostManagementAPI.Models;

namespace CostManagementAPI.Services
{
    public class InvoiceReportService : IInvoiceReportService
    {
        private readonly DataStore _dataStore = DataStore.Instance;

        public async Task<InvoiceSummaryReport> GenerateInvoiceSummaryReportAsync(InvoiceReportRequest request)
        {
            var invoices = _dataStore.Invoices.Values.AsQueryable();

            // Apply filters
            if (request.StartDate.HasValue)
                invoices = invoices.Where(i => i.IssueDate >= request.StartDate.Value);

            if (request.EndDate.HasValue)
                invoices = invoices.Where(i => i.IssueDate <= request.EndDate.Value);

            if (!string.IsNullOrEmpty(request.Status))
                invoices = invoices.Where(i => i.Status == request.Status);

            if (!string.IsNullOrEmpty(request.ClientId))
                invoices = invoices.Where(i => i.ClientId == request.ClientId);

            var filteredInvoices = invoices.ToList();

            var report = new InvoiceSummaryReport
            {
                GeneratedDate = DateTime.Now,
                Filters = new Dictionary<string, object>
                {
                    ["StartDate"] = request.StartDate?.ToString("yyyy-MM-dd") ?? "N/A",
                    ["EndDate"] = request.EndDate?.ToString("yyyy-MM-dd") ?? "N/A",
                    ["Status"] = request.Status ?? "All",
                    ["ClientId"] = request.ClientId ?? "All"
                },
                Items = filteredInvoices.Select(i => new InvoiceSummaryItem
                {
                    InvoiceId = i.Id,
                    ClientId = i.ClientId,
                    Amount = i.Amount,
                    PaidAmount = i.PaidAmount,
                    RemainingAmount = i.RemainingAmount,
                    Status = i.Status,
                    IssueDate = i.IssueDate,
                    DueDate = i.DueDate
                }).ToList(),
                Statistics = new ReportStatistics
                {
                    TotalInvoices = filteredInvoices.Count,
                    TotalAmount = filteredInvoices.Sum(i => i.Amount),
                    TotalPaid = filteredInvoices.Sum(i => i.PaidAmount),
                    TotalOutstanding = filteredInvoices.Sum(i => i.RemainingAmount),
                    StatusBreakdown = filteredInvoices.GroupBy(i => i.Status)
                        .ToDictionary(g => g.Key, g => g.Count())
                }
            };

            return await Task.FromResult(report);
        }
    }
} 