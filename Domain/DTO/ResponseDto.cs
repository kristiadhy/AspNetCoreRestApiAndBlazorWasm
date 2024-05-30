namespace Domain.DTO;

public class ResponseDto
{
    public bool IsSuccess { get; set; }
    public string Error { get; set; } = string.Empty;
}
