using FinancialControl.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FinancialControl.Infrastructure.Tests.InMemoryDB;

public class InMemoryDBContextFactory
{
    private readonly FinancialControlDbContext _context;

    public InMemoryDBContextFactory(string databaseName = "InMemoryTest")
    {
        var option = new DbContextOptionsBuilder<FinancialControlDbContext>()
            .UseInMemoryDatabase(databaseName)
            .Options;

        _context = new FinancialControlDbContext(option);
    }

    public FinancialControlDbContext GetContext() 
        => _context;
}
