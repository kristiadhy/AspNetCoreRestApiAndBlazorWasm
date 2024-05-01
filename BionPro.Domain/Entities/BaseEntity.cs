using System.ComponentModel.DataAnnotations;

namespace Domain;

public abstract class BaseEntity
{
    public DateTimeOffset DateCreated { get; set; } = DateTime.Now;
    [MaxLength(50)]
    public string CreatedBy { get; set; } = string.Empty;
    public DateTimeOffset? DateUpdated { get; set; }
    [MaxLength(50)]
    public string? UpdatedBy { get; set; } = string.Empty;
    public DateTimeOffset? DateDeleted { get; set; }
    [MaxLength(50)]
    public string? DeletedBy { get; set; } = string.Empty;
}