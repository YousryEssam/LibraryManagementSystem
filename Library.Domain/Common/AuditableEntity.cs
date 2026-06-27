namespace Library.Domain.Common;

public abstract class AuditableEntity : BaseEntity
{
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? ModifiedAt { get; set; }

    public DateTime? DeletedAt { get; set; }
}
