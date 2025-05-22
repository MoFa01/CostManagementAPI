using System;
using System.Collections.Generic;
using CostManagementAPI.Models;

namespace CostManagementAPI.Data
{
    public class DataStore
    {
        private static readonly Lazy<DataStore> _instance = new(() => new DataStore());
        public static DataStore Instance => _instance.Value;

        private readonly Dictionary<int, Invoice> _invoices = new();
        private readonly Dictionary<int, Payment> _payments = new();
        private readonly Dictionary<int, Receipt> _receipts = new();
        private int _nextInvoiceId = 1;
        private int _nextPaymentId = 1;
        private int _nextReceiptId = 1;

        private DataStore()
        {
            SeedTestData();
        }

        public Dictionary<int, Invoice> Invoices => _invoices;
        public Dictionary<int, Payment> Payments => _payments;
        public Dictionary<int, Receipt> Receipts => _receipts;

        public int GetNextInvoiceId() => _nextInvoiceId++;
        public int GetNextPaymentId() => _nextPaymentId++;
        public int GetNextReceiptId() => _nextReceiptId++;

        private void SeedTestData()
        {
            var invoice1 = new Invoice
            {
                Id = GetNextInvoiceId(),
                ClientId = "CLIENT001",
                Amount = 1000m,
                IssueDate = DateTime.Now.AddDays(-30),
                DueDate = DateTime.Now.AddDays(-10),
                Status = "Overdue"
            };

            var invoice2 = new Invoice
            {
                Id = GetNextInvoiceId(),
                ClientId = "CLIENT002",
                Amount = 500m,
                IssueDate = DateTime.Now.AddDays(-15),
                DueDate = DateTime.Now.AddDays(15),
                Status = "Unpaid"
            };

            _invoices[invoice1.Id] = invoice1;
            _invoices[invoice2.Id] = invoice2;
        }
    }
} 