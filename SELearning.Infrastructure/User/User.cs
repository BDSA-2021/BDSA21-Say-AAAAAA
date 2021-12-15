using System;

namespace SELearning.Infrastructure.User;

public class User : IEquatable<User>
{
#nullable disable
    public string Id { get; set; }

    public string Name { get; set; }

    public bool Equals(User other)
        => other.Id == Id && other.Name == Name;
}