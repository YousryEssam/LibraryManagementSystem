namespace Library.Infrastructure.Services;

public sealed class BookService : IBookService
{
    private readonly IBookRepository _bookRepository;
    private readonly IBookAuthorRepository _bookAuthorRepository;
    private readonly IPublisherRepository _publisherRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IAuthorRepository _authorRepository;
    private readonly IBorrowingTransactionRepository _transactionRepository;
    private readonly IActivityLogService _activityLogService;
    private readonly ICurrentUserService _currentUser;

    public BookService(
        IBookRepository bookRepository,
        IBookAuthorRepository bookAuthorRepository,
        IPublisherRepository publisherRepository,
        ICategoryRepository categoryRepository,
        IAuthorRepository authorRepository,
        IBorrowingTransactionRepository transactionRepository,
        IActivityLogService activityLogService,
        ICurrentUserService currentUser)
    {
        _bookRepository = bookRepository;
        _bookAuthorRepository = bookAuthorRepository;
        _publisherRepository = publisherRepository;
        _categoryRepository = categoryRepository;
        _authorRepository = authorRepository;
        _transactionRepository = transactionRepository;
        _activityLogService = activityLogService;
        _currentUser = currentUser;
    }

    public async Task<IEnumerable<BookDto>> GetAllAsync(BookFilterQuery filter, CancellationToken ct = default)
    {
        var query = _bookRepository.QueryWithDetails()
            .Where(b => b.DeletedAt == null);

        if (!string.IsNullOrWhiteSpace(filter.Title))
            query = query.Where(b => b.Title.Contains(filter.Title));

        if (!string.IsNullOrWhiteSpace(filter.AuthorName))
            query = query.Where(b => b.BookAuthors
                .Any(ba => ba.Author.FullName.Contains(filter.AuthorName)));

        if (!string.IsNullOrWhiteSpace(filter.Category))
            query = query.Where(b => b.Category.Name.Contains(filter.Category));

        if (filter.PublisherId.HasValue)
            query = query.Where(b => b.PublisherId == filter.PublisherId.Value);

        if (filter.Status.HasValue)
            query = query.Where(b => b.Status == filter.Status.Value);

        return await query
            .OrderBy(b => b.Title)
            .Select(b => MapToDto(b))
            .ToListAsync(ct);
    }

    public async Task<BookDetailsDto> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var book = await _bookRepository.GetWithDetailsAsync(id, ct)
            ?? throw new KeyNotFoundException("Book not found.");

