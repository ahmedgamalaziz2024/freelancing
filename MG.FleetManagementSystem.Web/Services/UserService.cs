using MG.FleetManagementSystem.Web.Models;
using MG.FleetManagementSystem.Web.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace MG.FleetManagementSystem.Web.Services
{
    public interface IUserService
    {
        User Authenticate(string username, string password);
        IEnumerable<User> GetAll();
        User GetById(int id);
    }

    public class UserService : IUserService
    {
        private List<User> _users;
        public UserService() { 
            var _userRepository = new UserRepository();
            _users = _userRepository.GetAll().ToList();
        }
        public User Authenticate(string username, string password)
        {
            return _users.SingleOrDefault(x => x.Username == username && x.Password == password);
        }

        public IEnumerable<User> GetAll()
        {
            return _users;
        }

        public User GetById(int id)
        {
            return _users.FirstOrDefault(x => x.Id == id);
        }
    }
}