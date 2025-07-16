using AutoMapper;
using Microsoft.EntityFrameworkCore;
using T1TestTask.Abstractions;
using T1TestTask.Data;
using T1TestTask.Data.Entites;
using T1TestTask.Exceptions;
using T1TestTask.ViewModels.Request;
using T1TestTask.ViewModels.Response;

namespace T1TestTask.Services
{
    public class CourseService : ICoursesService
    {
        private readonly IMapper _mapper;
        private readonly ApplicationContext _dbContext;

        public CourseService(
            IMapper mapper,
            ApplicationContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<CourseDto> Create(CreateCourseDto model)
        {
            var course = new Course()
            {
                Name = model.Name
            };

            await _dbContext.Courses.AddAsync(course);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<CourseDto>(course);
        }


        public async Task<CourseDto[]> Get()
        {
            var courses = await _dbContext.Courses.AsNoTracking().Include(p => p.Students).ToArrayAsync();
            return _mapper.Map<CourseDto[]>(courses);
        }

        public async Task<CourseDto> Get(Guid id)
        {
            var course = await _dbContext.Courses.AsNoTracking().Include(c => c.Students).FirstOrDefaultAsync(c => c.Id == id);
            if (course == null)
                throw new ServiceException("Course Not Found", $"Course with id {id} not found.", StatusCodes.Status404NotFound);

            return _mapper.Map<CourseDto>(course);
        }

        public async Task<StudentDto> AddStudent(Guid courseId, CreateStudentDto model)
        {
            if(!await _dbContext.Courses.AnyAsync(c => c.Id == courseId))
                throw new ServiceException("Course Not Found", $"Course with id {courseId} not found.", StatusCodes.Status404NotFound);

            var student = new Student()
            {
                CourseId = courseId,
                FullName = model.FullName
            };

            await _dbContext.Students.AddAsync(student);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<StudentDto>(student);
        }

        public async Task<CourseDto> Update(Guid id, UpdateCourseDto model)
        {
            var course = await _dbContext.Courses.Include(c => c.Students).FirstOrDefaultAsync(c => c.Id == id);
            if (course == null)
                throw new ServiceException("Course Not Found", $"Course with id {id} not found.", StatusCodes.Status404NotFound);

            course.Name = model.Name;
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<CourseDto>(course);
        }

        public async Task Delete(Guid id)
        {
            var course = await _dbContext.Courses.FirstOrDefaultAsync(m => m.Id == id);
            if (course == null)
                throw new ServiceException("Course Not Found", $"Course with id {id} not found.", StatusCodes.Status404NotFound);

            _dbContext.Courses.Remove(course);
            await _dbContext.SaveChangesAsync();
        }
    }
}
