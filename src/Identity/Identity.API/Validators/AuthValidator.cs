namespace Identity.API.Validators
{
    using FluentValidation;
    using Identity.Application.Models;

    public class AuthValidator : AbstractValidator<AuthRequest>
    {
        public AuthValidator()
        {
            RuleFor(_ => _.Email)
                .EmailAddress();

            RuleFor(_ => _.Password)
                .NotNull()
                .NotEmpty()
                .MinimumLength(5);
        }
    }
}
