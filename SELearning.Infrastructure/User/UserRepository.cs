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
        var (id, name) = user;
        var retrievedUser = await _context
            .Users
            .FirstOrDefaultAsync(u => u.Id == id);

        if (retrievedUser != null)
        {
            return retrievedUser.ToUserDTO();
        }

        var createUser = new User
        {
            Id = id,
            Name = name
        };

        await _context.Users.AddAsync(createUser);
        await _context.SaveChangesAsync();

        return createUser.ToUserDTO();
    }
}
