namespace personal_pages
{
    using System.ComponentModel.DataAnnotations;
    using System.Collections.Generic;
    
    public partial class Education_Form
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Education_Form()
        {
            this.Users = new HashSet<User>();
        }
    
        public System.Guid id { get; set; }

        [Required(ErrorMessage = "Name cannot be empty")]
        public string name { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<User> Users { get; set; }
    }
}
