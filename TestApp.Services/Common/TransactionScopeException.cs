using System;

namespace TestApp.Services
{
    public class TransactionScopeException : Exception
    {
        public TransactionScopeException(string message)
            : this(ServiceResponse.Error(message))
        {
        }

        public TransactionScopeException(ServiceResponse serviceResponse)
            : base(serviceResponse.ErrorMessage)
        {
            ServiceResponse = serviceResponse;
        }

        public ServiceResponse ServiceResponse { get; }
    }
}
