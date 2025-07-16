using Microsoft.AspNetCore.Mvc;
using T1TestTask.Abstractions;
using T1TestTask.ViewModels.Request;
using T1TestTask.ViewModels.Response;

namespace T1TestTask.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CoursesController : BaseController
    {
        private readonly ICoursesService _coursesService;

        public CoursesController(ICoursesService coursesService)
        {
            _coursesService = coursesService;
        }

        /// <summary>
        /// Создание курса
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<CourseDto>> Create(CreateCourseDto model)
        {
            var courseDto = await _coursesService.Create(model);
            return Ok(courseDto);
        }

        /// <summary>
        /// Получение курса
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<CourseDto>> Get(Guid id)
        {
            var courseDto = await _coursesService.Get(id);
            return Ok(courseDto);
        }

        /// <summary>
        /// Получение курсов
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<CourseDto[]>> Get()
        {
            var coursesDto = await _coursesService.Get();
            return Ok(coursesDto);
        }

        /// <summary>
        /// Обновление курса
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<CourseDto>> Create(Guid id, UpdateCourseDto model)
        {
            var courseDto = await _coursesService.Update(id, model);
            return Ok(courseDto);
        }

        /// <summary>
        /// Добавление студента
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("{id:guid}/students")]
        public async Task<ActionResult<StudentDto>> AddStudents(Guid id, CreateStudentDto model)
        {
            var studentDto = await _coursesService.AddStudent(id, model);
            return Ok(studentDto);
        }

        /// <summary>
        /// Удаление курса
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _coursesService.Delete(id);
            return Ok();
        }
    }
}
