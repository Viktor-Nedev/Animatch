using Animatch.Data;
using Microsoft.EntityFrameworkCore;

namespace UnitTestAnimatch;

public static class TestDbContextFactory
{
    public static AnimalManagerDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AnimalManagerDbContext>()
            .UseInMemoryDatabase($"AnimatchTests_{Guid.NewGuid()}")
            .Options;

        return new AnimalManagerDbContext(options);
    }
}
