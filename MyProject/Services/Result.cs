namespace MyProject.Services;
public class Result
{
    public bool IsSuccess { get; }
    public string ErrorMessage { get; }

    private Result(bool isSuccess, string errorMessage)
    {
        IsSuccess = isSuccess;
        ErrorMessage = errorMessage;
    }

    public static Result Success() => new Result(true, string.Empty);
    
    public static Result Fail(string message) => 
        new Result(false, string.IsNullOrWhiteSpace(message) ? "Wystąpił nieznany błąd." : message);
}