using System;

namespace personal_pages
{
    public class Schedule
    {
        public Guid ScheduleId { get; set; }

        public Guid CourseId { get; set; }

        public string TeacherId { get; set; }

        public DateTime StartDate { get; set; }

        public string Class_Nr { get; set; }

        public virtual Course Course { get; set; }

        public virtual User User { get; set; }
    }
}