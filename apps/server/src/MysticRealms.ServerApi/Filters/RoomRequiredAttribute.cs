using Microsoft.AspNetCore.Mvc;

namespace MysticRealms.ServerApi.Filters;

/// <summary>
/// Ensures the authenticated user is a participant of the game identified by the {gameId} route value
/// and that the game has a valid, active combat room.
/// Wraps <see cref="RoomRequiredFilter"/>.
/// </summary>
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class RoomRequiredAttribute : TypeFilterAttribute
{
    public RoomRequiredAttribute() : base(typeof(RoomRequiredFilter)) { }
}
