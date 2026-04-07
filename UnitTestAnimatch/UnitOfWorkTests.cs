using Animatch.Models;
using Animatch.Services;

namespace UnitTestAnimatch;

[TestFixture]
public class UnitOfWorkTests
{
    [Test]
    public async Task SaveChangesAsync_ShouldPersistPendingChanges()
    {
        using var context = TestDbContextFactory.CreateContext();
        var unitOfWork = new UnitOfWork(context);

        context.Categories.Add(new Category { Name = "Uow Category" });
        var affected = await unitOfWork.SaveChangesAsync();

        Assert.That(affected, Is.GreaterThanOrEqualTo(1));
        Assert.That(context.Categories.Count(), Is.EqualTo(1));
    }

    [Test]
    public async Task SaveChangesAsync_WithNoChanges_ShouldReturnZero()
    {
        using var context = TestDbContextFactory.CreateContext();
        var unitOfWork = new UnitOfWork(context);

        var affected = await unitOfWork.SaveChangesAsync();

        Assert.That(affected, Is.EqualTo(0));
    }
}
