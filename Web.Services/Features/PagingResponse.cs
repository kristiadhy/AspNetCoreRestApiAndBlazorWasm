using Domain.Parameters;

namespace Features;

public class PagingResponse<T> where T : class
{
    public List<T> Items { get; set; } = default!;
    public MetaData MetaData { get; set; } = default!;
}
