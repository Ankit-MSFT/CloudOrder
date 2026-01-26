using Azure.Data.Tables;
using CloudOrder.Product.Storage;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddSingleton(sp =>
{
    try
    {
        var conn_string = builder.Configuration["Storage:ConnectionString"];
        var tableClient = new TableServiceClient(conn_string).GetTableClient("Products");
        tableClient.CreateIfNotExists();
        return tableClient;
    }
    catch (Exception)
    {
        throw new InvalidOperationException("Azure Table Storage connection is not configured.");
    }
});

builder.Services.AddScoped<IProductRepository, TableProductRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
