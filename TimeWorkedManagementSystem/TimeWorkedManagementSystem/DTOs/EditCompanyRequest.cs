namespace TimeWorkedManagementSystem.DTOs;

public class EditCompanyRequest
{
    public Guid CompanyId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
}