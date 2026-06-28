namespace Library.Infrastructure.Services;

public sealed class MemberService : IMemberService
{
    private readonly ICurrentUserService _currentUser;
    private readonly IMemberRepository _memberRepository;
    private readonly IActivityLogService _activityLogService;

    public MemberService(
        IMemberRepository memberRepository,
        IActivityLogService activityLogService,
        ICurrentUserService currentUser)
    {
        _memberRepository = memberRepository;
        _activityLogService = activityLogService;
        _currentUser = currentUser;
    }

    public async Task<IEnumerable<MemberDto>> GetAllAsync(CancellationToken ct = default)
    {
        var members = await _memberRepository.GetAllAsync(ct);

        return members.Select(member => new MemberDto
        {
            Id = member.Id,
            FullName = member.FullName,
            Email = member.Email,
            Phone = member.Phone,
            Status = member.Status
        });
    }

    public async Task<MemberDetailsDto> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var member = await _memberRepository.GetByIdAsync(id, ct);

        if (member is null)
            throw new KeyNotFoundException("Member not found.");

        return new MemberDetailsDto
        {
            Id = member.Id,
            FullName = member.FullName,
            Email = member.Email,
            Phone = member.Phone,
            Address = member.Address,
            MembershipDate = member.MembershipDate,
            MembershipExpiryDate = member.MembershipExpiryDate,
            Status = member.Status
        };
    }

    public async Task<int> CreateAsync(CreateMemberDto request, CancellationToken ct = default)
    {
        var existingMember = await _memberRepository.GetByEmailAsync(request.Email, ct);

        if (existingMember is not null)
            throw new InvalidOperationException("A member with this email already exists.");

        var member = new Member
        {
            FullName = request.FullName,
            Email = request.Email,
            Phone = request.Phone,
            Address = request.Address,
            MembershipDate = DateTime.UtcNow,
            MembershipExpiryDate = request.MembershipExpiryDate,
            Status = MemberStatus.Active
        };

        await _memberRepository.AddAsync(member, ct);
        await _memberRepository.SaveChangesAsync(ct);
        var userId = _currentUser.UserId;

        await _activityLogService.LogAsync(
            userId,
            "CreateMember",
            nameof(Member),
            member.Id,
            $"Member '{member.FullName}' created.",
            null,
            ct);

        return member.Id;
    }

    public async Task UpdateAsync(int id, UpdateMemberDto request, CancellationToken ct = default)
    {
        var member = await _memberRepository.GetByIdAsync(id, ct);

        if (member is null)
            throw new KeyNotFoundException("Member not found.");

        var existingMember = await _memberRepository.GetByEmailAsync(request.Email, ct);

        if (existingMember is not null && existingMember.Id != id)
            throw new InvalidOperationException("A member with this email already exists.");

        member.FullName = request.FullName;
        member.Email = request.Email;
        member.Phone = request.Phone;
        member.Address = request.Address;
        member.Status = request.Status;
        member.MembershipExpiryDate = request.MembershipExpiryDate;
        member.ModifiedAt = DateTime.UtcNow;

        _memberRepository.Update(member);

        await _memberRepository.SaveChangesAsync(ct);
        var userId = _currentUser.UserId;

        await _activityLogService.LogAsync(
            userId,
            "UpdateMember",
            nameof(Member),
            member.Id,
            $"Member '{member.FullName}' updated.",
            null,
            ct);
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        var member = await _memberRepository.GetByIdAsync(id, ct);

        if (member is null)
            throw new KeyNotFoundException("Member not found.");

        // Soft Delete
        member.DeletedAt = DateTime.UtcNow;
        member.ModifiedAt = DateTime.UtcNow;

        _memberRepository.Update(member);

        await _memberRepository.SaveChangesAsync(ct);
        var userId = _currentUser.UserId;

        await _activityLogService.LogAsync(
            userId,
            "DeleteMember",
            nameof(Member),
            member.Id,
            $"Member '{member.FullName}' deleted.",
            null,
            ct);
    }
}