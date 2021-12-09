namespace SELearning.Core.User;

public class User
{
    #nullable disable
    public string Id { get; set; }

    public string Name { get; set; }

    public User(string name)
    {
        Name = name;
    }


}