namespace Application.Repositories;

public interface IRepositoryManager
{
    ICustomerRepo CustomerRepo { get; }
    IUnitOfWorkRepo UnitOfWorkRepo { get; }
}
