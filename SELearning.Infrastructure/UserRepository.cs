namespace SELearning.Infrastructure;

public class UserRepository : IUserRepository {
    private readonly ISELearningContext _context;

    public UserRepository(ISELearningContext context)
    {
        _context = context;
    }

    public async Task<User> GetOrAddUser(UserDTO user) {
        var retrievedUser = await _context
            .Users
            .FirstOrDefaultAsync(u => u.Id == user.Id);
        
        if (retrievedUser != null) {
            return retrievedUser;
        }

        var createUser = new User {
            Id = user.Id,
            Name = user.Name
        };
        await _context.Users.AddAsync(createUser);
        await _context.SaveChangesAsync();

        return createUser; 
    }
}