using CostManagementAPI.Interfaces;
using CostManagementAPI.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register application services
builder.Services.AddSingleton<IPaymentLoggingService, PaymentLoggingService>();
builder.Services.AddSingleton<IReceiptGeneratorService, ReceiptGeneratorService>();
builder.Services.AddSingleton<IInvoiceStatusService, InvoiceStatusService>();
builder.Services.AddSingleton<IPaymentHistoryService, PaymentHistoryService>();
builder.Services.AddSingleton<IInvoiceReportService, InvoiceReportService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
