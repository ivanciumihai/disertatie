namespace personal_pages
{
    using System;
    using System.Collections.Generic;
    
    public partial class Notification
    {
        public System.Guid NotificationId { get; set; }

        public System.Guid FromUser { get; set; }

        public string Message { get; set; }

        public System.DateTime Date { get; set; }

        public Nullable<System.Guid> Batchid { get; set; }

        public Nullable<System.Guid> ToUser { get; set; }

        public Nullable<int> GroupNumber { get; set; }

        public Nullable<System.Guid> FacultyId { get; set; }

        public Nullable<System.Guid> UniversityId { get; set; }
    
        public virtual University University { get; set; }
    }
}
