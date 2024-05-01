namespace Domain.Parameters;

public abstract class RequestParameters
{
    private int _pageSize = 10;
    const int maxPageSize = 50;

    public int PageNumber { get; set; } = 1;
    public int PageSize
    {
        get { return _pageSize; }
        set
        {
            _pageSize = (value > maxPageSize) ? maxPageSize : value;
        }
    }
    public string? OrderBy { get; set; }
}
