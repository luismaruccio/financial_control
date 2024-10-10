using FinancialControl.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using FinancialControl.IoC.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<FinancialControlDbContext>(options =>
     options.UseNpgsql(GetDatabaseConnectionString(builder.Configuration),
                      b => b.MigrationsAssembly("FinancialControl.Infrastructure")));

builder.Services.AddDependencies();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

ApplyMigrations(app);

await app.RunAsync();

static string? GetDatabaseConnectionString(IConfiguration configuration)
{
    var envConnectionString = Environment.GetEnvironmentVariable("DATABASE_CONNECTION_STRING");
    if (string.IsNullOrEmpty(envConnectionString))
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        return connectionString;
    }
    return envConnectionString;
}

static void ApplyMigrations(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<FinancialControlDbContext>();
    dbContext.Database.Migrate();
}