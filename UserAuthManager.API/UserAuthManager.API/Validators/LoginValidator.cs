using FluentValidation;
using UserAuthManager.API.Models;

namespace UserAuthManager.API.Validators
{
    public class LoginValidator: AbstractValidator<Login>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Email).EmailAddress().NotEmpty();
            RuleFor(x => x.Password).NotEmpty();
        }
    }
}