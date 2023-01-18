using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using TimeWorkedManagementSystem.Attributes;
using TimeWorkedManagementSystem.Interfaces;
using System.ComponentModel;

namespace TimeWorkedManagementSystem.Models
{
    [TimeSpanValid]
    [BreaksWithinBounds]
    [BreaksDoNotOverlap]
    public class Shift : ITimeSpanned
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public List<Break> Breaks { get; set; } = new List<Break>();
        public Guid UserId { get; set; }
        public Guid CompanyId { get; set; }
        [ForeignKey(nameof(CompanyId))]
        public Company? Company { get; set; }

        [NotMapped]
        [DisplayName("Total shift time")]
        public TimeSpan TotalTime => End - Start;
        
        [NotMapped]
        [DisplayName("Total work time")]
        public TimeSpan WorkTime => End - Start - TimeSpan.FromTicks(Breaks.Select(b => b.End - b.Start).Sum(t => t.Ticks));

        [NotMapped]
        [DisplayName("Total break time")]
        public TimeSpan BreakTime => TimeSpan.FromTicks(Breaks.Select(b => b.End - b.Start).Sum(t => t.Ticks));
    }
}
