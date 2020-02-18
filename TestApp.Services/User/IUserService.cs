using System.Threading.Tasks;

namespace TestApp.Services.User
{
    public interface IUserService
    {
        Task<User> Authenticate(string username, string password);
    }
}
