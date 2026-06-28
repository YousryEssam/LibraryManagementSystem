using Library.Application.DTOs.Borrowing;

namespace Library.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public sealed class BorrowingController : ControllerBase
{
    private readonly IBorrowingTransactionService _borrowingService;

    public BorrowingController(IBorrowingTransactionService borrowingService)
        => _borrowingService = borrowingService;

    /// <summary>Get all transactions. Supports optional filters: status, memberId, bookId.</summary>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<BorrowingTransactionDto>>>> GetAll(
        [FromQuery] BorrowingFilterQuery filter, CancellationToken ct)
    {
        var transactions = await _borrowingService.GetAllAsync(filter, ct);
        return Ok(ApiResponse<IEnumerable<BorrowingTransactionDto>>.Ok(transactions, "Transactions retrieved successfully."));
    }

    /// <summary>Get all active transactions past their due date.</summary>
    [HttpGet("overdue")]
    public async Task<ActionResult<ApiResponse<IEnumerable<BorrowingTransactionDto>>>> GetOverdue(CancellationToken ct)
    {
        var transactions = await _borrowingService.GetOverdueAsync(ct);
        return Ok(ApiResponse<IEnumerable<BorrowingTransactionDto>>.Ok(transactions, "Overdue transactions retrieved successfully."));
    }

    /// <summary>Get full borrowing history for a specific member.</summary>
    [HttpGet("member/{memberId:int}")]
    public async Task<ActionResult<ApiResponse<IEnumerable<BorrowingTransactionDto>>>> GetByMember(
        int memberId, CancellationToken ct)
    {
        var transactions = await _borrowingService.GetByMemberAsync(memberId, ct);
        return Ok(ApiResponse<IEnumerable<BorrowingTransactionDto>>.Ok(transactions, "Member transactions retrieved successfully."));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApiResponse<BorrowingTransactionDetailsDto>>> GetById(
        int id, CancellationToken ct)
    {
        var transaction = await _borrowingService.GetByIdAsync(id, ct);
        return Ok(ApiResponse<BorrowingTransactionDetailsDto>.Ok(transaction));
    }

    /// <summary>Borrow a book for a member.</summary>
    [HttpPost]
    [Authorize(Roles = "Librarian,Administrator")]
    public async Task<ActionResult<ApiResponse<object>>> Borrow(
        [FromBody] BorrowBookDto request, CancellationToken ct)
    {
        var id = await _borrowingService.BorrowAsync(request, ct);
        return CreatedAtAction(
            nameof(GetById), new { id },
            ApiResponse<object>.Ok(new { id }, "Book borrowed successfully."));
    }

    /// <summary>Mark a borrowed book as returned.</summary>
    [HttpPatch("{id:int}/return")]
    [Authorize(Roles = "Librarian,Administrator")]
    public async Task<ActionResult<ApiResponse>> Return(
        int id, [FromBody] ReturnBookDto request, CancellationToken ct)
    {
        await _borrowingService.ReturnAsync(id, request, ct);
        return Ok(ApiResponse.Ok("Book returned successfully."));
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Administrator")]
    public async Task<ActionResult<ApiResponse>> Delete(int id, CancellationToken ct)
    {
        await _borrowingService.DeleteAsync(id, ct);
        return Ok(ApiResponse.Ok("Transaction deleted successfully."));
    }
}
