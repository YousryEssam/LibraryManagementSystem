namespace Library.Application.DTOs.Members;

public sealed class MemberDetailsDto : MemberDto
{
    public string? Address { get; set; }

    public DateTime MembershipDate { get; set; }

    public DateTime? MembershipExpiryDate { get; set; }
}
