namespace personal_pages
{
    using System;
    using System.Collections.Generic;
    
    public partial class Request
    {
        public System.Guid RequestId { get; set; }

        public string Message { get; set; }

        public string RequestBy { get; set; }

        public System.DateTime RequestDate { get; set; }
    
        public virtual User User { get; set; }
    }
}
