namespace TimeWorkedManagementSystem.DTOs;

public class UpdateShiftRequest
{
    public DateTimeOffset Start { get; set; }
    public DateTimeOffset End { get; set; }
    public List<CreateBreakRequest>? Breaks { get; set; }
}