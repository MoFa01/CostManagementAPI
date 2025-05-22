using System;
using System.Threading.Tasks;
using CostManagementAPI.Data;
using CostManagementAPI.DTOs;
using CostManagementAPI.Interfaces;
using CostManagementAPI.Models;

namespace CostManagementAPI.Services
{
    public class InvoiceStatusService : IInvoiceStatusService
    {
        private readonly DataStore _dataStore = DataStore.Instance;

        public async Task<Invoice> UpdateInvoiceStatusAsync(UpdateInvoiceStatusRequest request)
        {
            if (!_dataStore.Invoices.TryGetValue(request.InvoiceId, out var invoice))
                throw new ArgumentException($"Invoice with ID {request.InvoiceId} not found");

            invoice.Status = request.Status;

            return await Task.FromResult(invoice);
        }

        public async Task<string> GetInvoiceStatusAsync(int invoiceId)
        {
            if (!_dataStore.Invoices.TryGetValue(invoiceId, out var invoice))
                throw new ArgumentException($"Invoice with ID {invoiceId} not found");

            return await Task.FromResult(invoice.Status);
        }
    }
} 