using ComLineData;
using Manosque.ServiceData;

namespace MktCore8
{
    public interface IUserService
    {
        User Authenticate(string username, string password);
    }

    public class UserService : IUserService
    {
        private string? ConnectionString;
        public UserService(IConfiguration config)
        {
            ConnectionString = config.GetConnectionString("DefaultConnection");
        }
        //private List<User> _users = new List<User>
        //{
        //    new User { Username = "test", Password = "password" } // Utilisez un hachage pour stocker les mots de passe dans un vrai projet
        //};

        public User Authenticate(string username, string password)
        {
            //var user = _users.SingleOrDefault(x => x.Username == username && x.Password == password);
            if (ConnectionString != null)
            {
                if (new ServiceData(ConnectionString).Authenticate(username, password))
                {
                    return new User { Username = username, Password = password };
                }
            }
            return null;
        }
    }
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; } // Assurez-vous de stocker les mots de passe de manière sécurisée !
    }
}
