namespace T1TestTask.Data.Entites
{
    public class Course : BaseEntity
    {
        public string Name { get; set; } = string.Empty;

        public List<Student> Students { get; set; } = [];
    }
}
