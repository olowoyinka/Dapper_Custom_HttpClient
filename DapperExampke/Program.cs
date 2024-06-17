using DapperHttps.DataAccess;
using DapperHttps.Https;
using System.Data;
using System.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IAppDbContext, AppDbContext>()
                .AddScoped<ICustomerRepository, CustomerRepository>()
                .AddScoped<IDbConnection, SqlConnection>(x => new SqlConnection(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHttpClient<GithubHttpClient>(c =>
{
    c.BaseAddress = new Uri("");
    c.Timeout = new TimeSpan(0, 30, 0);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();