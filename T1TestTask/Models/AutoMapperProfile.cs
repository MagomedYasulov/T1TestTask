using AutoMapper;
using T1TestTask.Data.Entites;
using T1TestTask.ViewModels.Response;

namespace T1TestTask.Models
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Course, CourseDto>();
            CreateMap<Student, StudentDto>();
        }
    }
}
