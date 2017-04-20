namespace personal_pages
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Course
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Course()
        {
            this.ClassBooks = new HashSet<ClassBook>();
            this.Schedules = new HashSet<Schedule>();
        }
    
        public Guid CourseId { get; set; }

        [Required(ErrorMessage = "Course Name cannot be empty")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Credits cannot be empty")]
        [Range(0, 100, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public short Credits { get; set; }

        public Guid DepartamentId { get; set; }

        public string TeacherId { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ClassBook> ClassBooks { get; set; }

        public virtual Departament Departament { get; set; }

        public virtual User User { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Schedule> Schedules { get; set; }
    }
}
