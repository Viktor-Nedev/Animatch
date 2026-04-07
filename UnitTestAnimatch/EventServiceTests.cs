using Animatch.Models;
using Animatch.Services;

namespace UnitTestAnimatch;

[TestFixture]
public class EventServiceTests
{
    [Test]
    public async Task GetAllAsync_ShouldReturnAllEvents()
    {
        using var context = TestDbContextFactory.CreateContext();
        var service = new EventService(context);

        context.Set<Event>().AddRange(
            new Event
            {
                Title = "Event 1",
                Description = "Long enough description for event one in get all test.",
                Date = new DateTime(2026, 6, 1, 9, 0, 0),
                Location = "Sofia"
            },
            new Event
            {
                Title = "Event 2",
                Description = "Long enough description for event two in get all test.",
                Date = new DateTime(2026, 6, 2, 9, 0, 0),
                Location = "Varna"
            });
        await context.SaveChangesAsync();

        var items = (await service.GetAllAsync()).ToList();

        Assert.That(items.Count, Is.EqualTo(2));
    }

    [Test]
    public async Task AddUpdateDelete_ShouldWork()
    {
        using var context = TestDbContextFactory.CreateContext();
        var service = new EventService(context);

        var ev = new Event
        {
            Title = "Walking Initiative",
            Description = "This is a full event description with enough length for validation.",
            Date = new DateTime(2026, 5, 1, 10, 0, 0),
            Location = "Sofia",
            CreatedById = "admin-1"
        };

        await service.AddAsync(ev);

        var added = await service.GetByIdAsync(ev.Id);
        Assert.That(added, Is.Not.Null);
        Assert.That(added!.Title, Is.EqualTo("Walking Initiative"));

        added.Title = "Updated Initiative";
        await service.UpdateAsync(added);

        var updated = await service.GetByIdAsync(ev.Id);
        Assert.That(updated, Is.Not.Null);
        Assert.That(updated!.Title, Is.EqualTo("Updated Initiative"));

        await service.DeleteAsync(ev.Id);
        var deleted = await service.GetByIdAsync(ev.Id);
        Assert.That(deleted, Is.Null);
    }

    [Test]
    public async Task GetPagedAsync_ShouldFilterAndPaginate()
    {
        using var context = TestDbContextFactory.CreateContext();
        var service = new EventService(context);

        var events = new List<Event>
        {
            new() { Title = "Walk Alpha", Description = "First long description for event pagination tests.", Date = new DateTime(2026, 5, 1, 10, 0, 0), Location = "Sofia" },
            new() { Title = "Walk Beta", Description = "Second long description for event pagination tests.", Date = new DateTime(2026, 5, 2, 10, 0, 0), Location = "Sofia" },
            new() { Title = "Walk Gamma", Description = "Third long description for event pagination tests.", Date = new DateTime(2026, 5, 3, 10, 0, 0), Location = "Sofia" },
            new() { Title = "Walk Delta", Description = "Fourth long description for event pagination tests.", Date = new DateTime(2026, 5, 4, 10, 0, 0), Location = "Varna" },
            new() { Title = "Other Event", Description = "Fifth long description for event pagination tests.", Date = new DateTime(2026, 5, 5, 10, 0, 0), Location = "Sofia" }
        };

        context.Set<Event>().AddRange(events);
        await context.SaveChangesAsync();

        var (items, totalCount) = await service.GetPagedAsync("Walk", "Sofia", 1, 2);
        var pageItems = items.ToList();

        Assert.That(totalCount, Is.EqualTo(3));
        Assert.That(pageItems.Count, Is.EqualTo(2));
        Assert.That(pageItems.All(e => e.Title.Contains("Walk")), Is.True);
        Assert.That(pageItems.All(e => e.Location.Contains("Sofia")), Is.True);
    }
}
