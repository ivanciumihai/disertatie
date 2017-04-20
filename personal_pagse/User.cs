using System.ComponentModel.DataAnnotations;

namespace personal_pages
{
    using Helpers;
    using System;
    using System.Collections.Generic;
    
    public partial class User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            this.ClassBooks = new HashSet<ClassBook>();
            this.Courses = new HashSet<Course>();
            this.Requests = new HashSet<Request>();
            this.Schedules = new HashSet<Schedule>();
        }

        public string FullName => StringHelper.CutWhiteSpace(
            $"{FirstName}{StringHelper.SubStringCapital(FatherName)}{LastName}");
        public string UserId { get; set; }

        [Required(ErrorMessage = "First Name cannot be empty")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name cannot be empty")]
        public string LastName { get; set; }

        [DataType(DataType.Date)]
        public System.DateTime Reg_date { get; set; }
        public Guid? Ed_Form { get; set; }

        [Required(ErrorMessage = "Father name cannot be empty")]
        public string FatherName { get; set; }
        public Guid? DepID { get; set; }

        [Range(0, 9999, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public int? GroupNumber { get; set; }
        public System.Guid FacultyId { get; set; }
        public System.Guid UniversityId { get; set; }
        public string RoleId { get; set; }
    
        public virtual AspNetRole AspNetRole { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ClassBook> ClassBooks { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Course> Courses { get; set; }
        public virtual Departament Departament { get; set; }
        public virtual Education_Form Education_Form { get; set; }
        public virtual Faculty Faculty { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Request> Requests { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Schedule> Schedules { get; set; }
        public virtual AspNetUser AspNetUser { get; set; }
    }
}
