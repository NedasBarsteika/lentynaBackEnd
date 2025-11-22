namespace lentynaBackEnd.Common
{
    public class Result
    {
        public bool IsSuccess { get; }
        public string Message { get; }

        public Result(bool isSuccess, string message)
        {
            IsSuccess = isSuccess;
            Message = message;
        }

        public static Result Failure(string message) => new(false, message);
        public static Result Success() => new(true, string.Empty);
        public static Result Success(string message) => new(true, message);
    }
}
