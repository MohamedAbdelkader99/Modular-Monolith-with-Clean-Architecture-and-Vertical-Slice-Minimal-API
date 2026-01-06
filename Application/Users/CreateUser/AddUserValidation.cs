using FluentValidation;

namespace Application.Users.CreateUser;

public sealed class AddUserValidation : AbstractValidator<CreateUserCommand>
{
    public AddUserValidation()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(100);

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(320);
    }
}
