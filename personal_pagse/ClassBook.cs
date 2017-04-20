namespace personal_pages
{
    using System;
    using System.ComponentModel.DataAnnotations;


    public partial class ClassBook
    {
        public Guid ClassBookId { get; set; }

        public Guid? CourseId { get; set; }

        public string StudentId { get; set; }

        [Required(ErrorMessage = "Grade cannot be empty")]
        [Range(1, 10, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public double? Grade { get; set; }

        public bool? Promoted { get; set; }

        public string TeacherId { get; set; }

        public DateTime? Grade_Date { get; set; }

        public DateTime? Grade_modified { get; set; }
    
        public virtual Course Course { get; set; }
        public virtual User User { get; set; }
    }
}
