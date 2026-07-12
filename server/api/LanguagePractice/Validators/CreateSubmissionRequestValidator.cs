using Api.Submissions.DTOs;
using FluentValidation;

namespace Api.Submissions.Validators;

public class CreateSubmissionRequestValidator : AbstractValidator<CreateSubmissionRequest>
{
    public CreateSubmissionRequestValidator()
    {
        // Max length is fairly arbitrary for now, can adjust in future
        RuleFor(x => x.Text)
            .NotEmpty()
            .MaximumLength(500);
    }
}
