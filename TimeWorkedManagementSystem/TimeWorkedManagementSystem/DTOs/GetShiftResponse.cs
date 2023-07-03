using TimeWorkedManagementSystem.Models;

namespace TimeWorkedManagementSystem.DTOs;

public class GetShiftResponse
{
    public Guid Id { get; set; }
    public DateTimeOffset Start { get; set; }
    public DateTimeOffset? End { get; set; }
    public List<Break> Breaks { get; set; }
    public Guid CompanyId { get; set; }
    public Company? Company { get; set; }
}