namespace Application.IRepositories;

public interface IRepositoryManager
{
    ICustomerRepo CustomerRepo { get; }
    ISupplierRepo SupplierRepo { get; }
    IUnitOfWorkRepo UnitOfWorkRepo { get; }
}
