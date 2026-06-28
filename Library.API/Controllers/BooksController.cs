using Library.Application.DTOs.Books;

namespace Library.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public sealed class BooksController : ControllerBase
{
    private readonly IBookService _bookService;
    private readonly IBookAuthorService _bookAuthorService;

    public BooksController(IBookService bookService, IBookAuthorService bookAuthorService)
    {
        _bookService = bookService;
        _bookAuthorService = bookAuthorService;
    }

    // ── Book CRUD ──────────────────────────────────────────────────────────────

    /// <summary>Get all books. Supports filters: title, authorName, category, publisherId, status.</summary>
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<IEnumerable<BookDto>>>> GetAll(
        [FromQuery] BookFilterQuery filter, CancellationToken ct)
    {
        var books = await _bookService.GetAllAsync(filter, ct);
        return Ok(ApiResponse<IEnumerable<BookDto>>.Ok(books, "Books retrieved successfully."));
    }

    /// <summary>Search books by title, author name, or category name (bonus endpoint).</summary>
    [HttpGet("search")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<IEnumerable<BookDto>>>> Search(
        [FromQuery] BookFilterQuery filter, CancellationToken ct)
    {
        var books = await _bookService.GetAllAsync(filter, ct);
        return Ok(ApiResponse<IEnumerable<BookDto>>.Ok(books, "Search completed successfully."));
    }

    [HttpGet("{id:int}")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<BookDetailsDto>>> GetById(int id, CancellationToken ct)
    {
        var book = await _bookService.GetByIdAsync(id, ct);
        return Ok(ApiResponse<BookDetailsDto>.Ok(book));
    }

    [HttpPost]
    [Authorize(Roles = "Librarian,Administrator")]
    public async Task<ActionResult<ApiResponse<object>>> Create(
        [FromBody] CreateBookDto request, CancellationToken ct)
    {
        var id = await _bookService.CreateAsync(request, ct);
        return CreatedAtAction(
            nameof(GetById), new { id },
            ApiResponse<object>.Ok(new { id }, "Book created successfully."));
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Librarian,Administrator")]
    public async Task<ActionResult<ApiResponse>> Update(
        int id, [FromBody] UpdateBookDto request, CancellationToken ct)
    {
        await _bookService.UpdateAsync(id, request, ct);
        return Ok(ApiResponse.Ok("Book updated successfully."));
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Administrator")]
    public async Task<ActionResult<ApiResponse>> Delete(int id, CancellationToken ct)
    {
        await _bookService.DeleteAsync(id, ct);
        return Ok(ApiResponse.Ok("Book deleted successfully."));
    }

    // ── BookAuthor Sub-Resource ────────────────────────────────────────────────

    /// <summary>Add an author to a book.</summary>
    [HttpPost("{bookId:int}/authors")]
    [Authorize(Roles = "Librarian,Administrator")]
    public async Task<ActionResult<ApiResponse>> AddAuthor(
        int bookId, [FromBody] BookAuthorEntryDto request, CancellationToken ct)
    {
        await _bookAuthorService.AddAsync(bookId, request, ct);
        return Ok(ApiResponse.Ok("Author added to book successfully."));
    }

    /// <summary>Update the role of an existing author on a book.</summary>
    [HttpPatch("{bookId:int}/authors/{authorId:int}")]
    [Authorize(Roles = "Librarian,Administrator")]
    public async Task<ActionResult<ApiResponse>> UpdateAuthorRole(
        int bookId, int authorId, [FromBody] UpdateBookAuthorRoleDto request, CancellationToken ct)
    {
        await _bookAuthorService.UpdateRoleAsync(bookId, authorId, request, ct);
        return Ok(ApiResponse.Ok("Author role updated successfully."));
    }

    /// <summary>Remove an author from a book.</summary>
    [HttpDelete("{bookId:int}/authors/{authorId:int}")]
    [Authorize(Roles = "Librarian,Administrator")]
    public async Task<ActionResult<ApiResponse>> RemoveAuthor(
        int bookId, int authorId, CancellationToken ct)
    {
        await _bookAuthorService.RemoveAsync(bookId, authorId, ct);
        return Ok(ApiResponse.Ok("Author removed from book successfully."));
    }
}
