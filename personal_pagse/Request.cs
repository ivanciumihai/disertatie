using System;

namespace personal_pages
{
    public class Request
    {
        public Guid RequestId { get; set; }

        public string Message { get; set; }

        public string RequestBy { get; set; }

        public DateTime RequestDate { get; set; }

        public virtual User User { get; set; }
    }
}