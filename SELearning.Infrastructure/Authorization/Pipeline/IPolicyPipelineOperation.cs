using System.Security.Claims;
using SELearning.Core.Collections;
using SELearning.Core.Permission;

namespace SELearning.Infrastructure.Authorization;

/// <summary>
/// Defines methods for the policy pipeline operation module
/// </summary>
public interface IPolicyPipelineOperation
{
    /// <summary>
    /// Sets the next pipeline module to be invoked after this current instance
    /// </summary>
    /// <param name="operation">The next pipeline operation</param>
    void SetNext(IPolicyPipelineOperation operation);

    /// <summary>
    /// Starts the pipeline operation and invokes the next pipeline operation when done
    /// </summary>
    /// <param name="data"></param>
    Task Invoke(PermissionAuthorizationContext context);
}