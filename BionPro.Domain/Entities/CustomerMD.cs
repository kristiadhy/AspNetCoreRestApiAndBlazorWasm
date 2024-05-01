namespace Domain.Entities;

public class CustomerMD : BaseEntity
{
    public Guid CustomerID { get; set; }
    public string? CustomerName { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Address { get; set; }
    public string? Email { get; set; }
    public string? ContactPerson { get; set; }
    public string? CPPhone { get; set; }
    public string? Note { get; set; }
    public bool? Active { get; set; }
}