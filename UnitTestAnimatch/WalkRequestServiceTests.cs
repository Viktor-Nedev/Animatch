using Animatch.Models;
using Animatch.Services;

namespace UnitTestAnimatch;

[TestFixture]
public class WalkRequestServiceTests
{
    [Test]
    public async Task GetAllAsync_ShouldReturnAllRequestsOrderedByRequestedOn()
    {
        using var context = TestDbContextFactory.CreateContext();
        var service = new WalkRequestService(context);

        var category = new Category { Name = "All Requests Category" };
        context.Categories.Add(category);
        await context.SaveChangesAsync();

        var animal = new Animal
        {
            Name = "Request Animal",
            Species = "Dog",
            Description = "Long enough description for get all requests test animal.",
            PhoneNumber = "0885000001",
            CategoryId = category.Id,
            OwnerId = "owner-10"
        };
        context.Animals.Add(animal);
        await context.SaveChangesAsync();

        context.WalkRequests.AddRange(
            new WalkRequest
            {
                AnimalId = animal.Id,
                RequesterId = "user-1",
                Status = RequestStatus.Pending,
                Message = "First message for all requests test.",
                RequestedOn = new DateTime(2026, 4, 1, 10, 0, 0, DateTimeKind.Utc)
            },
            new WalkRequest
            {
                AnimalId = animal.Id,
                RequesterId = "user-2",
                Status = RequestStatus.Pending,
                Message = "Second message for all requests test.",
                RequestedOn = new DateTime(2026, 4, 2, 10, 0, 0, DateTimeKind.Utc)
            });
        await context.SaveChangesAsync();

        var items = (await service.GetAllAsync()).ToList();

        Assert.That(items.Count, Is.EqualTo(2));
        Assert.That(items[0].RequesterId, Is.EqualTo("user-2"));
        Assert.That(items[1].RequesterId, Is.EqualTo("user-1"));
        Assert.That(items.All(r => r.Animal != null), Is.True);
    }

    [Test]
    public async Task AddUpdateAndGet_ShouldWork()
    {
        using var context = TestDbContextFactory.CreateContext();
        var service = new WalkRequestService(context);

        var category = new Category { Name = "Requests Category" };
        context.Categories.Add(category);
        await context.SaveChangesAsync();

        var animal = new Animal
        {
            Name = "Owner Animal",
            Species = "Dog",
            Description = "This description is long enough for creating an animal in tests.",
            PhoneNumber = "0887000001",
            CategoryId = category.Id,
            OwnerId = "owner-1",
            Town = "Sofia"
        };
        context.Animals.Add(animal);
        await context.SaveChangesAsync();

        var request = new WalkRequest
        {
            AnimalId = animal.Id,
            RequesterId = "requester-1",
            Status = RequestStatus.Pending,
            Message = "I can help this weekend.",
            RequestedOn = DateTime.UtcNow
        };

        await service.AddAsync(request);

        var added = await service.GetByIdAsync(request.Id);
        Assert.That(added, Is.Not.Null);
        Assert.That(added!.Animal, Is.Not.Null);
        Assert.That(added.RequesterId, Is.EqualTo("requester-1"));

        var hasPending = await service.HasPendingAsync(animal.Id, "requester-1");
        Assert.That(hasPending, Is.True);

        added.Status = RequestStatus.Approved;
        await service.UpdateAsync(added);

        var hasPendingAfterUpdate = await service.HasPendingAsync(animal.Id, "requester-1");
        Assert.That(hasPendingAfterUpdate, Is.False);
    }

    [Test]
    public async Task Filters_ShouldReturnByRequesterAndByOwner()
    {
        using var context = TestDbContextFactory.CreateContext();
        var service = new WalkRequestService(context);

        var category = new Category { Name = "Requests Category 2" };
        context.Categories.Add(category);
        await context.SaveChangesAsync();

        var ownerAnimal = new Animal
        {
            Name = "Owner Animal 2",
            Species = "Dog",
            Description = "This description is long enough for creating another animal in tests.",
            PhoneNumber = "0887000002",
            CategoryId = category.Id,
            OwnerId = "owner-2",
            Town = "Plovdiv"
        };
        var otherAnimal = new Animal
        {
            Name = "Other Animal",
            Species = "Dog",
            Description = "This description is long enough for creating a second owner animal.",
            PhoneNumber = "0887000003",
            CategoryId = category.Id,
            OwnerId = "owner-3",
            Town = "Varna"
        };

        context.Animals.AddRange(ownerAnimal, otherAnimal);
        await context.SaveChangesAsync();

        context.WalkRequests.AddRange(
            new WalkRequest
            {
                AnimalId = ownerAnimal.Id,
                RequesterId = "requester-A",
                Status = RequestStatus.Pending,
                Message = "I can help with this walk request.",
                RequestedOn = DateTime.UtcNow.AddHours(-2)
            },
            new WalkRequest
            {
                AnimalId = ownerAnimal.Id,
                RequesterId = "requester-B",
                Status = RequestStatus.Pending,
                Message = "I can also help with this walk request.",
                RequestedOn = DateTime.UtcNow.AddHours(-1)
            },
            new WalkRequest
            {
                AnimalId = otherAnimal.Id,
                RequesterId = "requester-A",
                Status = RequestStatus.Pending,
                Message = "Different owner incoming request example.",
                RequestedOn = DateTime.UtcNow
            });
        await context.SaveChangesAsync();

        var byRequester = (await service.GetByRequesterAsync("requester-A")).ToList();
        var incomingForOwner = (await service.GetIncomingForOwnerAsync("owner-2")).ToList();

        Assert.That(byRequester.Count, Is.EqualTo(2));
        Assert.That(incomingForOwner.Count, Is.EqualTo(2));
        Assert.That(incomingForOwner.All(r => r.Animal.OwnerId == "owner-2"), Is.True);
    }
}
