# Cost Management API

A robust .NET 6 RESTful API for managing invoices, payments, and receipts in financial systems.

## 🚀 Features

- **Invoice Management**: Create, retrieve, and update invoice status
- **Payment Processing**: Record and track payments against invoices
- **Receipt Generation**: Generate and manage digital receipts for payments
- **Payment History**: Track complete payment history for each invoice
- **Reporting**: Generate detailed invoice summary reports with filtering options

## 📋 API Endpoints

### Invoice Endpoints

- `POST /api/Invoice`: Create a new invoice
- `GET /api/Invoice/{invoiceId}`: Get invoice details by ID
- `PUT /api/Invoice/status`: Update invoice status
- `GET /api/Invoice/{invoiceId}/status`: Get current status of an invoice
- `GET /api/Invoice/{invoiceId}/payment-history`: Get payment history for an invoice
- `POST /api/Invoice/report`: Generate invoice summary reports with filtering
- `GET /api/Invoice`: Get all invoices

### Payment Endpoints

- Payment processing endpoints for recording and managing payments

### Receipt Endpoints

- Receipt generation and management endpoints

## 🛠️ Tech Stack

- **Framework**: .NET 6
- **API Documentation**: Swagger/OpenAPI
- **Architecture**: Service-oriented architecture with dependency injection

## 📦 Project Structure

```
CostManagementAPI/
├── Controllers/          # API controllers defining endpoints
├── Models/               # Data models
├── DTOs/                 # Data Transfer Objects
├── Services/             # Business logic implementation
├── Interfaces/           # Service contracts
├── Data/                 # Data access and storage
```

## 🔧 Getting Started

### Prerequisites

- .NET 6 SDK
- Visual Studio 2022 or VS Code

### Installation

1. Clone the repository
   ```
   git clone https://github.com/MoFa01/CostManagementAPI.git
   ```

2. Navigate to the project directory
   ```
   cd CostManagementAPI
   ```

3. Build the project
   ```
   dotnet build
   ```

4. Run the application
   ```
   dotnet run
   ```

5. Access the Swagger UI
   ```
   http://localhost:3000/swagger
   ```



## 📄 API Documentation

When running the application, navigate to `/swagger` to access the interactive API documentation.

