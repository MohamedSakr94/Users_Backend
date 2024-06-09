namespace Users.DAL
{
    public class UserRepo : GenericRepo<User>, IUserRepo
    {
        private readonly UsersContext _options;

        public UserRepo(UsersContext options) : base(options)
        {
            this._options = options;
        }
        public User? GetByEmailAndPassword(string email, string password)
        {
            return _options.Set<User>()
                .FirstOrDefault(
                    u => u.Email == email
                    && u.PasswordHash == password);
        }
        public User? GetByEmail(string email)
        {
            return _options.Set<User>()
                .FirstOrDefault(
                    u => u.Email == email);
        }
    }
}
