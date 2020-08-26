using FluentValidation;
using UserAuthManager.API.Models;

namespace UserAuthManager.API.Validators
{
    public class RegisterValidator: AbstractValidator<Register>
    {
        public RegisterValidator()
        {
            RuleFor(x => x.Email).EmailAddress().NotEmpty();
            RuleFor(x => x.UserName).NotEmpty();
            RuleFor(x => x.Password).NotEmpty();
        }
    }
}