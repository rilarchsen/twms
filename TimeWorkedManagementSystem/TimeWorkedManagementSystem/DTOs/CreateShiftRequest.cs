namespace TimeWorkedManagementSystem.DTOs;

public class CreateShiftRequest
{
    public DateTimeOffset Start { get; set; }
    public DateTimeOffset End { get; set; }
    public Guid CompanyId { get; set; }
    public List<CreateBreakRequest>? Breaks { get; set; }
}