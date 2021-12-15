using Microsoft.Extensions.DependencyInjection;
using SELearning.Core;

namespace SELearning.Infrastructure.Authorization;

public class Provider<T> : IProvider<T>
{
    private readonly IServiceScopeFactory _serviceFactory;

    public Provider(IServiceScopeFactory serviceFactory)
    {
        _serviceFactory = serviceFactory;
    }

    public T Get()
    {
        var scope = _serviceFactory.CreateScope();

        T? service = scope.ServiceProvider.GetService<T>();

        return service!;
    }
}