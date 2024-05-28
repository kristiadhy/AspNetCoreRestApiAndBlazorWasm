using Domain.Entities;
using FluentValidation;

namespace Application.Validators;

public class CustomerValidator : AbstractValidator<CustomerMD>
{
    public CustomerValidator()
    {
        RuleFor(customer => customer.CustomerName)
            .NotEmpty()
            .MaximumLength(200)
            ;

        RuleFor(customer => customer.PhoneNumber)
            .NotEmpty()
            .MaximumLength(50)
            ;

        RuleFor(customer => customer.Email)
           .NotEmpty()
           .MaximumLength(100)
           .EmailAddress()
           //.MustAsync(async (email, _) => await IsUniqueAsync(email))
           ;

        RuleFor(customer => customer.ContactPerson)
           .MaximumLength(100)
           ;
    }

    //Example of how to check uniqueness of email in the database
    private static async Task<bool> IsUniqueAsync(string? email)
    {
        await Task.Delay(300);
        return email?.ToLower() != "mail@my.com";
    }

    public void ValidateInput(CustomerMD customerMD)
    {
        var validationResult = Validate(customerMD);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult);
    }
}
