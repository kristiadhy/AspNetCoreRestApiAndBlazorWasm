namespace Application.Repositories;

public class CommonLogMessage(string entityName)
{
    public string EntityName { get; set; } = entityName;

    public string GetALl_Message()
    {
        return $"Get all {EntityName}";
    }

    public string GetByID_Message()
    {
        return $"Get {EntityName} with this ID : ";
    }

    public string Validate_Message()
    {
        return $"Validate {EntityName}";
    }

    public string InsertNew_Message()
    {
        return $"Insert new {EntityName}";
    }

    public string Update_Message()
    {
        return $"Insert new {EntityName}";
    }
}
