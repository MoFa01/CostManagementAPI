using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CostManagementAPI.Data;
using CostManagementAPI.Interfaces;
using CostManagementAPI.Models;

namespace CostManagementAPI.Services
{
    public class PaymentHistoryService : IPaymentHistoryService
    {
        private readonly DataStore _dataStore = DataStore.Instance;

        public async Task<List<Payment>> GetPaymentHistoryAsync(int invoiceId)
        {
            if (!_dataStore.Invoices.TryGetValue(invoiceId, out var invoice))
                throw new ArgumentException($"Invoice with ID {invoiceId} not found");

            return await Task.FromResult(invoice.Payments.OrderByDescending(p => p.PaymentDate).ToList());
        }
    }
} 