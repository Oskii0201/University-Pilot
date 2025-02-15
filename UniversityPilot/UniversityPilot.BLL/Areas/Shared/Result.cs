namespace UniversityPilot.BLL.Areas.Shared
{
    public class Result
    {
        public bool IsSuccess { get; private set; }
        public string Message { get; private set; }
        public string ErrorCode { get; private set; }
        public IEnumerable<string> Errors { get; private set; }

        private Result(bool isSuccess, string message, string errorCode = null, IEnumerable<string> errors = null)
        {
            IsSuccess = isSuccess;
            Message = message;
            ErrorCode = errorCode;
            Errors = errors ?? Enumerable.Empty<string>();
        }

        public static Result Success(string message = "Operation completed successfully.")
        {
            return new Result(true, message);
        }

        public static Result Failure(string message, string errorCode = null, IEnumerable<string> errors = null)
        {
            return new Result(false, message, errorCode, errors);
        }
    }
}