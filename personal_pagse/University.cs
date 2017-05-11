namespace personal_pages
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;


    public partial class University
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public University()
        {
            this.Faculties = new HashSet<Faculty>();
            this.Notifications = new HashSet<Notification>();
        }
    
        public System.Guid UniversityId { get; set; }

        [Required(ErrorMessage = "name cannot be empty")]
        public string Name { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Faculty> Faculties { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Notification> Notifications { get; set; }
    }
}
