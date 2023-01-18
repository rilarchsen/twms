using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TimeWorkedManagementSystem.Models
{
    public class Company
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Email { get; set; }
        public Guid UserId { get; set; }
        public List<Shift> Shifts { get; set; } = new List<Shift>();
    }
}
