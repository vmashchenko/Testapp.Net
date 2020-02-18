using TestApp.Core.Guard;

namespace TestApp.Services
{
    public class ServiceResponse
    {
        protected ServiceResponse()
        {
        }

        protected ServiceResponse(string errorMessage, object errorDetails = null, ErrorResponseType errorType = ErrorResponseType.Unknown)
        {
            Throw.IfNullOrWhiteSpace(errorMessage, nameof(errorMessage));
            IsError = true;
            ErrorMessage = errorMessage;
            ErrorDetails = errorDetails;
            ErrorType = errorType;
        }

        public bool IsError { get; }

        public ErrorResponseType ErrorType { get; set; }

        public string ErrorMessage { get; set; }

        public object ErrorDetails { get; set; }

        public static ServiceResponse Ok() => new ServiceResponse();

        public static ServiceResponse<T> Ok<T>(T value) => new ServiceResponse<T>(value);

        public static ServiceResponse NotFound(string entityType) => new ServiceResponse($"{entityType} not found", errorType: ErrorResponseType.NotFound);

        public static ServiceResponse<T> NotFound<T>(string entityType) => new ServiceResponse<T>($"{entityType} not found", errorType: ErrorResponseType.NotFound);                                

        public static ServiceResponse Error(string error, object details = null) => new ServiceResponse(error, details);

        public static ServiceResponse<T> Error<T>(string error, object details = null) => new ServiceResponse<T>(error, details);

        public static ServiceResponse<T> Error<T>(ServiceResponse errorResponse)
        {
            if (!errorResponse.IsError)
            {
                throw new System.InvalidOperationException("Isn't error response!");
            }

            return new ServiceResponse<T>(errorResponse.ErrorMessage, errorResponse.ErrorDetails, errorResponse.ErrorType);
        }
    }

    public class ServiceResponse<T> : ServiceResponse
    {
        public ServiceResponse(T value)
        {
            Value = value;
        }

        public ServiceResponse(string errorMessage, object errorDetails = null, ErrorResponseType errorType = ErrorResponseType.Unknown) : base(errorMessage, errorDetails, errorType)
        {
        }

        public T Value { get; set; }

        public static implicit operator ServiceResponse<T>(T value) => Ok(value);
    }
}
