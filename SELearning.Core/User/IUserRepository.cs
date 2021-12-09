namespace SELearning.Core.User;

public interface IUserRepository
{
    Task<User> GetOrAddUser(UserDTO user);
}