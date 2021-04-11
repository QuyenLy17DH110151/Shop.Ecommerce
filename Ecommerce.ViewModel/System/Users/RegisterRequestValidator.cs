using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.ViewModel.System.Users
{
    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator()
        {
            RuleFor(x => x.FristName).NotEmpty().WithMessage("First name is required")
               .MaximumLength(200).WithMessage("Max length is 200");

            RuleFor(x => x.LastName).NotEmpty().WithMessage("Last name is required")
              .MaximumLength(200).WithMessage("Max length is 200");

            RuleFor(x => x.Dob).GreaterThan(DateTime.Now.AddYears(-100)).WithMessage("Dob cannot greater than 100 years");

            RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required")
                .Matches(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$")
                .WithMessage("Email format not match");

            RuleFor(x => x.PhoneNumber).NotNull().WithMessage("Phone number is not null");
            RuleFor(x => x.UserName).NotEmpty().WithMessage("User name is required");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required")
                .MinimumLength(6).WithMessage("Password is at least 6 characters");
            RuleFor(x => x).Custom((request, context) => {
                if (request.Password!=request.ConfirmPassword)
                {
                    context.AddFailure("confirm password is not match");
                }
            });
        }
    }
}
