#nullable disable
namespace SELearning.Infrastructure.User;

public class User : IEquatable<User>
{
    public string Id { get; set; }

    public string Name { get; set; }

    public bool Equals(User other)
        => other != null && other.Id == Id && other.Name == Name;

    public UserDTO ToUserDTO() => new(Id, Name);
}
