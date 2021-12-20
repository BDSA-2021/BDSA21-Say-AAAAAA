namespace SELearning.Infrastructure.Tests;

public class UserTests
{
    [Fact]
    public void Equals_return_true_when_users_are_equal()
    {
        var user1 = new User.User { Id = "User1", Name = "User1" };
        var user2 = new User.User { Id = "User1", Name = "User1" };

        Assert.True(user1.Equals(user2));
    }

    [Fact]
    public void Equals_return_false_when_users_are_not_equal()
    {
        var user1 = new User.User { Id = "User1", Name = "User1" };
        var user2 = new User.User { Id = "User2", Name = "User1" };

        Assert.False(user1.Equals(user2));
    }
}
