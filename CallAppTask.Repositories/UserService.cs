using CallAppTask.DTO;

namespace CallAppTask.Repositories
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public User Create(User user)
        {
            _context.Add(user);
            _context.SaveChanges();

            return user;
        }

        public bool Delete(int id)
        {
            _context.User.Remove(Get(id));
            return true;
        }

        public User Get(int id)
        {
            return _context.User.Find(id)!;
        }

        public List<User> GetAll()
        {
            return _context.User.ToList();
        }

        public User Update(User user)
        {
            _context.User.Attach(user);
            _context.SaveChanges();

            return user;
        }
    }
}
