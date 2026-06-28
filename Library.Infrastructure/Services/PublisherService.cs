using Library.Application.DTOs.Publishers;

namespace Library.Infrastructure.Services;

public sealed class PublisherService : IPublisherService
{
    private readonly IPublisherRepository _publisherRepository;
    private readonly IActivityLogService _activityLogService;
    private readonly ICurrentUserService _currentUser;

    public PublisherService(
        IPublisherRepository publisherRepository,
        IActivityLogService activityLogService,
        ICurrentUserService currentUser)
    {
        _publisherRepository = publisherRepository;
        _activityLogService = activityLogService;
        _currentUser = currentUser;
    }

    public async Task<IEnumerable<PublisherDto>> GetAllAsync(CancellationToken ct = default)
        => await _publisherRepository.Query()
            .Where(p => p.DeletedAt == null)
            .AsNoTracking()
            .Select(p => new PublisherDto
            {
                Id = p.Id,
                Name = p.Name,
                Website = p.Website
            })
            .ToListAsync(ct);

    public async Task<PublisherDetailsDto> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var publisher = await _publisherRepository.Query()
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id && p.DeletedAt == null, ct)
            ?? throw new KeyNotFoundException("Publisher not found.");

        return new PublisherDetailsDto
        {
            Id = publisher.Id,
            Name = publisher.Name,
            Address = publisher.Address,
            ContactEmail = publisher.ContactEmail,
            Phone = publisher.Phone,
            Website = publisher.Website,
            BookCount = await _publisherRepository.HasActiveBooksAsync(id, ct)
                               ? await _publisherRepository.Query()
                                   .CountAsync(p => p.Id == id, ct)
                               : 0
        };
    }

    public async Task<int> CreateAsync(CreatePublisherDto request, CancellationToken ct = default)
    {
        if (await _publisherRepository.ExistsByNameAsync(request.Name, ct: ct))
            throw new InvalidOperationException($"Publisher '{request.Name}' already exists.");

        var publisher = new Publisher
        {
            Name = request.Name,
            Address = request.Address,
            ContactEmail = request.ContactEmail,
            Phone = request.Phone,
            Website = request.Website,
            CreatedAt = DateTime.UtcNow
        };

        await _publisherRepository.AddAsync(publisher, ct);
        await _publisherRepository.SaveChangesAsync(ct);

        await _activityLogService.LogAsync(
            _currentUser.UserId, "CreatePublisher", nameof(Publisher),
            publisher.Id, $"Publisher '{publisher.Name}' created.", ct: ct);

        return publisher.Id;
    }

    public async Task UpdateAsync(int id, UpdatePublisherDto request, CancellationToken ct = default)
    {
        var publisher = await GetActiveOrThrowAsync(id, ct);

        if (await _publisherRepository.ExistsByNameAsync(request.Name, excludeId: id, ct: ct))
            throw new InvalidOperationException($"Publisher '{request.Name}' already exists.");

        publisher.Name = request.Name;
        publisher.Address = request.Address;
        publisher.ContactEmail = request.ContactEmail;
        publisher.Phone = request.Phone;
        publisher.Website = request.Website;
        publisher.ModifiedAt = DateTime.UtcNow;

        _publisherRepository.Update(publisher);
        await _publisherRepository.SaveChangesAsync(ct);

        await _activityLogService.LogAsync(
            _currentUser.UserId, "UpdatePublisher", nameof(Publisher),
            publisher.Id, $"Publisher '{publisher.Name}' updated.", ct: ct);
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        var publisher = await GetActiveOrThrowAsync(id, ct);

        if (await _publisherRepository.HasActiveBooksAsync(id, ct))
            throw new InvalidOperationException("Cannot delete a publisher that has active books.");

        publisher.DeletedAt = DateTime.UtcNow;
        publisher.ModifiedAt = DateTime.UtcNow;

        _publisherRepository.Update(publisher);
        await _publisherRepository.SaveChangesAsync(ct);

        await _activityLogService.LogAsync(
            _currentUser.UserId, "DeletePublisher", nameof(Publisher),
            publisher.Id, $"Publisher '{publisher.Name}' deleted.", ct: ct);
    }

    // ── Helpers ────────────────────────────────────────────────────────────────

    private async Task<Publisher> GetActiveOrThrowAsync(int id, CancellationToken ct)
        => await _publisherRepository.Query()
               .FirstOrDefaultAsync(p => p.Id == id && p.DeletedAt == null, ct)
           ?? throw new KeyNotFoundException("Publisher not found.");
}
