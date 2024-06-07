using Domain.Entities;
using FluentValidation;

namespace Application.Validators;

public class SupplierValidator : AbstractValidator<SupplierModel>
{
    public SupplierValidator()
    {
        RuleFor(supplier => supplier.SupplierName)
            .NotEmpty()
            .MaximumLength(200)
            ;

        RuleFor(supplier => supplier.PhoneNumber)
            .NotEmpty()
            .MaximumLength(50)
            ;

        RuleFor(supplier => supplier.Email)
           .NotEmpty()
           .MaximumLength(100)
           .EmailAddress()
           ;

        RuleFor(supplier => supplier.ContactPerson)
           .MaximumLength(100)
           ;
    }

    public void ValidateInput(SupplierModel supplierModel)
    {
        var validationResult = Validate(supplierModel);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult);
    }
}
