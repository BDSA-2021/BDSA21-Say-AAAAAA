namespace SELearning.Infrastructure.Authorization;

public interface IProvider<T>
{
    T Get();
}