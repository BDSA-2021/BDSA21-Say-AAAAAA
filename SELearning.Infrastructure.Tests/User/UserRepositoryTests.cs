using System.Threading.Tasks;
using SELearning.Core.User;
using SELearning.Infrastructure.User;

namespace SELearning.Infrastructure.Tests;

public class UserRepositoryTests
{
    private readonly UserRepository _repository;

    public UserRepositoryTests()
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();
        var builder = new DbContextOptionsBuilder<SELearningContext>()
            .UseSqlite(connection);
        var context = new SELearningContext(builder.Options);
        context.Database.EnsureCreated();

        context.Users.Add(new User.User {Id = "User 1", Name = "User 1"});

        _repository = new UserRepository(context);
        context.SaveChanges();
    }

    [Fact]
    public async Task GetOrAddUser_given_non_existing_user_return_user()
    {
        var (id, name) = await _repository.GetOrAddUser(new UserDTO("User 2", "User 2"));
        Assert.Equal("User 2", id);
        Assert.Equal("User 2", name);
    }

    [Fact]
    public async Task GetOrAddUser_given_existing_user_return_user()
    {
        var (id, name) = await _repository.GetOrAddUser(new UserDTO("User 1", "User 1"));
        Assert.Equal("User 1", id);
        Assert.Equal("User 1", name);
    }
}
