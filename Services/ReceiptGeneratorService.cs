using System;
using System.Threading.Tasks;
using CostManagementAPI.Data;
using CostManagementAPI.DTOs;
using CostManagementAPI.Interfaces;
using CostManagementAPI.Models;

namespace CostManagementAPI.Services
{
    public class ReceiptGeneratorService : IReceiptGeneratorService
    {
        private readonly DataStore _dataStore = DataStore.Instance;
        
        public async Task<Receipt> GenerateReceiptAsync(GenerateReceiptRequest request)
        {
            if (!_dataStore.Invoices.TryGetValue(request.InvoiceId, out var invoice))
                throw new ArgumentException($"Invoice with ID {request.InvoiceId} not found");

            if (!_dataStore.Payments.TryGetValue(request.PaymentId, out var payment))
                throw new ArgumentException($"Payment with ID {request.PaymentId} not found");

            if (payment.InvoiceId != request.InvoiceId)
                throw new ArgumentException("Payment does not belong to the specified invoice");

            if (!PaymentMethods.IsValid(payment.Method))
                throw new ArgumentException($"Invalid payment method: {payment.Method}. Valid values are: {string.Join(", ", PaymentMethods.ValidValues)}");

            var receipt = new Receipt
            {
                Id = _dataStore.GetNextReceiptId(),
                InvoiceId = request.InvoiceId,
                PaymentId = request.PaymentId,
                ClientId = invoice.ClientId,
                Amount = payment.Amount,
                Method = payment.Method,
                IssueDate = DateTime.Now,
                ReceiptNumber = $"RCP-{DateTime.Now:yyyyMMdd}-{_dataStore.GetNextReceiptId():D4}"
            };

            _dataStore.Receipts[receipt.Id] = receipt;

            return await Task.FromResult(receipt);
        }
    }
} 