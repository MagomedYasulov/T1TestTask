using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T1TestTask.Data;
using T1TestTask.Data.Entites;

namespace T1TestTask.Tests.Extentions
{
    public static class ApplicationContextExtentions
    {
        public static void SeedData(this ApplicationContext dbContext)
        {
            var course1 = new Course()
            {
                Id = new Guid("f2693670-5516-4eb9-845b-803642fcb19f"),
                Name = "Курс 1",
                Students = new List<Student>()
                {
                    new Student()
                    {
                        Id = Guid.NewGuid(),
                        FullName = "Studen1 Full Namme"
                    }
                }
            };

            var course2 = new Course()
            {
                Id = Guid.NewGuid(),
                Name = "Курс 2",
                Students = new List<Student>()
                {
                    new Student()
                    {
                        Id = Guid.NewGuid(),
                         FullName = "Studen2 Full Namme"
                    }
                }
            };

            var course3 = new Course()
            {
                Id = new Guid("9bff4b5c-91f4-4db9-b927-27ccf9b0b131"),
                Name = "Курс 3",
            };

            var course4 = new Course()
            {
                Id = new Guid("0eab5f55-68c5-47f6-a63d-dcd25a18fe1a"),
                Name = "Курс 4",
            };

            var course5 = new Course()
            {
                Id = new Guid("711f59a9-dddb-4d97-9004-374cdf41e4a0"),
                Name = "Курс 5",
            };

            dbContext.Courses.AddRange(course1, course2, course3, course4, course5);
            dbContext.SaveChanges();
        }
    }
}
