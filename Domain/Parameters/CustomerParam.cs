namespace Domain.Parameters;

public class CustomerParam : RequestParameters
{
    public CustomerParam() => OrderBy = "CustomerName";
    public string? srcByName { get; set; }
}