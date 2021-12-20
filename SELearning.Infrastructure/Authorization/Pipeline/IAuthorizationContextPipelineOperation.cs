namespace SELearning.Infrastructure.Authorization.Pipeline;

/// <summary>
/// Defines methods for the policy pipeline operation module
/// </summary>
public interface IAuthorizationContextPipelineOperation
{
    /// <summary>
    /// Starts the pipeline operation and invokes the next pipeline operation when done
    /// </summary>
    /// <param name="context"></param>
    Task Invoke(PermissionAuthorizationContext context);
}
