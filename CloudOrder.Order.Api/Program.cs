using Azure.Data.Tables;
using CloudOrder.Order.Storage;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddSingleton(sp =>
{
    try
    {
        var conn_string = builder.Configuration["Storage:ConnectionString"];
        var tableClient = new TableServiceClient(conn_string).GetTableClient("Orders");
        tableClient.CreateIfNotExists();
        return tableClient;
    }
    catch (Exception)
    {
        throw new InvalidOperationException("Azure Table Storage connection is not configured.");
    }
});

builder.Services.AddScoped<IOrderRepository, TableOrderRepository>();

// HttpClient for Product API calls
builder.Services.AddHttpClient("ProductApi", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["Services:ProductApi"] ?? "http://localhost:5144");
});

// HttpClient for Customer API calls
builder.Services.AddHttpClient("CustomerApi", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["Services:CustomerApi"] ?? "http://localhost:5047");
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
