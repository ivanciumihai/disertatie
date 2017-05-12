using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using personal_pages.Helpers;

namespace personal_pages
{
    public class User
    {
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            ClassBooks = new HashSet<ClassBook>();
            Courses = new HashSet<Course>();
        }

        public string FullName => StringHelper.CutWhiteSpace(
            $"{FirstName}{StringHelper.SubStringCapital(FatherName)}{LastName}");

        public string UserId { get; set; }

        [Required(ErrorMessage = "First Name cannot be empty")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name cannot be empty")]
        public string LastName { get; set; }

        [DataType(DataType.Date)]
        public DateTime Reg_date { get; set; }

        public Guid? Ed_Form { get; set; }

        [Required(ErrorMessage = "Father name cannot be empty")]
        public string FatherName { get; set; }

        public Guid? DepID { get; set; }

        [Range(0, 9999, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public int? GroupNumber { get; set; }

        public Guid FacultyId { get; set; }

        public Guid UniversityId { get; set; }

        public string RoleId { get; set; }

        public string ImagePath { get; set; }

        public virtual AspNetRole AspNetRole { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ClassBook> ClassBooks { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Course> Courses { get; set; }

        public virtual Departament Departament { get; set; }

        public virtual Education_Form Education_Form { get; set; }

        public virtual Faculty Faculty { get; set; }

        public virtual AspNetUser AspNetUser { get; set; }
    }
}