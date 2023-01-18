using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TimeWorkedManagementSystem.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public List<Company> Companies { get; set; } = new List<Company>();
        public List<Shift> Shifts { get; set; } = new List<Shift>();
    }
}
