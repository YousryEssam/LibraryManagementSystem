namespace Library.Infrastructure.Persistence.Repositories
{
    public sealed class MemberRepository : Repository<Member>, IMemberRepository
    {
        private readonly LibraryDbContext _context;

        public MemberRepository(LibraryDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Member?> GetByEmailAsync(
            string email,
            CancellationToken cancellationToken = default)
        {
            return await _context.Members
                .FirstOrDefaultAsync(
                    m => m.Email == email && m.DeletedAt == null,
                    cancellationToken);
        }

        public async Task<bool> ExistsByEmailAsync(
            string email,
            CancellationToken cancellationToken = default)
        {
            return await _context.Members
                .AnyAsync(
                    m => m.Email == email && m.DeletedAt == null,
                    cancellationToken);
        }

        public async Task<bool> ExistsByEmailAsync(
            string email,
            int excludeMemberId,
            CancellationToken cancellationToken = default)
        {
            return await _context.Members
                .AnyAsync(
                    m => m.Email == email
                         && m.Id != excludeMemberId
                         && m.DeletedAt == null,
                    cancellationToken);
        }
    }
}
