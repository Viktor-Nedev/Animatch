using Animatch.Models;
using Animatch.Services;

namespace UnitTestAnimatch;

[TestFixture]
public class AnimalServiceTests
{
    [Test]
    public async Task GetAllWithCategoryAsync_ShouldReturnAnimalsWithCategory()
    {
        using var context = TestDbContextFactory.CreateContext();
        var service = new AnimalService(context);

        var category = new Category { Name = "Dogs" };
        context.Categories.Add(category);
        await context.SaveChangesAsync();

        context.Animals.Add(new Animal
        {
            Name = "Rex",
            Species = "Dog",
            Description = "Long enough description for get all with category test.",
            PhoneNumber = "0881111111",
            CategoryId = category.Id
        });
        await context.SaveChangesAsync();

        var items = (await service.GetAllWithCategoryAsync()).ToList();

        Assert.That(items.Count, Is.EqualTo(1));
        Assert.That(items[0].Category, Is.Not.Null);
        Assert.That(items[0].Category!.Name, Is.EqualTo("Dogs"));
    }

    [Test]
    public async Task AddUpdateDelete_ShouldWork()
    {
        using var context = TestDbContextFactory.CreateContext();
        var service = new AnimalService(context);

        var category = new Category { Name = "Test Category" };
        context.Categories.Add(category);
        await context.SaveChangesAsync();

        var animal = new Animal
        {
            Name = "Test Animal",
            Species = "Dog",
            Description = "This is a long enough description for a test animal.",
            PhoneNumber = "0881234567",
            CategoryId = category.Id
        };

        await service.AddAsync(animal);

        var added = await service.GetByIdWithCategoryAsync(animal.Id);
        Assert.That(added, Is.Not.Null);
        Assert.That(added!.Name, Is.EqualTo("Test Animal"));

        added.Name = "Updated Animal";
        await service.UpdateAsync(added);

        var updated = await service.GetByIdWithCategoryAsync(animal.Id);
        Assert.That(updated, Is.Not.Null);
        Assert.That(updated!.Name, Is.EqualTo("Updated Animal"));

        await service.DeleteAsync(animal.Id);
        var deleted = await service.GetByIdWithCategoryAsync(animal.Id);
        Assert.That(deleted, Is.Null);
    }

    [Test]
    public async Task ExistsAsync_ShouldReturnTrueOnlyForExistingId()
    {
        using var context = TestDbContextFactory.CreateContext();
        var service = new AnimalService(context);

        var category = new Category { Name = "Exists Category" };
        context.Categories.Add(category);
        await context.SaveChangesAsync();

        var animal = new Animal
        {
            Name = "Exists Animal",
            Species = "Dog",
            Description = "Long enough description for exists method test case.",
            PhoneNumber = "0881234000",
            CategoryId = category.Id
        };
        context.Animals.Add(animal);
        await context.SaveChangesAsync();

        var existsTrue = await service.ExistsAsync(animal.Id);
        var existsFalse = await service.ExistsAsync(animal.Id + 100);

        Assert.That(existsTrue, Is.True);
        Assert.That(existsFalse, Is.False);
    }

    [Test]
    public async Task GetPagedFilteredAsync_ShouldFilterAndPaginate()
    {
        using var context = TestDbContextFactory.CreateContext();
        var service = new AnimalService(context);

        var categoryA = new Category { Name = "Category A" };
        var categoryB = new Category { Name = "Category B" };
        context.Categories.AddRange(categoryA, categoryB);
        await context.SaveChangesAsync();

        var animals = new List<Animal>
        {
            new() { Name = "FilterTarget-1", Species = "Dog", Description = "Long test description number one for filtering.", PhoneNumber = "0880000001", CategoryId = categoryA.Id, Town = "Sofia" },
            new() { Name = "FilterTarget-2", Species = "Dog", Description = "Long test description number two for filtering.", PhoneNumber = "0880000002", CategoryId = categoryA.Id, Town = "Sofia" },
            new() { Name = "FilterTarget-3", Species = "Dog", Description = "Long test description number three for filtering.", PhoneNumber = "0880000003", CategoryId = categoryA.Id, Town = "Sofia" },
            new() { Name = "FilterTarget-4", Species = "Dog", Description = "Long test description number four for filtering.", PhoneNumber = "0880000004", CategoryId = categoryA.Id, Town = "Plovdiv" },
            new() { Name = "OtherName-1", Species = "Dog", Description = "Long test description number five for filtering.", PhoneNumber = "0880000005", CategoryId = categoryB.Id, Town = "Sofia" }
        };
        context.Animals.AddRange(animals);
        await context.SaveChangesAsync();

        var (items, totalCount) = await service.GetPagedFilteredAsync("FilterTarget", categoryA.Id, "Sofia", 1, 2);

        var pageItems = items.ToList();
        Assert.That(totalCount, Is.EqualTo(3));
        Assert.That(pageItems.Count, Is.EqualTo(2));
        Assert.That(pageItems.All(i => i.Name.Contains("FilterTarget")), Is.True);
        Assert.That(pageItems.All(i => i.Town == "Sofia"), Is.True);
        Assert.That(pageItems.All(i => i.CategoryId == categoryA.Id), Is.True);
    }

