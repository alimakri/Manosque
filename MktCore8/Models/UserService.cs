using Manosque.ServiceData;

namespace MktCore8.Models
{
    public interface IUserService
    {
        User? Authenticate(string username, string password);
    }

    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;
        public UserService(IConfiguration config, ILogger<UserService> logger)
        {
        ServiceData.ConnectionString = config.GetConnectionString("DefaultConnection")??"";
        _logger = logger;
        }
        public User? Authenticate(string username, string password)
        {
            _logger.LogInformation("UserService.Authenticate");

            if (ServiceData.ConnectionString != null)
            {
                var id =new ServiceData().Authenticate(username, password);

                if (id != null)
                {
                    return new User { Id=id, Username = username, Password = password };
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
        public Guid? Id { get; set; } = default;
        public string Username { get; set; } = "";
        public string Password { get; set; } = ""; // Assurez-vous de stocker les mots de passe de manière sécurisée !
    }
}
