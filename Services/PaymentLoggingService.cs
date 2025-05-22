using System;
using System.Threading.Tasks;
using CostManagementAPI.Data;
using CostManagementAPI.DTOs;
using CostManagementAPI.Interfaces;
using CostManagementAPI.Models;

namespace CostManagementAPI.Services
{
    public class PaymentLoggingService : IPaymentLoggingService
    {
        private readonly DataStore _dataStore = DataStore.Instance;
        
        public async Task<Payment> LogPaymentAsync(LogPaymentRequest request)
        {
            if (!_dataStore.Invoices.TryGetValue(request.InvoiceId, out var invoice))
                throw new ArgumentException($"Invoice with ID {request.InvoiceId} not found");

            if (!PaymentMethods.IsValid(request.Method))
                throw new ArgumentException($"Invalid payment method: {request.Method}. Valid values are: {string.Join(", ", PaymentMethods.ValidValues)}");

            if (request.Amount > invoice.RemainingAmount)
                throw new ArgumentException($"Payment amount (${request.Amount}) exceeds the remaining amount (${invoice.RemainingAmount}) for invoice ID {request.InvoiceId}");

            var payment = new Payment
            {
                Id = _dataStore.GetNextPaymentId(),
                InvoiceId = request.InvoiceId,
                Amount = request.Amount,
                Method = request.Method,
                PaymentDate = DateTime.Now,
                Reference = request.Reference
            };

            _dataStore.Payments[payment.Id] = payment;
            invoice.Payments.Add(payment);

            // Update invoice status based on payment
            if (invoice.PaidAmount >= invoice.Amount)
            {
                invoice.Status = "Paid";
            }
            else if (invoice.Status == "Overdue" || invoice.Status == "Unpaid")
            {
                invoice.Status = invoice.DueDate < DateTime.Now ? "Overdue" : "Unpaid";
            }

            return await Task.FromResult(payment);
        }
    }
} 