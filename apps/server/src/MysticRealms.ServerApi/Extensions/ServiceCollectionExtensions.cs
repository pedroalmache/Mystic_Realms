using MysticRealms.Application.Auth;
using MysticRealms.Application.Interfaces;
using MysticRealms.Domain.Dungeons;
using MysticRealms.Infrastructure.Features.Blessings;
using MysticRealms.Infrastructure.Features.Cards;
using MysticRealms.Infrastructure.Features.CharacterClasses;
using MysticRealms.Infrastructure.Features.Decks;
using MysticRealms.Infrastructure.Features.Dungeons;
using MysticRealms.Infrastructure.Features.Enemies;
using MysticRealms.Infrastructure.Features.Games;
using MysticRealms.Infrastructure.Features.PartyMembers;
using MysticRealms.Infrastructure.Features.Players;
using MysticRealms.Infrastructure.Features.Rooms;
using MysticRealms.Infrastructure.Features.UserCharacters;
using MysticRealms.Infrastructure.Services;

namespace MysticRealms.ServerApi.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IGameRepository, GameRepository>();
        services.AddScoped<IPlayerRepository, PlayerRepository>();
        services.AddScoped<ICharacterRepository, CharacterClassRepository>();
        services.AddScoped<IDungeonRepository, DungeonRepository>();
        services.AddScoped<IPartyMemberRepository, PartyMemberRepository>();
        services.AddScoped<IEnemyRepository, EnemyRepository>();
        services.AddScoped<ICardRepository, CardRepository>();
        services.AddScoped<IDeckRepository, DeckRepository>();
        services.AddScoped<IBlessingRepository, BlessingRepository>();
        services.AddScoped<IRoomRepository, RoomRepository>();
        services.AddScoped<IUserCharacterRepository, UserCharacterRepository>();

        services.AddTransient<RoomTypeGenerator>();
        services.AddTransient<RewardTypeGenerator>();
        services.AddTransient<EnemyGenerator>();
        services.AddTransient<DungeonGenerator>();
        

        services.AddScoped<SeedService>();

        return services;
    }
}