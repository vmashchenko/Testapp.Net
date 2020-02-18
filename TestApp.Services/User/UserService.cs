using System.Threading.Tasks;

namespace TestApp.Services.User
{
    public class UserService : IUserService
    {
        public async Task<User> Authenticate(string username, string password)
        {
            // fake authentication
            return new User() { Id = 1, Username = username };
        }
    }
}
