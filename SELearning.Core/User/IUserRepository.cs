namespace SELearning.Core.User;

public interface IUserRepository
{
    Task<UserDTO> GetOrAddUser(UserDTO user);
}