        return MapToDetailsDto(book);
    }

    public async Task<int> CreateAsync(CreateBookDto request, CancellationToken ct = default)
    {
        if (await _bookRepository.ExistsByIsbnAsync(request.ISBN, ct: ct))
            throw new InvalidOperationException($"A book with ISBN '{request.ISBN}' already exists.");

        await ValidateForeignKeysAsync(request.PublisherId, request.CategoryId, ct);

        var authorIds = request.Authors.Select(a => a.AuthorId).Distinct().ToList();
        await ValidateAuthorsExistAsync(authorIds, ct);

        var book = new Book
        {
            Title = request.Title,
            ISBN = request.ISBN,
            Edition = request.Edition,
            PublicationYear = request.PublicationYear,
            Language = request.Language,
            Summary = request.Summary,
            CoverImageUrl = request.CoverImageUrl,
            Status = BookStatus.Available,
            PublisherId = request.PublisherId,
            CategoryId = request.CategoryId,
            CreatedAt = DateTime.UtcNow
        };

        await _bookRepository.AddAsync(book, ct);
        await _bookRepository.SaveChangesAsync(ct);

        var bookAuthors = request.Authors.Select(a => new BookAuthor
        {
            BookId = book.Id,
            AuthorId = a.AuthorId,
            Role = a.Role
        });

        await _bookAuthorRepository.AddRangeAsync(bookAuthors, ct);
        await _bookAuthorRepository.SaveChangesAsync(ct);

        await _activityLogService.LogAsync(
            _currentUser.UserId, "CreateBook", nameof(Book),
            book.Id, $"Book '{book.Title}' created.", ct: ct);

        return book.Id;
    }

    public async Task UpdateAsync(int id, UpdateBookDto request, CancellationToken ct = default)
    {
        var book = await _bookRepository.GetWithDetailsAsync(id, ct)
            ?? throw new KeyNotFoundException("Book not found.");

        if (await _bookRepository.ExistsByIsbnAsync(request.ISBN, excludeId: id, ct: ct))
            throw new InvalidOperationException($"A book with ISBN '{request.ISBN}' already exists.");

        await ValidateForeignKeysAsync(request.PublisherId, request.CategoryId, ct);

        var authorIds = request.Authors.Select(a => a.AuthorId).Distinct().ToList();
        await ValidateAuthorsExistAsync(authorIds, ct);

        book.Title = request.Title;
        book.ISBN = request.ISBN;
        book.Edition = request.Edition;
        book.PublicationYear = request.PublicationYear;
        book.Language = request.Language;
        book.Summary = request.Summary;
        book.CoverImageUrl = request.CoverImageUrl;
        book.PublisherId = request.PublisherId;
        book.CategoryId = request.CategoryId;
        book.ModifiedAt = DateTime.UtcNow;

        _bookRepository.Update(book);

        // Replace authors — remove all then re-add
        _bookAuthorRepository.DeleteRange(book.BookAuthors);

        var updatedAuthors = request.Authors.Select(a => new BookAuthor
        {
            BookId = book.Id,
            AuthorId = a.AuthorId,
            Role = a.Role
        });

        await _bookAuthorRepository.AddRangeAsync(updatedAuthors, ct);
        await _bookAuthorRepository.SaveChangesAsync(ct);

        await _activityLogService.LogAsync(
            _currentUser.UserId, "UpdateBook", nameof(Book),
            book.Id, $"Book '{book.Title}' updated.", ct: ct);
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        var book = await _bookRepository.GetActiveByIdAsync(id, ct)
            ?? throw new KeyNotFoundException("Book not found.");

        if (book.Status == BookStatus.Borrowed)
            throw new InvalidOperationException("Cannot delete a book that is currently borrowed.");

        if (await _transactionRepository.HasActiveTransactionAsync(id, ct))
            throw new InvalidOperationException("Cannot delete a book with active transactions.");

        book.DeletedAt = DateTime.UtcNow;
        book.ModifiedAt = DateTime.UtcNow;

        _bookRepository.Update(book);
        await _bookRepository.SaveChangesAsync(ct);

        await _activityLogService.LogAsync(
            _currentUser.UserId, "DeleteBook", nameof(Book),
            book.Id, $"Book '{book.Title}' deleted.", ct: ct);
    }

    // ── Helpers ────────────────────────────────────────────────────────────────

    private async Task ValidateForeignKeysAsync(int publisherId, int categoryId, CancellationToken ct)
    {
        var publisher = await _publisherRepository.Query()
            .AnyAsync(p => p.Id == publisherId && p.DeletedAt == null, ct);
        if (!publisher)
            throw new KeyNotFoundException($"Publisher with id {publisherId} not found.");

        var category = await _categoryRepository.Query()
            .AnyAsync(c => c.Id == categoryId && c.DeletedAt == null, ct);
        if (!category)
            throw new KeyNotFoundException($"Category with id {categoryId} not found.");
    }

    private async Task ValidateAuthorsExistAsync(IEnumerable<int> authorIds, CancellationToken ct)
    {
        foreach (var authorId in authorIds)
        {
            var exists = await _authorRepository.Query()
                .AnyAsync(a => a.Id == authorId && a.DeletedAt == null, ct);
            if (!exists)
                throw new KeyNotFoundException($"Author with id {authorId} not found.");
        }
    }

    private static BookDto MapToDto(Book b) => new()
    {
        Id = b.Id,
        Title = b.Title,
        ISBN = b.ISBN,
        Edition = b.Edition,
        PublicationYear = b.PublicationYear,
        Language = b.Language,
        CategoryName = b.Category.Name,
        PublisherName = b.Publisher.Name,
        Status = b.Status,
        Authors = b.BookAuthors.Select(ba => new BookAuthorDto
        {
            AuthorId = ba.AuthorId,
            FullName = ba.Author.FullName,
            Role = ba.Role
        })
    };

    private static BookDetailsDto MapToDetailsDto(Book b) => new()
    {
        Id = b.Id,
        Title = b.Title,
        ISBN = b.ISBN,
        Edition = b.Edition,
        PublicationYear = b.PublicationYear,
        Language = b.Language,
        Summary = b.Summary,
        CoverImageUrl = b.CoverImageUrl,
        Status = b.Status,
        CategoryId = b.CategoryId,
        CategoryName = b.Category.Name,
        PublisherId = b.PublisherId,
        PublisherName = b.Publisher.Name,
        Authors = b.BookAuthors.Select(ba => new BookAuthorDto
        {
            AuthorId = ba.AuthorId,
            FullName = ba.Author.FullName,
            Role = ba.Role
        })
    };
}
