namespace OracleSampleProject.Models
{
    public class Enrollment
    {
        public int EnrollmentId { get; set; }
        public int StudentId { get; set; }
        public int CourseId { get; set; }

        // Navigation properties
        public StudentModel Student { get; set; }
        public CourseModel Course { get; set; }
    }

}
