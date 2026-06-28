using Library.Domain.Enums;

namespace Library.Application.DTOs.Members;

public sealed class UpdateMemberDto
{
    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string? Phone { get; set; }

    public string? Address { get; set; }

    public MemberStatus Status { get; set; }

    public DateTime? MembershipExpiryDate { get; set; }
}
