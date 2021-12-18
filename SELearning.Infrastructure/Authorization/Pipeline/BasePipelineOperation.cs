using System.Security.Claims;
using SELearning.Core.Collections;

namespace SELearning.Infrastructure.Authorization;

public abstract class BasePipelineOperation : IPolicyPipelineOperation
{
    private IPolicyPipelineOperation? _nextOperation;


    public abstract Task Invoke(PermissionAuthorizationContext context);

    public void SetNext(IPolicyPipelineOperation operation)
    {
        if(HasNext)
            _nextOperation?.SetNext(operation);
        else
            _nextOperation = operation;
    }

    public bool HasNext => _nextOperation != null;

    protected async Task InvokeNext(PermissionAuthorizationContext context)
    {
        if(_nextOperation != null)
            await _nextOperation.Invoke(context);
    }

}