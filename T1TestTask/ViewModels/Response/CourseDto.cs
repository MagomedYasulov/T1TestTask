using T1TestTask.Data.Entites;

namespace T1TestTask.ViewModels.Response
{
    public class CourseDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public List<StudentDto> Students { get; set; } = [];
    }
}
