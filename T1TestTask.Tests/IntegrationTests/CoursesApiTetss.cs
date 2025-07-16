using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using T1TestTask.Data;
using T1TestTask.Tests.Extentions;
using T1TestTask.ViewModels.Request;
using T1TestTask.ViewModels.Response;

namespace T1TestTask.Tests.IntegrationTests
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Удаляем реальный контекст и заменяем InMemory
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<ApplicationContext>));
                if (descriptor != null) services.Remove(descriptor);


                services.AddDbContext<ApplicationContext>(options =>
                {
                    options.UseInMemoryDatabase("TestDb");
                });

                // Можно заполнить тестовыми данными
                using var scope = services.BuildServiceProvider().CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
                db.Database.EnsureCreated();
                db.SeedData();
            });
        }
    }

    public class CoursesApiTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly CustomWebApplicationFactory<Program> _factory;

        public CoursesApiTests(CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        #region Get API Tests

        [Fact]
        public async Task Get_Courses_ReturnsSuccess()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/api/v1/courses");

            // Assert
            response.EnsureSuccessStatusCode();
            var courses = JsonConvert.DeserializeObject<CourseDto[]>(await response.Content.ReadAsStringAsync());

            Assert.NotNull(courses);
            Assert.NotEmpty(courses);
        }

        [Fact]
        public async Task GetCourse_ExistId_ReturnsSuccess()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/api/v1/courses/f2693670-5516-4eb9-845b-803642fcb19f");

            // Assert
            response.EnsureSuccessStatusCode();
            var course = JsonConvert.DeserializeObject<CourseDto>(await response.Content.ReadAsStringAsync());

            Assert.NotNull(course);
            Assert.Equal(new Guid("f2693670-5516-4eb9-845b-803642fcb19f"), course.Id);
            Assert.NotEmpty(course.Students);
        }

        [Fact]
        public async Task GetCourse_NotExistId_ReturnsNotFound()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync($"/api/v1/courses/{Guid.NewGuid()}");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        #endregion

        #region Create API Tests

        [Fact]
        public async Task CreateCourse_ValidModel_ReturnsSuccess()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var model = new CreateCourseDto() { Name = "новый курс"};

            var json1 = JsonConvert.SerializeObject(model);

            var httpContent = new StringContent(json1, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/api/v1/courses", httpContent);

            // Assert
            response.EnsureSuccessStatusCode();
            var courseDto = JsonConvert.DeserializeObject<CourseDto>(await response.Content.ReadAsStringAsync());

            Assert.NotNull(courseDto);
            Assert.NotEqual(Guid.Empty, courseDto.Id);
            Assert.Equal(model.Name, courseDto.Name);
        }


        [Fact]
        public async Task CreateCourse_InvalidModel_ReturnsBadRequest()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var model = new CreateCourseDto() { Name = string.Empty };

            var json1 = JsonConvert.SerializeObject(model);

            var httpContent = new StringContent(json1, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/api/v1/courses", httpContent);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var problemDetails = JsonConvert.DeserializeObject<ProblemDetails>(await response.Content.ReadAsStringAsync());

            Assert.NotNull(problemDetails);
        }

        #endregion

        #region Update API Test

        [Fact]
        public async Task UpdateCourse_ValidModel_ReturnsSuccess()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var model = new UpdateCourseDto() { Name = "новый название курса" };

            var json1 = JsonConvert.SerializeObject(model);

            var httpContent = new StringContent(json1, Encoding.UTF8, "application/json");
            var response = await client.PutAsync("/api/v1/courses/9bff4b5c-91f4-4db9-b927-27ccf9b0b131", httpContent);

            // Assert
            response.EnsureSuccessStatusCode();
            var courseDto = JsonConvert.DeserializeObject<CourseDto>(await response.Content.ReadAsStringAsync());

            Assert.NotNull(courseDto);
            Assert.NotEqual(Guid.Empty, courseDto.Id);
            Assert.Equal(model.Name, courseDto.Name);
        }


        [Fact]
        public async Task UpdateCourse_InvalidModel_ReturnsBadRequest()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var model = new CreateCourseDto() { Name = string.Empty };

            var json1 = JsonConvert.SerializeObject(model);

            var httpContent = new StringContent(json1, Encoding.UTF8, "application/json");
            var response = await client.PutAsync("/api/v1/courses/9bff4b5c-91f4-4db9-b927-27ccf9b0b131", httpContent);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var problemDetails = JsonConvert.DeserializeObject<ProblemDetails>(await response.Content.ReadAsStringAsync());

            Assert.NotNull(problemDetails);
        }

        [Fact]
        public async Task UpdateCourse_NotExistCourse_ReturnsNotFound()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var model = new CreateCourseDto() { Name = "новое название" };

            var json1 = JsonConvert.SerializeObject(model);

            var httpContent = new StringContent(json1, Encoding.UTF8, "application/json");
            var response = await client.PutAsync($"/api/v1/courses/{Guid.NewGuid()}", httpContent);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            var problemDetails = JsonConvert.DeserializeObject<ProblemDetails>(await response.Content.ReadAsStringAsync());

            Assert.NotNull(problemDetails);
        }

        #endregion

        #region Delete API Test

        [Fact]
        public async Task DeleteCourse_ExistCourse_ReturnsSuccess()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.DeleteAsync("/api/v1/courses/0eab5f55-68c5-47f6-a63d-dcd25a18fe1a");

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task DeleteCourse_NotExistCourse_ReturnsNotFound()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.DeleteAsync($"/api/v1/courses/{Guid.NewGuid()}");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            var problemDetails = JsonConvert.DeserializeObject<ProblemDetails>(await response.Content.ReadAsStringAsync());

            Assert.NotNull(problemDetails);
        }

        #endregion

        #region Add Student API Tests

        [Fact]
        public async Task CreateStudent_ValidModel_ReturnsSuccess()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var model = new CreateStudentDto() { FullName = "новый студент" };

            var json1 = JsonConvert.SerializeObject(model);

            var httpContent = new StringContent(json1, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/api/v1/courses/711f59a9-dddb-4d97-9004-374cdf41e4a0/students", httpContent);

            // Assert
            response.EnsureSuccessStatusCode();
            var studentsDto = JsonConvert.DeserializeObject<StudentDto>(await response.Content.ReadAsStringAsync());

            Assert.NotNull(studentsDto);
            Assert.NotEqual(Guid.Empty, studentsDto.Id);
            Assert.Equal(model.FullName, studentsDto.FullName);
            Assert.Equal(new Guid("711f59a9-dddb-4d97-9004-374cdf41e4a0"), studentsDto.CourseId);
        }

        [Fact]
        public async Task CreateStudent_InvalidModel_ReturnsBadRequest()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var model = new CreateStudentDto() { FullName = string.Empty };

            var json1 = JsonConvert.SerializeObject(model);

            var httpContent = new StringContent(json1, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/api/v1/courses/711f59a9-dddb-4d97-9004-374cdf41e4a0/students", httpContent);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var problemDetails = JsonConvert.DeserializeObject<ProblemDetails>(await response.Content.ReadAsStringAsync());

            Assert.NotNull(problemDetails);
        }

        [Fact]
        public async Task CreateStudent_NotExistCourse_ReturnsNotFound()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var model = new CreateStudentDto() { FullName = "новый студент" };

            var json1 = JsonConvert.SerializeObject(model);

            var httpContent = new StringContent(json1, Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"/api/v1/courses/{Guid.NewGuid()}/students", httpContent);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            var problemDetails = JsonConvert.DeserializeObject<ProblemDetails>(await response.Content.ReadAsStringAsync());

            Assert.NotNull(problemDetails);
        }

        #endregion
    }
}
