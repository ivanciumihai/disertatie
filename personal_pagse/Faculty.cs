using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace personal_pages
{
    public class Faculty
    {
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Faculty()
        {
            Users = new HashSet<User>();
        }

        public Guid FacultyId { get; set; }

        [Required(ErrorMessage = "name cannot be empty")]
        public string Name { get; set; }

        public Guid UniversityId { get; set; }

        public virtual University University { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<User> Users { get; set; }
    }
}