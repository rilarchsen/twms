using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TimeWorkedManagementSystem.Attributes;
using TimeWorkedManagementSystem.Interfaces;

namespace TimeWorkedManagementSystem.Models
{
    [TimeSpanValid]
    public class Break : ITimeSpanned
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public Guid UserId { get; set; }
        public Guid ShiftId { get; set; }
    }
}
