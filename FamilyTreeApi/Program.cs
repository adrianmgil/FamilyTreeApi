using FamilyTreeApi.DependencyInjection;
using Microsoft.Data.SqlClient;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.RegisterDependencyInjection();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string connectionString = @"Data Source=ADRIANHP\SQLEXPRESS;Initial Catalog=family;Integrated Security=SSPI;TrustServerCertificate=true";

builder.Services.AddTransient(x => new SqlConnection(connectionString));

builder.Services.AddCors(options => options.AddPolicy(
    "ApiCorsPolicy",
    builder =>
    {
        builder.AllowAnyMethod().AllowAnyHeader().SetIsOriginAllowed((host) => true).AllowCredentials();
    })
);
builder.Services.AddControllersWithViews().
                AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                    options.JsonSerializerOptions.PropertyNamingPolicy = null;
                });

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
app.UseCors("ApiCorsPolicy");
app.Run();

