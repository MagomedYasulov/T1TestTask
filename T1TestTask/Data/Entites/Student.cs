namespace T1TestTask.Data.Entites
{
    public class Student : BaseEntity
    {
        public string FullName { get; set; } = string.Empty;

        public Guid CourseId { get; set; }
        public Course Course { get; set; }
    }
}
