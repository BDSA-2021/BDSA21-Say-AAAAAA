namespace SELearning.Infrastructure.User;

public class UserRepository : IUserRepository
{
    private readonly ISELearningContext _context;

    public UserRepository(ISELearningContext context)
    {
        _context = context;
    }

    public async Task<UserDTO> GetOrAddUser(UserDTO user)
    {
        var retrievedUser = await _context
            .Users
            .FirstOrDefaultAsync(u => u.Id == user.Id);

        if (retrievedUser != null)
        {
            return ConvertToDTO(retrievedUser);
        }

        var createUser = new User
        {


            Id = user.Id,
            Name = user.Name
        };

        await _context.Users.AddAsync(createUser);
        await _context.SaveChangesAsync();

        return ConvertToDTO(createUser);
    }

    private UserDTO ConvertToDTO(User user) => new UserDTO(user.Id, user.Name);
}