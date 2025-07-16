using T1TestTask.ViewModels.Request;
using T1TestTask.ViewModels.Response;

namespace T1TestTask.Abstractions
{
    public interface ICoursesService
    {
        public Task<CourseDto> Create(CreateCourseDto model);
        public Task<CourseDto> Update(Guid id, UpdateCourseDto model);
        public Task<CourseDto[]> Get();
        public Task<CourseDto> Get(Guid id);
        public Task<StudentDto> AddStudent(Guid id, CreateStudentDto model);
        public Task Delete(Guid id);
    }
}
