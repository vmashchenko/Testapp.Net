using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;

using TestApp.Services;

namespace TestAppApi.Infrastructure.Extensions
{
    public static class ServiceResponseExtensions
    {
        public static T ThrowIfError<T>(this T serviceResponse)
            where T : ServiceResponse
        {
            if (serviceResponse.IsError)
                throw new TransactionScopeException(serviceResponse);
            return serviceResponse;
        }

        public static async Task<T> ThrowIfError<T>(this Task<T> serviceResponseTask)
            where T : ServiceResponse
            => (await serviceResponseTask).ThrowIfError();

        public static void ThrowIfError(this IdentityResult identityResult)
        {
            if (!identityResult.Succeeded)
                throw new TransactionScopeException(identityResult.Errors.First().Description);
        }

        public static async Task ThrowIfError(this Task<IdentityResult> identityResultTask)
            => (await identityResultTask).ThrowIfError();
    }
}
