using TimeWorkedManagementSystem.Models;

namespace TimeWorkedManagementSystem.Extensions
{
    public static class ShiftExtensions
    {
        public static void SortBreaks(this Shift shift)
        {
            shift.Breaks.Sort(new Comparison<Break>((Break b1, Break b2) => b1.Start.CompareTo(b2.Start)));
        }
    }
}
