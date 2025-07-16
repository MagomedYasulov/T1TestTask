
using T1TestTask.Extentions;

namespace T1TestTask
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.AddData();
            builder.AddControllers();
            builder.AddAutoMapper();
            builder.AddAppServices();
            builder.AddExceptionHandler();
            builder.AddSwagger();
            builder.AddFluentValidation();

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
                app.MigrateDB();

            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseExceptionHandler();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
