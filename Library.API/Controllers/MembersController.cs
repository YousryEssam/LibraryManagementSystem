namespace Library.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MembersController : ControllerBase
    {
        private readonly IMemberService _memberService;

        public MembersController(IMemberService memberService)
        {
            _memberService = memberService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<MemberDto>>>> GetAll(CancellationToken ct)
        {
            var members = await _memberService.GetAllAsync(ct);

            return Ok(ApiResponse<IEnumerable<MemberDto>>
                .Ok(members, "Members retrieved successfully."));
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ApiResponse<MemberDetailsDto>>> GetById(
            int id,
            CancellationToken ct)
        {
            var member = await _memberService.GetByIdAsync(id, ct);

            return Ok(ApiResponse<MemberDetailsDto>
                .Ok(member));
        }

        [HttpPost]
        [Authorize(Roles = "Librarian,Administrator")]
        public async Task<ActionResult<ApiResponse<object>>> Create(
            CreateMemberDto request,
            CancellationToken ct)
        {
            var id = await _memberService.CreateAsync(request, ct);

            return CreatedAtAction(
                nameof(GetById),
                new { id },
                ApiResponse<object>.Ok(new { id }, "Member created successfully."));
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Librarian,Administrator")]
        public async Task<ActionResult<ApiResponse>> Update(
            int id,
            UpdateMemberDto request,
            CancellationToken ct)
        {
            await _memberService.UpdateAsync(id, request, ct);

            return Ok(ApiResponse.Ok("Member updated successfully."));
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<ApiResponse>> Delete(
            int id,
            CancellationToken ct)
        {
            await _memberService.DeleteAsync(id, ct);

            return Ok(ApiResponse.Ok("Member deleted successfully."));
        }
    }
}
