namespace Domain.DTO;

public record CustomerDTO(Guid CustomerID, string? CustomerName, string? PhoneNumber, string? Address, string? Email, string? ContactPerson, string? CPPhone, string? Note, bool? Active) : BaseDTO
{
    //public Guid CustomerID { get; init; }
}