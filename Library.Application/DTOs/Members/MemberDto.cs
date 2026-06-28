using Library.Domain.Enums;

namespace Library.Application.DTOs.Members;

public class MemberDto
{
    public int Id { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string? Phone { get; set; }

    public MemberStatus Status { get; set; }
}
