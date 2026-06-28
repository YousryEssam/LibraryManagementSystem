namespace Library.Infrastructure.Services;

public sealed class AuthorService : IAuthorService
{
    private readonly IAuthorRepository _authorRepository;
    private readonly IActivityLogService _activityLogService;
    private readonly ICurrentUserService _currentUser;

    public AuthorService(
        IAuthorRepository authorRepository,
        IActivityLogService activityLogService,
        ICurrentUserService currentUser)
    {
        _authorRepository = authorRepository;
        _activityLogService = activityLogService;
        _currentUser = currentUser;
    }

    public async Task<IEnumerable<AuthorDto>> GetAllAsync(CancellationToken ct = default)
    {
        var authors = await _authorRepository.Query()
            .Where(a => a.DeletedAt == null)
            .AsNoTracking()
            .Select(a => new AuthorDto
            {
                Id = a.Id,
                FullName = a.FullName,
                Nationality = a.Nationality
            })
            .ToListAsync(ct);

        return authors;
    }

    public async Task<AuthorDetailsDto> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var author = await _authorRepository.GetWithBooksAsync(id, ct)
            ?? throw new KeyNotFoundException("Author not found.");

        return new AuthorDetailsDto
        {
            Id = author.Id,
            FullName = author.FullName,
            Bio = author.Bio,
            Nationality = author.Nationality,
            BookCount = author.BookAuthors?.Count(ba => ba.Book.DeletedAt == null) ?? 0
        };
    }

    public async Task<int> CreateAsync(CreateAuthorDto request, CancellationToken ct = default)
    {
        var author = new Author
        {
            FullName = request.FullName,
            Bio = request.Bio,
            Nationality = request.Nationality,
            CreatedAt = DateTime.UtcNow
        };

        await _authorRepository.AddAsync(author, ct);
        await _authorRepository.SaveChangesAsync(ct);

        await _activityLogService.LogAsync(
            _currentUser.UserId,
            "CreateAuthor",
            nameof(Author),
            author.Id,
            $"Author '{author.FullName}' created.",
            ct: ct);

        return author.Id;
    }

    public async Task UpdateAsync(int id, UpdateAuthorDto request, CancellationToken ct = default)
    {
        var author = await _authorRepository.GetByIdAsync(id, ct)
            ?? throw new KeyNotFoundException("Author not found.");

        if (author.DeletedAt is not null)
            throw new KeyNotFoundException("Author not found.");

        author.FullName = request.FullName;
        author.Bio = request.Bio;
        author.Nationality = request.Nationality;
        author.ModifiedAt = DateTime.UtcNow;

        _authorRepository.Update(author);
        await _authorRepository.SaveChangesAsync(ct);

        await _activityLogService.LogAsync(
            _currentUser.UserId,
            "UpdateAuthor",
            nameof(Author),
            author.Id,
            $"Author '{author.FullName}' updated.",
            ct: ct);
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        var author = await _authorRepository.GetByIdAsync(id, ct)
            ?? throw new KeyNotFoundException("Author not found.");

        if (author.DeletedAt is not null)
            throw new KeyNotFoundException("Author not found.");

        if (await _authorRepository.HasActiveBooksAsync(id, ct))
            throw new InvalidOperationException("Cannot delete an author who has active books.");

        // Soft delete
        author.DeletedAt = DateTime.UtcNow;
        author.ModifiedAt = DateTime.UtcNow;

        _authorRepository.Update(author);
        await _authorRepository.SaveChangesAsync(ct);

        await _activityLogService.LogAsync(
            _currentUser.UserId,
            "DeleteAuthor",
            nameof(Author),
            author.Id,
            $"Author '{author.FullName}' deleted.",
            ct: ct);
    }
}
