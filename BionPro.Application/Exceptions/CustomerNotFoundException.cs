namespace Application.Exceptions;
public sealed class CustomerIDNotFoundException(Guid customerId) : NotFoundException($"The customer with id: {customerId} doesn't exist in the database.") { }

public sealed class NoCustomerFoundException() : NotFoundException($"There are no customers in the database") { }