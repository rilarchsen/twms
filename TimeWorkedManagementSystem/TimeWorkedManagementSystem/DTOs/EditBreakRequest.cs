namespace TimeWorkedManagementSystem.DTOs;

public class EditBreakRequest
{
    public Guid Id { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
}