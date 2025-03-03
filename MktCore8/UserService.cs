using ComLineData;
using Manosque.ServiceData;
using MktCore8.Controllers;

namespace MktCore8
{
    public interface IUserService
    {
        User? Authenticate(string username, string password);
    }

    public class UserService(IConfiguration config, ILogger<UserService> logger) : IUserService
    {
        private readonly string? ConnectionString = config.GetConnectionString("DefaultConnection");
        private readonly ILogger<UserService> _logger = logger;

        public User? Authenticate(string username, string password)
        {
            _logger.LogInformation("UserService.Authenticate");

            if (ConnectionString != null)
            {
                new ServiceData(ConnectionString).Authenticate(username, password);
                if (ServiceData.Exception == null)
                {
                    return new User { Username = username, Password = password };
                }
                else
                {
                    _logger.LogError("UserService.Authenticate: {ServiceData.Exception}", ServiceData.Exception);

                }
            }
            return null;
        }
    }
    public class User
    {
        public string Username { get; set; } = "";
        public string Password { get; set; } = ""; // Assurez-vous de stocker les mots de passe de manière sécurisée !
    }
}
