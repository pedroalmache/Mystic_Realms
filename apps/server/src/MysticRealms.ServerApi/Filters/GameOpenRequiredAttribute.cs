using Microsoft.AspNetCore.Mvc;

namespace MysticRealms.ServerApi.Filters;

/// <summary>
/// Ensures the game identified by the {gameId} route value is in <c>Open</c> status.
/// If the game is in any other status, the endpoint returns <c>200 OK</c> with a single-element
/// list containing a <c>GameOver</c> action instead of executing the action body.
/// Wraps <see cref="GameOpenRequiredFilter"/>.
/// </summary>
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class GameOpenRequiredAttribute : TypeFilterAttribute
{
    public GameOpenRequiredAttribute() : base(typeof(GameOpenRequiredFilter)) { }
}
