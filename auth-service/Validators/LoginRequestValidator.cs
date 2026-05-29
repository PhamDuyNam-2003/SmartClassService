using auth_service.DTOs.Requests;
using FluentValidation;

namespace auth_service.Validators;

public class LoginRequestValidator
    : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.UserNameOrEmail)
            .NotEmpty();

        RuleFor(x => x.Password)
            .NotEmpty();
    }
}