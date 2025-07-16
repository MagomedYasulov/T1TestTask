using FluentValidation;
using T1TestTask.ViewModels.Request;

namespace T1TestTask.Validators
{
    public class CreateStudentValidator : AbstractValidator<CreateStudentDto>
    {
        public CreateStudentValidator()
        {
            RuleFor(c => c.FullName).NotEmpty();
        }
    }
}