    [Test]
    public async Task GetWithCoordinatesAsync_ShouldReturnOnlyAnimalsWithCoordinates()
    {
        using var context = TestDbContextFactory.CreateContext();
        var service = new AnimalService(context);

        var category = new Category { Name = "Coordinates Category" };
        context.Categories.Add(category);
        await context.SaveChangesAsync();

        context.Animals.AddRange(
            new Animal
            {
                Name = "With Coordinates",
                Species = "Dog",
                Description = "Long enough description for coordinates test number one.",
                PhoneNumber = "0882233445",
                CategoryId = category.Id,
                Latitude = 42.6977,
                Longitude = 23.3219
            },
            new Animal
            {
                Name = "Without Coordinates",
                Species = "Dog",
                Description = "Long enough description for coordinates test number two.",
                PhoneNumber = "0882233446",
                CategoryId = category.Id
            });
        await context.SaveChangesAsync();

        var items = (await service.GetWithCoordinatesAsync()).ToList();

        Assert.That(items.Count, Is.EqualTo(1));
        Assert.That(items[0].Name, Is.EqualTo("With Coordinates"));
        Assert.That(items[0].Latitude, Is.Not.Null);
        Assert.That(items[0].Longitude, Is.Not.Null);
    }

    [Test]
    public async Task GetByOwnerAsync_ShouldReturnOnlyOwnerAnimals()
    {
        using var context = TestDbContextFactory.CreateContext();
        var service = new AnimalService(context);

        var category = new Category { Name = "Owner Category" };
        context.Categories.Add(category);
        await context.SaveChangesAsync();

        context.Animals.AddRange(
            new Animal
            {
                Name = "Owner Animal 1",
                Species = "Dog",
                Description = "Long enough description for owner filter first animal.",
                PhoneNumber = "0883000001",
                CategoryId = category.Id,
                OwnerId = "owner-1"
            },
            new Animal
            {
                Name = "Owner Animal 2",
                Species = "Dog",
                Description = "Long enough description for owner filter second animal.",
                PhoneNumber = "0883000002",
                CategoryId = category.Id,
                OwnerId = "owner-1"
            },
            new Animal
            {
                Name = "Other Owner Animal",
                Species = "Dog",
                Description = "Long enough description for owner filter third animal.",
                PhoneNumber = "0883000003",
                CategoryId = category.Id,
                OwnerId = "owner-2"
            });
        await context.SaveChangesAsync();

        var items = (await service.GetByOwnerAsync("owner-1")).ToList();

        Assert.That(items.Count, Is.EqualTo(2));
        Assert.That(items.All(a => a.OwnerId == "owner-1"), Is.True);
        Assert.That(items.All(a => a.Category != null), Is.True);
    }

    [Test]
    public async Task GetDistinctTownsAsync_ShouldReturnDistinctSortedAndWithoutEmpty()
    {
        using var context = TestDbContextFactory.CreateContext();
        var service = new AnimalService(context);

        var category = new Category { Name = "Town Category" };
        context.Categories.Add(category);
        await context.SaveChangesAsync();

        context.Animals.AddRange(
            new Animal
            {
                Name = "A1",
                Species = "Dog",
                Description = "Long enough description for towns test one.",
                PhoneNumber = "0884100001",
                CategoryId = category.Id,
                Town = "Sofia"
            },
            new Animal
            {
                Name = "A2",
                Species = "Dog",
                Description = "Long enough description for towns test two.",
                PhoneNumber = "0884100002",
                CategoryId = category.Id,
                Town = "Varna"
            },
            new Animal
            {
                Name = "A3",
                Species = "Dog",
                Description = "Long enough description for towns test three.",
                PhoneNumber = "0884100003",
                CategoryId = category.Id,
                Town = "Sofia"
            },
            new Animal
            {
                Name = "A4",
                Species = "Dog",
                Description = "Long enough description for towns test four.",
                PhoneNumber = "0884100004",
                CategoryId = category.Id,
                Town = " "
            },
            new Animal
            {
                Name = "A5",
                Species = "Dog",
                Description = "Long enough description for towns test five.",
                PhoneNumber = "0884100005",
                CategoryId = category.Id
            });
        await context.SaveChangesAsync();

        var towns = (await service.GetDistinctTownsAsync()).ToList();

        Assert.That(towns.Count, Is.EqualTo(2));
        Assert.That(towns, Is.EqualTo(new[] { "Sofia", "Varna" }));
    }
}
