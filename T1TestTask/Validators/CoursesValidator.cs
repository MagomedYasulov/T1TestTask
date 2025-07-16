using FluentValidation;
using T1TestTask.ViewModels.Request;

namespace T1TestTask.Validators
{
    public class CreateCourseValidator : AbstractValidator<CreateCourseDto>
    {
        public CreateCourseValidator()
        {
            RuleFor(c => c.Name).NotEmpty();
        }
    }

    public class UpdateCourseValidator : AbstractValidator<CreateCourseDto>
    {
        public UpdateCourseValidator()
        {
            RuleFor(c => c.Name).NotEmpty();
        }
    }
}
