using FluentValidation;

namespace TestApp.Services.User
{
    public class UserInfoVmValidator : AbstractValidator<UserInfoVm>
    {
        public UserInfoVmValidator()
        {
            RuleFor(e => e.Email).EmailAddress().WithMessage("Please provide valid e-mail address");
            RuleFor(e => e.Password).NotEmpty().WithMessage("Password could not be empty");
        }
    }
}
