using Domain.DTO;
using FluentValidation;

namespace Application.Validators;

public class UserDtoValidator : AbstractValidator<UserRegistrationDTO>
{
    public UserDtoValidator()
    {
        RuleFor(user => user.Roles)
            .NotEmpty();

        RuleFor(user => user.Password)
            .NotEmpty();

        RuleFor(user => user.Email)
          .NotEmpty()
          .EmailAddress();
    }

    public void ValidateInput(UserRegistrationDTO userRegistrationDTO)
    {
        var validationResult = Validate(userRegistrationDTO);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult);
    }
}
