namespace TimeWorkedManagementSystem.DTOs;

public class CreateBreakRequest
{
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public Guid ShiftId { get; set; }
}