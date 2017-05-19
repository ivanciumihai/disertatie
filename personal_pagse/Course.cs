using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace personal_pages
{
    public class Course
    {
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Course()
        {
            ClassBooks = new HashSet<ClassBook>();
        }

        public Guid CourseId { get; set; }

        [Required(ErrorMessage = "Course Name cannot be empty")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Credits cannot be empty")]
        [Range(0, 100, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public short Credits { get; set; }

        [Required(ErrorMessage = "Department field cannot be empty")]
        public Guid DepartamentId { get; set; }

        [Required(ErrorMessage = "Teacher field cannot be empty")]
        public string TeacherId { get; set; }

        [Required(ErrorMessage = "Exam date cannot be empty")]
        public DateTime? ExamDate { get; set; }

        [Required(ErrorMessage = "Exam date cannot be empty")]
        public DateTime? CourseDate { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ClassBook> ClassBooks { get; set; }

        public virtual Departament Departament { get; set; }

        public virtual User User { get; set; }
    }
}