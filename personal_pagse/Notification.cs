using System;

namespace personal_pages
{
    public class Notification
    {
        public Guid NotificationId { get; set; }

        public Guid FromUser { get; set; }

        public string Message { get; set; }

        public DateTime Date { get; set; }

        public Guid? Batchid { get; set; }

        public Guid? ToUser { get; set; }

        public int? GroupNumber { get; set; }

        public Guid? FacultyId { get; set; }

        public Guid? UniversityId { get; set; }

        public virtual University University { get; set; }
    }
}