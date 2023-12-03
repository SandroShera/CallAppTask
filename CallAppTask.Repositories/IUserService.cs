using CallAppTask.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CallAppTask.Repositories
{
    public interface IUserService
    {
        public User Create(User user);
        public User Get(int id);
        public List<User> GetAll();
        public User Update(User user);
        public bool Delete(int id);
    }
}
