namespace Users.DAL
{
    public interface IUserRepo : IGenericRepo<User>
    {
        public User? GetByEmailAndPassword(string email, string password);
        public User? GetByEmail(string email);
        public User? GetByIdWithdetails(string id);
    }
}
