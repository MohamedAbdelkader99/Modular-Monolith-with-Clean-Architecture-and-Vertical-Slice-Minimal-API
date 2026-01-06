using FluentValidation;

namespace Application.Jobs.CreateJob;

public sealed class AddJobValidation : AbstractValidator<CreateJobCommand>
{
    public AddJobValidation()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(100);
    }
}
