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

- `POST /api/Invoice`
  - **Description**: Create a new invoice
  - **Request Body**: 
    ```json
    {
      "clientId": "string",
      "amount": 0.00,
      "issueDate": "2023-05-01T00:00:00Z",
      "dueDate": "2023-06-01T00:00:00Z",
      "status": "Draft"
    }
    ```
  - **Response**: Created invoice object with ID
  - **Status Codes**: 201 Created, 400 Bad Request

- `GET /api/Invoice/{invoiceId}`
  - **Description**: Get invoice details by ID
  - **Path Parameters**: invoiceId (integer)
  - **Response**: Complete invoice object
  - **Status Codes**: 200 OK, 404 Not Found

- `PUT /api/Invoice/status`
  - **Description**: Update invoice status
  - **Request Body**: 
    ```json
    {
      "invoiceId": 0,
      "status": "string"
    }
    ```
  - **Valid Status Values**: Draft, Pending, Paid, Unpaid, Overdue, Cancelled
  - **Response**: Updated invoice object
  - **Status Codes**: 200 OK, 400 Bad Request

- `GET /api/Invoice/{invoiceId}/status`
  - **Description**: Get current status of an invoice
  - **Path Parameters**: invoiceId (integer)
  - **Response**: Status string
  - **Status Codes**: 200 OK, 400 Bad Request

- `GET /api/Invoice/{invoiceId}/payment-history`
  - **Description**: Get payment history for an invoice
  - **Path Parameters**: invoiceId (integer)
  - **Response**: Array of payment objects
  - **Status Codes**: 200 OK, 400 Bad Request

- `POST /api/Invoice/report`
  - **Description**: Generate invoice summary reports with filtering
  - **Request Body**: 
    ```json
    {
      "startDate": "2023-01-01T00:00:00Z",
      "endDate": "2023-12-31T00:00:00Z",
      "status": "string",
      "clientId": "string"
    }
    ```
  - **Response**: Invoice summary report object
  - **Status Codes**: 200 OK, 404 Not Found

- `GET /api/Invoice`
  - **Description**: Get all invoices
  - **Response**: Array of invoice objects
  - **Status Codes**: 200 OK

### Payment Endpoints

- `POST /api/Payment/log`
  - **Description**: Log a new payment for an invoice
  - **Request Body**: 
    ```json
    {
      "invoiceId": 0,
      "amount": 0.00,
      "method": "string",
      "reference": "string"
    }
    ```
  - **Valid Payment Methods**: Cash, CreditCard, BankTransfer, Check, PayPal
  - **Response**: Created payment object
  - **Status Codes**: 200 OK, 400 Bad Request

### Receipt Endpoints

- `POST /api/Receipt/generate`
  - **Description**: Generate a receipt for a payment
  - **Request Body**: 
    ```json
    {
      "invoiceId": 0,
      "paymentId": 0
    }
    ```
  - **Response**: Generated receipt object
  - **Status Codes**: 200 OK, 400 Bad Request

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

