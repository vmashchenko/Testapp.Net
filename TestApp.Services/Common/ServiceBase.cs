using System;
using System.Threading.Tasks;
using System.Transactions;

namespace TestApp.Services
{
    public class ServiceBase
    {
        protected async Task<T> ExecuteInTransaction<T>(Func<Task<T>> func)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var result = await func();

                scope.Complete();

                return result;
            }
        }

        protected async Task ExecuteInTransaction(Func<Task> func)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                await func();
                scope.Complete();
            }
        }

        protected async Task<ServiceResponse<T>> ExecuteInTransaction<T>(Func<Task<ServiceResponse<T>>> func)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var result = await func();

                    scope.Complete();

                    return result;
                }
            }
            catch (TransactionScopeException e)
            {
                return ServiceResponse.Error<T>(e.ServiceResponse);
            }
        }

        protected async Task<ServiceResponse> ExecuteInTransaction(Func<Task<ServiceResponse>> func)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var result = await func();

                    scope.Complete();

                    return result;
                }
            }
            catch (TransactionScopeException e)
            {
                return e.ServiceResponse;
            }
        }

        protected void ExecuteInTransaction(Action action)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                action();

                scope.Complete();
            }
        }
    }    
}
