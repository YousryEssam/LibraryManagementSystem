namespace Library.Application.Interfaces
{
    public interface IMemberService
    {
        Task<IEnumerable<MemberDto>> GetAllAsync(CancellationToken ct = default);

        Task<MemberDetailsDto> GetByIdAsync(int id, CancellationToken ct = default);

        Task<int> CreateAsync(CreateMemberDto request, CancellationToken ct = default);

        Task UpdateAsync(int id, UpdateMemberDto request, CancellationToken ct = default);

        Task DeleteAsync(int id, CancellationToken ct = default);
    }
}
