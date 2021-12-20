namespace SELearning.Core;

public interface IProvider<T>
{
    T Get();
}
