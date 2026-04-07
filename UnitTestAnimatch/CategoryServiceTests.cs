using Animatch.Models;
using Animatch.Services;

namespace UnitTestAnimatch;

[TestFixture]
public class CategoryServiceTests
{
    [Test]
    public async Task AddUpdateDelete_ShouldWork()
    {
        using var context = TestDbContextFactory.CreateContext();
        var service = new CategoryService(context);

        var category = new Category { Name = "Initial Category" };
        await service.AddAsync(category);

        var added = await service.GetByIdAsync(category.Id);
        Assert.That(added, Is.Not.Null);
        Assert.That(added!.Name, Is.EqualTo("Initial Category"));

        added.Name = "Updated Category";
        await service.UpdateAsync(added);

        var updated = await service.GetByIdAsync(category.Id);
        Assert.That(updated, Is.Not.Null);
        Assert.That(updated!.Name, Is.EqualTo("Updated Category"));

        await service.DeleteAsync(category.Id);
        var deleted = await service.GetByIdAsync(category.Id);
        Assert.That(deleted, Is.Null);
    }

    [Test]
    public async Task GetAllAsync_ShouldReturnAllCategories()
    {
        using var context = TestDbContextFactory.CreateContext();
        var service = new CategoryService(context);

        context.Categories.AddRange(
            new Category { Name = "Dogs" },
            new Category { Name = "Cats" });
        await context.SaveChangesAsync();

        var categories = (await service.GetAllAsync()).ToList();

        Assert.That(categories.Count, Is.EqualTo(2));
        Assert.That(categories.Select(c => c.Name), Is.EquivalentTo(new[] { "Dogs", "Cats" }));
    }

    [Test]
    public async Task DeleteAsync_WithMissingId_ShouldNotThrowAndKeepData()
    {
        using var context = TestDbContextFactory.CreateContext();
        var service = new CategoryService(context);

        context.Categories.Add(new Category { Name = "Only Category" });
        await context.SaveChangesAsync();

        Assert.DoesNotThrowAsync(async () => await service.DeleteAsync(9999));
        var count = context.Categories.Count();

        Assert.That(count, Is.EqualTo(1));
    }
}
