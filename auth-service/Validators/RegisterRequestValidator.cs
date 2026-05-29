using auth_service.DTOs.Requests;
using FluentValidation;

namespace auth_service.Validators;

public class RegisterRequestValidator
    : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty()
            .MinimumLength(3); 

        RuleFor(x => x.UserName)
            .NotEmpty()
            .MinimumLength(4);

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(1);
    }
}