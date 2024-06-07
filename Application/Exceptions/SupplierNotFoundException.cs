namespace Application.Exceptions;
public sealed class SupplierIDNotFoundException(Guid supplierID) : NotFoundException($"The supplier with id: {supplierID} doesn't exist in the database.") { }

public sealed class NoSupplierFoundException() : NotFoundException($"There are no suppliers in the database") { }