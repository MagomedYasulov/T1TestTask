using T1TestTask.Data.Entites;

namespace T1TestTask.ViewModels.Response
{
    public class StudentDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = string.Empty;

        public Guid CourseId { get; set; }
        public CourseDto Course { get; set; }
    }
}
