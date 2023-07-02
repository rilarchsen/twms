namespace TimeWorkedManagementSystem.Models;

public abstract class OwnedEntityBase
{
    public Guid UserId { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
    public DateTimeOffset? ModifiedOn { get; set; }
}