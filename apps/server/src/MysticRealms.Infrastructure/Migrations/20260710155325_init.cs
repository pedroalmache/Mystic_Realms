using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MysticRealms.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    GamerTag = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Blessings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Level_1_Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Level_2_Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Level_3_Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Level_4_Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blessings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    CharacterId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ManaCost = table.Column<int>(type: "int", nullable: false),
                    Rarity = table.Column<int>(type: "int", nullable: false),
                    Attack = table.Column<int>(type: "int", nullable: false),
                    MagicAttack = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Armor = table.Column<int>(type: "int", nullable: false),
                    MagicArmor = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Mana = table.Column<int>(type: "int", nullable: false),
                    Strength = table.Column<int>(type: "int", nullable: false),
                    Defense = table.Column<int>(type: "int", nullable: false),
                    Draw = table.Column<int>(type: "int", nullable: false),
                    EffectMultiplier = table.Column<int>(type: "int", nullable: false),
                    AscendedManaCost = table.Column<int>(type: "int", nullable: false),
                    AscendedAttack = table.Column<int>(type: "int", nullable: false),
                    AscendedMagicAttack = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    AscendedArmor = table.Column<int>(type: "int", nullable: false),
                    AscendedMagicArmor = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    AscendedMana = table.Column<int>(type: "int", nullable: false),
                    AscendedStrength = table.Column<int>(type: "int", nullable: false),
                    AscendedDefense = table.Column<int>(type: "int", nullable: false),
                    AscendedDraw = table.Column<int>(type: "int", nullable: false),
                    AscendedEffectMultiplier = table.Column<int>(type: "int", nullable: false),
                    Heal = table.Column<int>(type: "int", nullable: false),
                    AscendedHeal = table.Column<int>(type: "int", nullable: false),
                    Poison = table.Column<int>(type: "int", nullable: false),
                    AscendedPoison = table.Column<int>(type: "int", nullable: false),
                    LifeCost = table.Column<int>(type: "int", nullable: false),
                    AscendedLifeCost = table.Column<int>(type: "int", nullable: false),
                    PlayerTargets = table.Column<int>(type: "int", nullable: false),
                    AscendedPlayerTargets = table.Column<int>(type: "int", nullable: false),
                    EnemyTargets = table.Column<int>(type: "int", nullable: false),
                    AscendedEnemyTargets = table.Column<int>(type: "int", nullable: false),
                    Permanent = table.Column<bool>(type: "bit", nullable: true),
                    Momentum = table.Column<int>(type: "int", nullable: true),
                    Daggers = table.Column<int>(type: "int", nullable: true),
                    Spirit = table.Column<int>(type: "int", nullable: true),
                    Fire = table.Column<bool>(type: "bit", nullable: true),
                    Water = table.Column<bool>(type: "bit", nullable: true),
                    Air = table.Column<bool>(type: "bit", nullable: true),
                    Earth = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cards", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CharacterClasses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    HitPoints = table.Column<int>(type: "int", nullable: false),
                    Speed = table.Column<int>(type: "int", nullable: false),
                    Energy = table.Column<int>(type: "int", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterClasses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Enemies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MinHitPoints = table.Column<int>(type: "int", nullable: false),
                    MaxHitPoints = table.Column<int>(type: "int", nullable: false),
                    MinSpeed = table.Column<int>(type: "int", nullable: false),
                    MaxSpeed = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Enemies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    Save = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsMultiplayer = table.Column<bool>(type: "bit", nullable: false),
                    GameStatus = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Token = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DeviceInfo = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RevokedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserCharacters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CharacterId = table.Column<int>(type: "int", nullable: false),
                    IsUnlocked = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCharacters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserCharacters_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserCharacters_CharacterClasses_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "CharacterClasses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Dungeons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GameId = table.Column<int>(type: "int", nullable: false),
                    Seed = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Path = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dungeons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Dungeons_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsHost = table.Column<bool>(type: "bit", nullable: false),
                    GameId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Players_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Players_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DungeonId = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Reward = table.Column<int>(type: "int", nullable: false),
                    Seed = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    EnemyIds = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BackgroundId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rooms_Dungeons_DungeonId",
                        column: x => x.DungeonId,
                        principalTable: "Dungeons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Decks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlayerId = table.Column<int>(type: "int", nullable: false),
                    CardId = table.Column<int>(type: "int", nullable: false),
                    PartyMemberId = table.Column<int>(type: "int", nullable: false),
                    InstanceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsAscended = table.Column<bool>(type: "bit", nullable: false),
                    UseCount = table.Column<int>(type: "int", nullable: false),
                    ManaCost = table.Column<int>(type: "int", nullable: false),
                    Attack = table.Column<int>(type: "int", nullable: false),
                    MagicAttack = table.Column<int>(type: "int", nullable: false),
                    Armor = table.Column<int>(type: "int", nullable: false),
                    MagicArmor = table.Column<int>(type: "int", nullable: false),
                    Mana = table.Column<int>(type: "int", nullable: false),
                    Strength = table.Column<int>(type: "int", nullable: false),
                    Defense = table.Column<int>(type: "int", nullable: false),
                    Draw = table.Column<int>(type: "int", nullable: false),
                    EffectMultiplier = table.Column<int>(type: "int", nullable: false),
                    Heal = table.Column<int>(type: "int", nullable: false),
                    Poison = table.Column<int>(type: "int", nullable: false),
                    LifeCost = table.Column<int>(type: "int", nullable: false),
                    PlayerTargets = table.Column<int>(type: "int", nullable: false),
                    EnemyTargets = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Decks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Decks_Cards_CardId",
                        column: x => x.CardId,
                        principalTable: "Cards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Decks_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PartyMembers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InstanceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlayerId = table.Column<int>(type: "int", nullable: false),
                    ClassId = table.Column<int>(type: "int", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    Health = table.Column<int>(type: "int", nullable: false),
                    MaxHealth = table.Column<int>(type: "int", nullable: false),
                    Mana = table.Column<int>(type: "int", nullable: false),
                    Speed = table.Column<int>(type: "int", nullable: false),
                    Gold = table.Column<int>(type: "int", nullable: false),
                    Spirit = table.Column<int>(type: "int", nullable: true),
                    Fire = table.Column<bool>(type: "bit", nullable: false),
                    Water = table.Column<bool>(type: "bit", nullable: false),
                    Earth = table.Column<bool>(type: "bit", nullable: false),
                    Air = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartyMembers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PartyMembers_CharacterClasses_ClassId",
                        column: x => x.ClassId,
                        principalTable: "CharacterClasses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PartyMembers_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Blessings",
                columns: new[] { "Id", "Level_1_Description", "Level_2_Description", "Level_3_Description", "Level_4_Description", "Name", "Type" },
                values: new object[,]
                {
                    { 1, "Heal 1 HP at the end of every turn.", "Heal 2 HP at the end of every turn.", "Heal 3 HP at the end of every turn.", "Heal 5 HP at the end of every turn.", "Vital Bloom", 3 },
                    { 2, "Whenever you heal, gain 2 extra HP.", "Whenever you heal, gain 3 extra HP.", "Whenever you heal, gain 4 extra HP.", "Whenever you heal, gain 5 extra HP.", "Overflowing Vigor", 3 },
                    { 3, "When your HP drops below 25% for the first time each combat, heal 10 HP.", "When your HP drops below 25% for the first time each combat, heal 15 HP.", "When your HP drops below 30% for the first time each combat, heal 15 HP.", "When your HP drops below 30% for the first time each combat, heal 20 HP.", "Second Wind", 3 },
                    { 4, "Start each combat with a buffer that prevents the first instance of Physical Damage.", "Start each combat with a buffer that prevents the first instance of Magic Damage.", "Start each combat with a buffer that prevents the first instance of HP loss.", "Start each combat with a buffer that heals instead of taking the first instance of Damage.", "Sanctified Veil", 3 },
                    { 5, "Every time you play 5 cards in a turn, heal 2 HP.", "Every time you play 4 cards in a turn, heal 2 HP.", "Every time you play 3 cards in a turn, heal 2 HP.", "Every time you play 3 cards in a turn, heal 5 HP.", "Nature's Embrace", 3 },
                    { 6, "Gain 5 max HP when you acquire this blessing.", "Gain 8 max HP when you acquire this blessing.", "Gain 12 max HP when you acquire this blessing.", "Gain 15 max HP when you acquire this blessing.", "Radiant Core", 3 },
                    { 7, "After defeating an enemy, restore 3 HP.", "After defeating an enemy, restore 4 HP.", "After defeating an enemy, restore 5 HP.", "After defeating an enemy, restore 7 HP.", "Healing Pulse", 3 },
                    { 8, "At the end of each combat, heal 3 HP.", "At the end of each combat, heal 4 HP.", "At the end of each combat, heal 6 HP.", "At the end of each combat, heal 8 HP.", "Preserved Essence", 3 },
                    { 9, "Heal by 15% of max HP before a boss room.", "Heal by 18% of max HP before a boss room.", "Heal by 20% of max HP before a boss room.", "Heal by 25% of max HP before a boss room.", "Bloodbond Pact", 3 },
                    { 10, "Gain 5 max HP when you acquire this blessing.", "Gain 8 max HP when you acquire this blessing.", "Gain 12 max HP when you acquire this blessing.", "Gain 15 max HP when you acquire this blessing.", "Blessed Resolve", 3 },
                    { 11, "Your first attack each turn deals +2 damage.", "Your first attack each turn deals +4 damage.", "Your first attack each turn deals +6 damage.", "Your first attack each turn deals +8 damage.", "Razor Instinct", 6 },
                    { 12, "Every 5th attack deals double damage.", "Every 4th attack deals double damage.", "Every 4th attack deals 150% damage.", "Every 3rd attack deals 150% damage.", "Crimson Burst", 6 },
                    { 13, "First Attack each turn applies 1 Poison.", "First Attack each turn applies 2 Poison.", "First Attack each turn applies 3 Poison.", "First Attack each turn applies 5 Poison.", "Combustion Surge", 6 },
                    { 14, "Every 5th turn gain 1 Power.", "Every 4th turn gain 1 Power.", "Every 4th turn gain 2 Power.", "Every 3rd turn gain 2 Power.", "Momentum Drive", 6 },
                    { 15, "Deal 25% more damage to enemies below 25% HP.", "Deal 30% more damage to enemies below 30% HP.", "Deal 35% more damage to enemies below 30% HP.", "Deal 40% more damage to enemies below 35% HP.", "Executioner's Edge", 6 },
                    { 16, "Critical hits deal 25% more damage.", "Critical hits deal 35% more damage.", "Critical hits deal 50% more damage.", "Critical hits deal 75% more damage.", "Precision Strike", 6 },
                    { 17, "Whenever you kill an enemy, gain +2 Power next turn.", "Whenever you kill an enemy, gain +3 Power next turn.", "Whenever you kill an enemy, gain +4 Power next turn.", "Whenever you kill an enemy, gain +5 Power next turn.", "Relentless Assault", 6 },
                    { 18, "Attacks ignore 2 Block.", "Attacks ignore 4 Block.", "Attacks ignore 6 Block.", "Attacks ignore all Block.", "Arcane Penetration", 6 },
                    { 19, "Whenever you lose HP, deal 1 damage back.", "Whenever you lose HP, deal 2 damage back.", "Whenever you lose HP, deal 3 damage back.", "Whenever you lose HP, deal 3 damage back to all enemies.", "Blood Price", 6 },
                    { 20, "Every 7th attack this combat hits all enemies.", "Every 6th attack this combat hits all enemies.", "Every 5th attack this combat hits all enemies.", "Every 4th attack this combat hits all enemies.", "Storm Flurry", 6 },
                    { 21, "Start each Combat with 4 Physical Defense.", "Start each Combat with 6 Physical Defense.", "Start each Combat with 8 Physical Defense.", "Start each Combat with 14 Physical Defense.", "Ironhide", 1 },
                    { 22, "Start each Combat with 4 Magic Defense.", "Start each Combat with 6 Magic Defense.", "Start each Combat with 8 Magic Defense.", "Start each Combat with 14 Magic Defense.", "Mystic Shroud", 1 },
                    { 23, "Gain +1 Armor.", "Gain +2 Armor.", "Gain +3 Armor.", "Gain +4 Armor.", "Stoneform", 1 },
                    { 24, "When you take Physical Damage, gain +2 Physical Defense next turn.", "When you take Physical Damage, gain +3 Physical Defense next turn.", "When you take any damage, gain +3 Physical Defense next turn.", "When you take any damage, gain +4 Physical Defense next turn.", "Reactive Plating", 1 },
                    { 25, "When you take Magic Damage, gain +2 Magic Defense next turn.", "When you take Magic Damage, gain +3 Magic Defense next turn.", "When you take Magic Damage, gain +3 Magic Defense and +1 Armor next turn.", "When you take Magic Damage, gain +4 Magic Defense and +2 Armor next turn.", "Warding Aura", 1 },
                    { 26, "Start combat with +2 Physical Defense for your first turn.", "Start combat with +3 Physical Defense and +1 Armor.", "Start combat with +4 Physical Defense and +1 Armor.", "Start combat with +5 Physical Defense and +2 Armor.", "Bulwark's Embrace", 1 },
                    { 27, "Start combat with +2 Magic Defense for your first turn.", "Start combat with +3 Magic Defense and +1 Armor.", "Start combat with +4 Magic Defense and +1 Armor.", "Start combat with +5 Magic Defense and +2 Armor.", "Arcane Shell", 1 },
                    { 28, "If your HP is below 50%, gain +1 Armor until end of combat.", "If your HP is below 50%, gain +2 Armor until end of combat.", "If your HP is below 40%, gain +3 Armor until end of combat.", "If your HP is below 40%, gain +4 Armor until end of combat.", "Guardian's Resolve", 1 },
                    { 29, "When you gain Physical Defense, gain +1 Magic Defense (1 turn).", "When you gain Physical Defense, gain +1 Magic Defense and +1 Armor.", "When you gain Physical Defense, gain +2 Magic Defense and +1 Armor.", "When you gain Physical Defense, gain +2 Magic Defense and +2 Armor.", "Aegis Flow", 1 },
                    { 30, "Blocking 5+ damage in a turn grants +1 Armor (rest of combat).", "Blocking 4+ damage in a turn grants +1 Armor (rest of combat).", "Blocking 4+ damage in a turn grants +2 Armor (rest of combat).", "Blocking 3+ damage in a turn grants +2 Armor (rest of combat).", "Tempered Fortitude", 1 },
                    { 31, "Gain +1 mana every 3rd turn.", "Gain +1 mana every 2nd turn.", "Gain +1 mana every turn.", "Gain +1 mana every turn and +1 on the first turn.", "Mana Spring", 4 },
                    { 32, "The first card you play each turn costs 1 less mana.", "The first two cards you play each turn cost 1 less mana.", "The first card you play each turn costs 2 less mana.", "The first two cards you play each turn cost 2 less mana.", "Efficient Flow", 4 },
                    { 33, "Unspent mana at the end of your turn carries over 1 point.", "Unspent mana carries over 2 points.", "Unspent mana carries over 3 points.", "All unspent mana carries over to the next turn.", "Arcane Overflow", 4 },
                    { 34, "Restore 1 mana whenever you play 4 Common cards in a turn.", "Restore 1 mana after playing 3 Common cards in a turn.", "Restore 1 mana after playing 2 Common cards in a turn.", "Restore 1 mana after playing any 3 cards.", "Mana Infusion", 4 },
                    { 35, "Gain 1 extra mana on the first turn of combat.", "Gain 2 mana on the first turn of combat.", "Gain 1 extra mana for the first 2 turns of combat.", "Gain 2 extra mana for the first 2 turns of combat.", "Surge Catalyst", 4 },
                    { 36, "Whenever you draw a card, 10% chance to gain 1 mana.", "15% chance to gain 1 mana when drawing a card.", "20% chance to gain 1 mana when drawing a card.", "25% chance to gain 1 mana when drawing a card.", "Mystic Channeling", 4 },
                    { 37, "If you play 0 cards this turn, gain 1 mana next turn.", "If you play 1 or fewer cards, gain 1 mana next turn.", "If you play 1 or fewer cards, gain 2 mana next turn.", "If you play 1 or fewer cards, gain 3 mana next turn.", "Arcane Meditation", 4 },
                    { 38, "Start each combat with +1 max mana.", "Start each combat with +2 max mana.", "Start each combat with +3 max mana.", "Start each combat with +4 max mana.", "Resonant Core", 4 },
                    { 39, "Gain 1 Armor when you gain mana.", "Gain 2 Armor when you gain mana.", "Gain 2 Armor and +1 Physical Defense (1 turn) when you gain mana.", "Gain 3 Armor and +1 Physical & Magic Defense (1 turn) when you gain mana.", "Mana Armor", 4 },
                    { 40, "Whenever you play 3 cards in a turn, gain 1 mana.", "Whenever you play 2 cards in a turn, gain 1 mana.", "Whenever you play 2 cards in a turn, gain 2 mana.", "Whenever you play 2 cards in a turn, gain 2 mana and draw a card.", "Spell Eater", 4 },
                    { 41, "Gain +2 Initiative at the start of each combat.", "Gain +3 Initiative at the start of each combat.", "Gain +5 Initiative at the start of each combat.", "Gain +7 Initiative at the start of each combat.", "Quickstep", 7 },
                    { 42, "If you play 3 or more cards in a turn, gain +1 Initiative next turn.", "If you play 3 or more cards, gain +2 Initiative next turn.", "If you play 2 or more cards, gain +2 Initiative next turn.", "If you play 2 or more cards, gain +3 Initiative next turn.", "Lightning Focus", 7 },
                    { 43, "Every 4th card played in a turn gives +1 Initiative next turn.", "Every 3rd card gives +1 Initiative next turn.", "Every 2nd card gives +1 Initiative next turn.", "Every 2nd card gives +2 Initiative next turn.", "Tempo Surge", 7 },
                    { 44, "Start turn with +1 Initiative if your hand has 6 or more cards.", "Start turn with +2 Initiative if your hand has 6 or more cards.", "Start turn with +2 Initiative if your hand has 5 or more cards.", "Start turn with +3 Initiative if your hand has 5 or more cards.", "Reflexive Rhythm", 7 },
                    { 45, "At the start of each combat, gain +2 Initiative and draw 1 extra card.", "Gain +3 Initiative and draw 1 extra card.", "Gain +4 Initiative and draw 2 extra cards.", "Gain +5 Initiative and draw 2 extra cards.", "Blurred Motion", 7 },
                    { 46, "When you play the last card in your hand, gain +1 Initiative next turn.", "Gain +2 Initiative next turn.", "Also draw 1 card.", "Gain +2 Initiative and draw 1 card.", "Fleet Edge", 7 },
                    { 47, "At the end of each turn, gain +1 Initiative if you played 4 cards.", "Gain +1 Initiative for 3+ cards.", "Gain +2 Initiative for 3+ cards.", "Gain +2 Initiative and draw 1 card for 3+ cards played.", "Slipstream", 7 },
                    { 48, "Your first card played each turn costs 1 less if you have higher Initiative.", "First two cards cost 1 less if you have higher Initiative.", "First card costs 2 less if you have higher Initiative.", "First two cards cost 2 less if you have higher Initiative.", "Fast Hands", 7 },
                    { 49, "Gain +1 Initiative the first time you play 2 cards in a single turn.", "Gain +1 Initiative and draw 1 card.", "Gain +2 Initiative and draw 1 card.", "Gain +2 Initiative and draw 2 cards.", "Momentum Charge", 7 },
                    { 50, "Whenever your Initiative increases, gain 1 Armor (once per turn).", "Gain 2 Armor instead.", "Gain 2 Armor and +1 Physical Defense (1 turn).", "Gain 3 Armor and +1 Physical & Magic Defense (1 turn).", "Accelerated Will", 7 },
                    { 51, "Gain 10% more gold from all encounters.", "Gain 15% more gold from all encounters.", "Gain 20% more gold from all encounters.", "Gain 25% more gold from all encounters.", "Greedy Heart", 2 },
                    { 52, "Gain 10% more gold from treasure rooms.", "Gain 20% more gold from treasure rooms.", "Gain 30% more gold from treasure rooms.", "Gain 50% more gold from treasure rooms.", "Treasure Seeker", 2 },
                    { 53, "Gain 15 extra gold from elite enemies.", "Gain 25 extra gold from elite enemies.", "Gain 40 extra gold from elite enemies.", "Gain 60 extra gold from elite enemies.", "Elite Tax", 2 },
                    { 54, "Gain 25 extra gold from bosses.", "Gain 50 extra gold from bosses.", "Gain 75 extra gold from bosses.", "Gain 100 extra gold from bosses.", "Boss Tribute", 2 },
                    { 55, "At the start of each combat, gain 5 gold.", "Gain 8 gold at the start of combat.", "Gain 12 gold at the start of combat.", "Gain 15 gold at the start of combat.", "Golden Opportunity", 2 },
                    { 56, "Store prices are reduced by 5%.", "Store prices are reduced by 10%.", "Store prices are reduced by 15%.", "Store prices are reduced by 20%.", "Merchant's Favor", 2 },
                    { 57, "Gain 5 gold back after each purchase.", "Gain 10 gold back after each purchase.", "Gain 15 gold back after each purchase.", "Gain 20 gold back after each purchase.", "Refund Ritual", 2 },
                    { 58, "Gain 100 Gold.", "Gain 125 Gold.", "Gain 150 Gold.", "Gain 300 Gold.", "Golden Idol", 2 },
                    { 59, "Each time you win a combat without taking damage, gain 10 bonus gold.", "Gain 15 bonus gold for flawless victories.", "Gain 20 gold for flawless victories.", "Gain 25 gold and +1 Initiative for flawless victories.", "Glittering Strikes", 2 },
                    { 60, "Every 3 rooms, gain interest: +10% of your current gold.", "Every 2 rooms, gain +10% gold interest.", "Every 2 rooms, gain +15% interest.", "Every rooms, gain +15% gold interest.", "Investment Plan", 2 },
                    { 61, "10% chance after combat to receive a random bonus reward.", "15% chance after combat to receive a random bonus reward.", "20% chance after combat to receive a random bonus reward.", "25% chance after combat to receive a random bonus reward.", "Wanderer's Luck", 5 },
                    { 62, "When choosing cards after combat, choose from 4 options instead of 3.", "Choose from 5 options instead of 3.", "Choose from 5 options and reroll once per map.", "Choose from 6 options and reroll once per combat.", "Explorer's Curiosity", 5 },
                    { 63, "Gain 1 random temporary blessing at the start of each combat.", "Gain 2 random temporary blessings.", "Gain 2 random temporary blessings (can repeat).", "Gain 3 random temporary blessings.", "Unstable Artifact", 5 },
                    { 64, "10% chance to discover a hidden room when exploring.", "15% chance to discover a hidden room.", "20% chance to discover a hidden room and gain a random reward.", "25% chance to discover a hidden room with double rewards.", "Whispers of the Unknown", 5 },
                    { 65, "After every 3 combats, gain 1 bonus card from a random rarity.", "After every 3 combats, gain 2 bonus cards (random rarity).", "After every 3 combats, gain 2 bonus cards and 1 random blessing.", "After every 3 combats, gain 3 cards and 1 random upgraded blessing.", "Deckbuilder's Delight", 5 },
                    { 66, "Draw +1 extra card on your first turn each combat.", "Draw +2 extra cards on your first turn each combat.", "Draw +1 extra card on your first two turns each combat.", "Draw +2 extra cards on your first two turns each combat.", "Rushed Planning", 5 },
                    { 67, "Once per map, reroll your post-combat rewards.", "Twice per map, reroll post-combat rewards.", "Once per combat, reroll card choices.", "Once per combat, reroll card and gold rewards.", "Fate Twister", 5 },
                    { 68, "After every 3 combats, gain 1 random card and 10 gold.", "Gain 1 card and 20 gold.", "Gain 1 card, 30 gold, and 1 random item (if implemented).", "Gain 2 cards, 50 gold, and 1 random item (if implemented).", "Traveler's Pack", 5 },
                    { 69, "When entering a new map region, choose from 2 random blessings.", "Choose from 3 random blessings.", "Choose from 3 and keep 1 permanent and 1 temporary.", "Choose 2 permanent blessings from 4 options.", "Deal with Destiny", 5 },
                    { 70, "Each combat, the first time you draw 5 or more cards in a turn, gain +1 mana.", "Gain +1 mana and draw 1 card.", "Gain +1 mana and draw 2 cards.", "Gain +2 mana and draw 2 cards.", "Vision of Victory", 5 }
                });

            migrationBuilder.InsertData(
                table: "Cards",
                columns: new[] { "Id", "Air", "Armor", "AscendedArmor", "AscendedAttack", "AscendedDefense", "AscendedDraw", "AscendedEffectMultiplier", "AscendedEnemyTargets", "AscendedHeal", "AscendedLifeCost", "AscendedMana", "AscendedManaCost", "AscendedPlayerTargets", "AscendedPoison", "AscendedStrength", "Attack", "CharacterId", "Daggers", "Defense", "Description", "Draw", "Earth", "EffectMultiplier", "EnemyTargets", "Fire", "Heal", "LifeCost", "Mana", "ManaCost", "Momentum", "Name", "Permanent", "PlayerTargets", "Poison", "Rarity", "Spirit", "Strength", "Water" },
                values: new object[,]
                {
                    { 1, null, 0, 0, 9, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 6, 1, null, 0, "Deal {{Attack}} {{Damage_IMG}}", 0, null, 0, 1, null, 0, 0, 0, 1, 1, "Attack", false, 0, 0, 0, null, 0, null },
                    { 2, null, 5, 8, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, null, 0, "Gain {{Armor}} {{Armor_IMG}}", 0, null, 0, 0, null, 0, 0, 0, 1, null, "Defense", false, 0, 0, 0, null, 0, null },
                    { 3, null, 4, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, null, 0, "Give {{Armor}} {{Armor_IMG}} to all allies", 0, null, 0, 0, null, 0, 0, 0, 1, null, "Shield Wall", false, 0, 0, 1, null, 0, null },
                    { 7, null, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 1, null, 0, "Gain {{Mana}} {{Mana_IMG}}", 0, null, 0, 0, null, 0, 0, 1, 0, null, "Rallying Cry", false, 0, 0, 1, null, 0, null },
                    { 8, null, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0, 1, null, 0, "Deal {{Damage_IMG}} equal to your {{Armor_IMG}}", 0, null, 0, 1, null, 0, 0, 0, 2, null, "Crushing Guard", false, 0, 0, 2, null, 0, null },
                    { 9, null, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5, 0, 1, null, -2, "Gain {{Strength}} {{Strength_IMG}} and {{Defense}} {{Defense_IMG}}", 0, null, 0, 0, null, 0, 0, 0, 3, null, "Rage", false, 0, 0, 2, null, 3, null },
                    { 10, null, 7, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, null, 0, "Gain {{Armor}} {{Armor_IMG}} and draw {{Draw}} {{Card_IMG}}", 1, null, 0, 0, null, 0, 0, 0, 1, null, "Battle Ready", false, 0, 0, 1, null, 0, null },
                    { 11, null, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 1, null, 0, "Gain {{Strength}} {{Strength_IMG}} and draw {{Draw}} {{Card_IMG}}", 1, null, 0, 0, null, 0, 0, 0, 1, null, "War Cry", false, 0, 0, 1, null, 1, null },
                    { 12, null, 0, 0, 20, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 15, 1, null, 0, "Deal {{Attack}} {{Damage_IMG}}", 0, null, 0, 1, null, 0, 0, 0, 2, 2, "Heavy Strike", false, 0, 0, 1, null, 0, null },
                    { 13, null, 20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, null, 0, "Gain {{Armor}} {{Armor_IMG}}", 0, null, 0, 0, null, 0, 0, 0, 2, null, "Defensive Stance", false, 0, 0, 3, null, 0, null },
                    { 14, null, 0, 0, 0, 0, 0, 6, 1, 0, 0, 0, 0, 0, 0, 0, 4, 1, null, 0, "Deal {{Attack}} {{Damage_IMG}}. Increase {{Damage_IMG}} by {{EffectMultiplier}}", 0, null, 4, 1, null, 0, 0, 0, 1, 1, "Press the Attack", false, 0, 0, 3, null, 0, null },
                    { 15, null, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, null, 1, "All Allies gain {{Defense}} {{Defense_IMG}}", 0, null, 0, 0, null, 0, 0, 0, 2, null, "Stand Together", false, 0, 0, 3, null, 0, null },
                    { 16, null, 9, 15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 1, null, 0, "Give {{Armor}} {{Armor_IMG}} to 2 allies", 0, null, 0, 0, null, 0, 0, 0, 1, null, "Guardian Angel", false, 2, 0, 1, null, 0, null },
                    { 101, null, 0, 0, 8, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 5, 2, -1, 0, "Deal {{Attack}} {{Damage_IMG}}", 0, null, 0, 1, null, 0, 0, 0, 1, null, "Attack", false, 0, 0, 0, null, 0, null },
                    { 102, null, 4, 7, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, null, 0, "Gain {{Armor}} {{Armor_IMG}}", 0, null, 0, 0, null, 0, 0, 0, 1, null, "Defense", false, 0, 0, 0, null, 0, null },
                    { 103, null, 0, 0, 6, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 4, 2, -1, 0, "Deal {{Attack}} {{Damage_IMG}}", 0, null, 0, 1, null, 0, 0, 0, 0, null, "Quick Strike", false, 0, 0, 1, null, 0, null },
                    { 104, null, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 4, 0, 0, 2, null, 0, "Inflict {{Poison}} {{Poison_IMG}} on an enemy", 0, null, 0, 1, null, 0, 0, 0, 1, null, "Poison Blade", false, 0, 2, 1, null, 0, null },
                    { 105, null, 0, 0, 3, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 2, 2, -1, 0, "Deal {{Attack}} {{Damage_IMG}}. Exhaust.", 0, null, 0, 1, null, 0, 0, 0, 0, null, "Throwing Knife", false, 0, 0, 0, null, 0, null },
                    { 106, null, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, "Add {{EffectMultiplier}} {{Throwing_Knife}} to your hand.", 0, null, 3, 0, null, 0, 0, 0, 1, null, "Knife Dance", false, 0, 0, 1, null, 0, null },
                    { 107, null, 0, 0, 4, 0, 0, 2, 1, 0, 0, 0, 0, 0, 0, 0, 3, 2, -2, 0, "Deal {{Attack}} {{Damage_IMG}} twice.", 0, null, 2, 1, null, 0, 0, 0, 1, null, "Flurry", false, 0, 0, 1, null, 0, null },
                    { 108, null, 5, 8, 0, 0, 0, 3, 0, 0, 0, 0, 0, 1, 0, 0, 0, 2, 0, 0, "Gain {{Armor}} {{Armor_IMG}}. Add {{EffectMultiplier}} {{Throwing_Knife}} to your hand.", 0, null, 2, 0, null, 0, 0, 0, 1, null, "Smoke Bomb", false, 1, 0, 1, null, 0, null },
                    { 109, null, 0, 0, 0, 0, 2, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, "Draw {{Draw}} {{Card_IMG}}. Add {{EffectMultiplier}} {{Throwing_Knife}} to your hand.", 1, null, 2, 0, null, 0, 0, 0, 1, null, "Hidden Knife", false, 0, 0, 2, null, 0, null },
                    { 110, null, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 1, 0, 0, 0, 2, 0, 0, "Your Poison deals {{EffectMultiplier}} more damage this combat.", 0, null, 1, 0, null, 0, 0, 0, 1, null, "Deadly Venom", false, 1, 0, 2, null, 0, null },
                    { 111, null, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 5, 0, 2, 2, 0, "Gain {{Daggers}} {{Dagger_IMG}}. Gain {{Strength}} {{Strength_IMG}} this turn.", 0, null, 0, 0, null, 0, 0, 0, 1, null, "Sharpen Blades", false, 1, 0, 2, null, 3, null },
                    { 112, null, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 1, 0, 0, 0, 0, 2, 0, 0, "Add {{EffectMultiplier}} {{Throwing_Knife}} to your hand.", 0, null, 4, 0, null, 0, 0, 0, 2, null, "Bandolier", false, 0, 0, 2, null, 0, null },
                    { 113, null, 0, 0, 10, 0, 0, 1, 1, 0, 0, 0, 1, 0, 0, 0, 8, 2, -1, 0, "Deal {{Attack}} {{Damage_IMG}}. Deal extra damage equal to the target's Poison.", 0, null, 1, 1, null, 0, 0, 0, 2, null, "Venom Burst", false, 0, 0, 2, null, 0, null },
                    { 114, null, 0, 0, 26, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 18, 2, 0, 0, "Can only be played with 0 {{Dagger_IMG}}. Deal {{Attack}} {{Damage_IMG}}.", 0, null, 0, 1, null, 0, 0, 0, 1, null, "Execution", false, 0, 0, 3, null, 0, null },
                    { 115, null, 0, 0, 8, 0, 0, 2, 1, 0, 0, 0, 0, 0, 0, 0, 6, 2, -1, 0, "Deal {{Attack}} {{Damage_IMG}}. Deals double if played as your first Attack this turn.", 0, null, 2, 1, null, 0, 0, 0, 1, null, "Backstab", false, 0, 0, 1, null, 0, null },
                    { 116, null, 0, 0, 5, 0, 0, 0, 99, 0, 0, 0, 0, 0, 0, 0, 3, 2, -1, 0, "Deal {{Attack}} {{Damage_IMG}} to ALL enemies.", 0, null, 0, 99, null, 0, 0, 0, 1, null, "Fan of Knives", false, 0, 0, 1, null, 0, null },
                    { 117, null, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 2, 0, 0, 2, 0, 0, "Your attacks apply {{Poison}} {{Poison_IMG}} this turn.", 0, null, 0, 0, null, 0, 0, 0, 1, null, "Toxic Edge", false, 1, 1, 2, null, 0, null },
                    { 118, null, 0, 0, 8, 0, 0, 0, 1, 0, 0, 0, 1, 0, 4, 0, 6, 2, -1, 0, "Deal {{Attack}} {{Damage_IMG}}. Apply {{Poison}} {{Poison_IMG}}.", 0, null, 0, 1, null, 0, 0, 0, 2, null, "Serrated Blade", false, 0, 4, 2, null, 0, null },
                    { 119, null, 0, 0, 7, 0, 0, 1, 1, 0, 0, 0, 1, 0, 0, 0, 6, 2, -99, 0, "Consume all {{Dagger_IMG}}. Deal {{Attack}} {{Damage_IMG}} for each consumed.", 0, null, 1, 1, null, 0, 0, 0, 2, null, "Death Blossom", false, 0, 0, 3, null, 0, null },
                    { 120, null, 0, 0, 0, 0, 0, 6, 0, 0, 0, 0, 1, 0, 0, 0, 0, 2, 0, 0, "Add {{EffectMultiplier}} {{Throwing_Knife}} to your hand.", 0, null, 6, 0, null, 0, 0, 0, 2, null, "Blade Storm", false, 0, 0, 3, null, 0, null },
                    { 121, null, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 2, 0, 2, 0, 0, "Whenever you consume a {{Dagger_IMG}}, gain {{Strength}} {{Strength_IMG}} this turn.", 0, null, 0, 0, null, 0, 0, 0, 1, null, "Blood Frenzy", false, 1, 0, 3, null, 1, null },
                    { 122, null, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 2, 0, 0, "Whenever you trigger Poison, draw {{Draw}} {{Card_IMG}}.", 1, null, 0, 0, null, 0, 0, 0, 1, null, "Assassin's Instinct", false, 1, 0, 3, null, 0, null },
                    { 123, null, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 1, 1, 0, 0, 0, 2, 0, 0, "Whenever you trigger Poison, reset your {{Dagger_IMG}} to 3.", 0, null, 3, 0, null, 0, 0, 0, 2, null, "Master Assassin", false, 1, 0, 4, null, 0, null },
                    { 124, null, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 1, 0, 0, 0, 2, 0, 0, "Whenever you play {{Throwing_Knife}}, add another {{Throwing_Knife}} to your hand.", 0, null, 1, 0, null, 0, 0, 0, 2, null, "Endless Arsenal", false, 1, 0, 4, null, 0, null },
                    { 125, null, 0, 0, 0, 0, 0, 2, 1, 0, 0, 0, 1, 0, 0, 0, 0, 2, 0, 0, "Target enemy takes {{EffectMultiplier}}x Poison damage.", 0, null, 2, 1, null, 0, 0, 0, 2, null, "Death Mark", false, 0, 0, 4, null, 0, null },
                    { 201, null, 0, 0, 8, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 5, 3, null, 0, "Deal {{Attack}} {{Damage_IMG}}", 0, null, 0, 1, null, 0, 0, 0, 1, null, "Attack", false, 0, 0, 0, null, 0, null },
                    { 202, null, 4, 7, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, null, 0, "Gain {{Armor}} {{Armor_IMG}}", 0, null, 0, 0, null, 0, 0, 0, 1, null, "Defense", false, 0, 0, 0, null, 0, null },
                    { 203, null, 0, 0, 12, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 8, 3, null, 0, "Deal {{Attack}} {{Damage_IMG}}", 0, null, 0, 0, null, 0, 0, 0, 1, null, "Precise Shot", false, 0, 0, 1, null, 0, null },
                    { 204, null, 0, 0, 6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 3, null, 0, "Deal {{Attack}} {{Damage_IMG}} to all enemies", 0, null, 0, 0, null, 0, 0, 0, 2, null, "Volley", false, 0, 0, 2, null, 0, null },
                    { 205, null, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 3, null, 0, "Gain {{Strength}} {{Strength_IMG}}", 0, null, 0, 0, null, 0, 0, 0, 1, null, "Eagle Eye", false, 0, 0, 1, null, 2, null },
                    { 206, null, 0, 0, 20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 14, 3, null, 0, "Deal {{Attack}} {{Damage_IMG}}", 0, null, 0, 0, null, 0, 0, 0, 2, null, "Aimed Shot", false, 0, 0, 2, null, 0, null },
                    { 207, null, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 3, null, 0, "Deal {{Attack}} {{Damage_IMG}} and draw {{Draw}} {{Card_IMG}}", 1, null, 0, 0, null, 0, 0, 0, 1, null, "Hunter's Mark", false, 0, 0, 1, null, 0, null },
                    { 208, null, 9, 14, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, null, 0, "Gain {{Armor}} {{Armor_IMG}}", 0, null, 0, 0, null, 0, 0, 0, 1, null, "Evasion", false, 0, 0, 2, null, 0, null },
                    { 209, null, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 3, 3, null, 0, "Deal {{Attack}} {{Damage_IMG}} to all enemies and gain {{Strength}} {{Strength_IMG}}", 0, null, 0, 0, null, 0, 0, 0, 2, null, "Scatter Shot", false, 0, 0, 2, null, 1, null },
                    { 210, null, 0, 0, 8, 0, 0, 7, 0, 0, 0, 0, 0, 0, 0, 0, 5, 3, null, 0, "Deal {{Attack}} {{Damage_IMG}}. Increase {{Damage_IMG}} by {{EffectMultiplier}}", 0, null, 5, 0, null, 0, 0, 0, 1, null, "Killing Blow", false, 0, 0, 3, null, 0, null },
                    { 301, null, 0, 0, 7, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 4, 4, null, 0, "Deal {{Attack}} {{Damage_IMG}}", 0, null, 0, 1, null, 0, 0, 0, 1, null, "Attack", false, 0, 0, 0, null, 0, null },
                    { 302, null, 6, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, null, 0, "Gain {{Armor}} {{Armor_IMG}}", 0, null, 0, 0, null, 0, 0, 0, 1, null, "Defense", false, 0, 0, 0, null, 0, null },
                    { 303, null, 0, 0, 0, 0, 0, 0, 0, 8, 0, 0, 0, 0, 0, 0, 0, 4, null, 0, "Restore {{Heal}} {{Heal_IMG}}", 0, null, 0, 0, null, 5, 0, 0, 1, null, "Holy Light", false, 0, 0, 1, null, 0, null },
                    { 304, null, 15, 22, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, null, 0, "Gain {{Armor}} {{Armor_IMG}}", 0, null, 0, 0, null, 0, 0, 0, 2, null, "Divine Shield", false, 0, 0, 2, null, 0, null },
                    { 305, null, 0, 0, 8, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 5, 4, null, 0, "Deal {{Attack}} {{Damage_IMG}} and restore {{Heal}} {{Heal_IMG}}", 0, null, 0, 0, null, 3, 0, 0, 2, null, "Blessed Strike", false, 0, 0, 1, null, 0, null },
                    { 306, null, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 4, null, 2, "Gain {{Strength}} {{Strength_IMG}} and {{Defense}} {{Defense_IMG}}", 0, null, 0, 0, null, 0, 0, 0, 2, null, "Consecration", false, 0, 0, 2, null, 2, null },
                    { 307, null, 0, 0, 0, 0, 0, 0, 0, 20, 0, 0, 1, 0, 0, 0, 0, 4, null, 0, "Restore {{Heal}} {{Heal_IMG}}", 0, null, 0, 0, null, 15, 0, 0, 2, null, "Lay on Hands", false, 0, 0, 3, null, 0, null },
                    { 308, null, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5, 0, 4, null, 0, "Gain {{Strength}} {{Strength_IMG}}", 0, null, 0, 0, null, 0, 0, 0, 1, null, "Righteous Fury", false, 0, 0, 2, null, 3, null },
                    { 309, null, 0, 0, 0, 3, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, null, 2, "Gain {{Defense}} {{Defense_IMG}} and draw {{Draw}} {{Card_IMG}}", 1, null, 0, 0, null, 0, 0, 0, 2, null, "Sacred Oath", false, 0, 0, 3, null, 0, null },
                    { 310, null, 5, 8, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 4, null, 0, "Gain {{Armor}} {{Armor_IMG}} and {{Mana}} {{Mana_IMG}}", 0, null, 0, 0, null, 0, 0, 1, 1, null, "Aura of Valor", false, 0, 0, 1, null, 0, null }
                });

            migrationBuilder.InsertData(
                table: "Cards",
                columns: new[] { "Id", "Air", "Armor", "AscendedArmor", "AscendedAttack", "AscendedDefense", "AscendedDraw", "AscendedEffectMultiplier", "AscendedEnemyTargets", "AscendedHeal", "AscendedLifeCost", "AscendedMagicAttack", "AscendedMana", "AscendedManaCost", "AscendedPlayerTargets", "AscendedPoison", "AscendedStrength", "Attack", "CharacterId", "Daggers", "Defense", "Description", "Draw", "Earth", "EffectMultiplier", "EnemyTargets", "Fire", "Heal", "LifeCost", "MagicAttack", "Mana", "ManaCost", "Momentum", "Name", "Permanent", "PlayerTargets", "Poison", "Rarity", "Spirit", "Strength", "Water" },
                values: new object[] { 401, null, 0, 0, 0, 0, 0, 0, 1, 0, 0, 5, 0, 0, 0, 0, 0, 0, 5, null, 0, "Deal {{MagicAttack}} {{Magic_Damage_IMG}}", 0, null, 0, 1, null, 0, 0, 3, 0, 1, null, "Attack", false, 0, 0, 0, null, 0, null });

            migrationBuilder.InsertData(
                table: "Cards",
                columns: new[] { "Id", "Air", "Armor", "AscendedArmor", "AscendedAttack", "AscendedDefense", "AscendedDraw", "AscendedEffectMultiplier", "AscendedEnemyTargets", "AscendedHeal", "AscendedLifeCost", "AscendedMagicArmor", "AscendedMana", "AscendedManaCost", "AscendedPlayerTargets", "AscendedPoison", "AscendedStrength", "Attack", "CharacterId", "Daggers", "Defense", "Description", "Draw", "Earth", "EffectMultiplier", "EnemyTargets", "Fire", "Heal", "LifeCost", "MagicArmor", "Mana", "ManaCost", "Momentum", "Name", "Permanent", "PlayerTargets", "Poison", "Rarity", "Spirit", "Strength", "Water" },
                values: new object[] { 402, null, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 0, 0, 0, 0, 0, 0, 5, null, 0, "Gain {{MagicArmor}} {{Magic_Armor_IMG}}", 0, null, 0, 0, null, 0, 0, 4, 0, 1, null, "Defense", false, 0, 0, 0, null, 0, null });

            migrationBuilder.InsertData(
                table: "Cards",
                columns: new[] { "Id", "Air", "Armor", "AscendedArmor", "AscendedAttack", "AscendedDefense", "AscendedDraw", "AscendedEffectMultiplier", "AscendedEnemyTargets", "AscendedHeal", "AscendedLifeCost", "AscendedMana", "AscendedManaCost", "AscendedPlayerTargets", "AscendedPoison", "AscendedStrength", "Attack", "CharacterId", "Daggers", "Defense", "Description", "Draw", "Earth", "EffectMultiplier", "EnemyTargets", "Fire", "Heal", "LifeCost", "Mana", "ManaCost", "Momentum", "Name", "Permanent", "PlayerTargets", "Poison", "Rarity", "Spirit", "Strength", "Water" },
                values: new object[,]
                {
                    { 403, null, 0, 0, 0, 0, 0, 0, 0, 13, 0, 0, 0, 1, 0, 0, 0, 5, null, 0, "Heal an ally for {{Heal}} {{Heal_IMG}}", 0, null, 0, 0, null, 9, 0, 0, 1, null, "Healing Light", false, 1, 0, 1, null, 0, null },
                    { 404, null, 7, 11, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 5, null, 0, "Give {{Armor}} {{Armor_IMG}} to an ally", 0, null, 0, 0, null, 0, 0, 0, 1, null, "Sanctuary Ward", false, 1, 0, 1, null, 0, null },
                    { 405, null, 0, 0, 0, 3, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 5, null, 2, "Give an ally {{Defense}} {{Defense_IMG}} and draw {{Draw}} {{Card_IMG}}", 1, null, 0, 0, null, 0, 0, 0, 1, null, "Blessed Haste", false, 1, 0, 2, null, 0, null },
                    { 406, null, 0, 0, 0, 0, 0, 0, 0, 8, 0, 0, 0, 0, 0, 0, 0, 5, null, 0, "Heal all allies for {{Heal}} {{Heal_IMG}}", 0, null, 0, 0, null, 5, 0, 0, 2, null, "Renewal Prayer", false, 0, 0, 2, null, 0, null },
                    { 407, null, 0, 0, 14, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 10, 5, null, 0, "Deal {{Attack}} {{Damage_IMG}}", 0, null, 0, 1, null, 0, 0, 0, 2, null, "Holy Fire", false, 0, 0, 2, null, 0, null },
                    { 408, null, 4, 6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 3, 0, 5, null, 0, "Give an ally {{Strength}} {{Strength_IMG}} and {{Armor}} {{Armor_IMG}}", 0, null, 0, 0, null, 0, 0, 0, 1, null, "Consecrate", false, 1, 0, 2, null, 2, null },
                    { 409, null, 0, 0, 0, 3, 0, 0, 0, 19, 0, 0, 1, 1, 0, 0, 0, 5, null, 2, "Heal an ally for {{Heal}} {{Heal_IMG}} and give {{Defense}} {{Defense_IMG}}", 0, null, 0, 0, null, 14, 0, 0, 2, null, "Seraphic Grace", false, 1, 0, 3, null, 0, null },
                    { 410, null, 0, 0, 17, 0, 2, 0, 1, 9, 0, 0, 2, 0, 0, 0, 12, 5, null, 0, "Deal {{Attack}} {{Damage_IMG}}, heal all allies for {{Heal}} {{Heal_IMG}}, and draw {{Draw}} {{Card_IMG}}", 1, null, 0, 1, null, 6, 0, 0, 3, null, "Phoenix Liturgy", false, 0, 0, 3, null, 0, null }
                });

            migrationBuilder.InsertData(
                table: "Cards",
                columns: new[] { "Id", "Air", "Armor", "AscendedArmor", "AscendedAttack", "AscendedDefense", "AscendedDraw", "AscendedEffectMultiplier", "AscendedEnemyTargets", "AscendedHeal", "AscendedLifeCost", "AscendedMagicAttack", "AscendedMana", "AscendedManaCost", "AscendedPlayerTargets", "AscendedPoison", "AscendedStrength", "Attack", "CharacterId", "Daggers", "Defense", "Description", "Draw", "Earth", "EffectMultiplier", "EnemyTargets", "Fire", "Heal", "LifeCost", "MagicAttack", "Mana", "ManaCost", "Momentum", "Name", "Permanent", "PlayerTargets", "Poison", "Rarity", "Spirit", "Strength", "Water" },
                values: new object[] { 501, null, 0, 0, 0, 0, 0, 0, 1, 0, 0, 12, 0, 0, 0, 0, 0, 0, 6, null, 0, "Deal {{MagicAttack}} {{Magic_Damage_IMG}}", 0, null, 0, 1, null, 0, 0, 8, 0, 1, null, "Attack", false, 0, 0, 0, null, 0, null });

            migrationBuilder.InsertData(
                table: "Cards",
                columns: new[] { "Id", "Air", "Armor", "AscendedArmor", "AscendedAttack", "AscendedDefense", "AscendedDraw", "AscendedEffectMultiplier", "AscendedEnemyTargets", "AscendedHeal", "AscendedLifeCost", "AscendedMagicArmor", "AscendedMana", "AscendedManaCost", "AscendedPlayerTargets", "AscendedPoison", "AscendedStrength", "Attack", "CharacterId", "Daggers", "Defense", "Description", "Draw", "Earth", "EffectMultiplier", "EnemyTargets", "Fire", "Heal", "LifeCost", "MagicArmor", "Mana", "ManaCost", "Momentum", "Name", "Permanent", "PlayerTargets", "Poison", "Rarity", "Spirit", "Strength", "Water" },
                values: new object[] { 502, null, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 6, null, 0, "Gain {{MagicArmor}} {{Magic_Armor_IMG}}", 0, null, 0, 0, null, 0, 0, 3, 0, 1, null, "Defense", false, 0, 0, 0, null, 0, null });

            migrationBuilder.InsertData(
                table: "Cards",
                columns: new[] { "Id", "Air", "Armor", "AscendedArmor", "AscendedAttack", "AscendedDefense", "AscendedDraw", "AscendedEffectMultiplier", "AscendedEnemyTargets", "AscendedHeal", "AscendedLifeCost", "AscendedMana", "AscendedManaCost", "AscendedPlayerTargets", "AscendedPoison", "AscendedStrength", "Attack", "CharacterId", "Daggers", "Defense", "Description", "Draw", "Earth", "EffectMultiplier", "EnemyTargets", "Fire", "Heal", "LifeCost", "Mana", "ManaCost", "Momentum", "Name", "Permanent", "PlayerTargets", "Poison", "Rarity", "Spirit", "Strength", "Water" },
                values: new object[,]
                {
                    { 503, null, 0, 0, 15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 10, 6, null, 0, "Deal {{Attack}} {{Damage_IMG}}", 0, null, 0, 0, null, 0, 0, 0, 2, null, "Fireball", false, 0, 0, 1, null, 0, null },
                    { 504, null, 0, 0, 0, 0, 0, 0, 0, 0, 3, 3, 0, 0, 0, 0, 0, 6, null, 0, "Sacrifice {{LifeCost}} {{Life_IMG}} to gain {{Mana}} {{Mana_IMG}}", 0, null, 0, 0, null, 0, 5, 2, 0, null, "Blood Pact", false, 0, 0, 2, null, 0, null },
                    { 505, null, 0, 0, 9, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 6, null, 0, "Deal {{Attack}} {{Damage_IMG}} to all enemies", 0, null, 0, 0, null, 0, 0, 0, 2, null, "Blizzard", false, 0, 0, 1, null, 0, null },
                    { 506, null, 0, 0, 20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 14, 6, null, 0, "Deal {{Attack}} {{Damage_IMG}}", 0, null, 0, 0, null, 0, 0, 0, 2, null, "Thunder", false, 0, 0, 2, null, 0, null },
                    { 507, null, 0, 0, 9, 0, 0, 0, 0, 9, 0, 0, 0, 0, 0, 0, 6, 6, null, 0, "Deal {{Attack}} {{Damage_IMG}} and restore {{Heal}} {{Heal_IMG}}", 0, null, 0, 0, null, 6, 0, 0, 2, null, "Drain Life", false, 0, 0, 2, null, 0, null },
                    { 508, null, 0, 0, 0, 0, 0, 0, 0, 0, 7, 0, 0, 0, 0, 5, 0, 6, null, 0, "Sacrifice {{LifeCost}} {{Life_IMG}} to gain {{Strength}} {{Strength_IMG}}", 0, null, 0, 0, null, 0, 10, 0, 0, null, "Dark Arts", false, 0, 0, 3, null, 3, null },
                    { 509, null, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 6, null, 0, "Gain {{Mana}} {{Mana_IMG}}", 0, null, 0, 0, null, 0, 0, 2, 0, null, "Arcane Surge", false, 0, 0, 2, null, 0, null },
                    { 510, null, 0, 0, 28, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 20, 6, null, 0, "Deal {{Attack}} {{Damage_IMG}}", 0, null, 0, 0, null, 0, 0, 0, 3, null, "Meteor", false, 0, 0, 3, null, 0, null }
                });

            migrationBuilder.InsertData(
                table: "Cards",
                columns: new[] { "Id", "Air", "Armor", "AscendedArmor", "AscendedAttack", "AscendedDefense", "AscendedDraw", "AscendedEffectMultiplier", "AscendedEnemyTargets", "AscendedHeal", "AscendedLifeCost", "AscendedMagicAttack", "AscendedMana", "AscendedManaCost", "AscendedPlayerTargets", "AscendedPoison", "AscendedStrength", "Attack", "CharacterId", "Daggers", "Defense", "Description", "Draw", "Earth", "EffectMultiplier", "EnemyTargets", "Fire", "Heal", "LifeCost", "MagicAttack", "Mana", "ManaCost", "Momentum", "Name", "Permanent", "PlayerTargets", "Poison", "Rarity", "Spirit", "Strength", "Water" },
                values: new object[] { 601, null, 0, 0, 0, 0, 0, 0, 1, 0, 0, 8, 0, 0, 0, 0, 0, 0, 7, null, 0, "Deal {{MagicAttack}} {{Magic_Damage_IMG}}", 0, null, 0, 1, null, 0, 0, 5, 0, 1, null, "Attack", false, 0, 0, 0, null, 0, null });

            migrationBuilder.InsertData(
                table: "Cards",
                columns: new[] { "Id", "Air", "Armor", "AscendedArmor", "AscendedAttack", "AscendedDefense", "AscendedDraw", "AscendedEffectMultiplier", "AscendedEnemyTargets", "AscendedHeal", "AscendedLifeCost", "AscendedMagicArmor", "AscendedMana", "AscendedManaCost", "AscendedPlayerTargets", "AscendedPoison", "AscendedStrength", "Attack", "CharacterId", "Daggers", "Defense", "Description", "Draw", "Earth", "EffectMultiplier", "EnemyTargets", "Fire", "Heal", "LifeCost", "MagicArmor", "Mana", "ManaCost", "Momentum", "Name", "Permanent", "PlayerTargets", "Poison", "Rarity", "Spirit", "Strength", "Water" },
                values: new object[] { 602, null, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 0, 0, 0, 0, 0, 0, 7, null, 0, "Gain {{MagicArmor}} {{Magic_Armor_IMG}}", 0, null, 0, 0, null, 0, 0, 4, 0, 1, null, "Defense", false, 0, 0, 0, null, 0, null });

            migrationBuilder.InsertData(
                table: "Cards",
                columns: new[] { "Id", "Air", "Armor", "AscendedArmor", "AscendedAttack", "AscendedDefense", "AscendedDraw", "AscendedEffectMultiplier", "AscendedEnemyTargets", "AscendedHeal", "AscendedLifeCost", "AscendedMana", "AscendedManaCost", "AscendedPlayerTargets", "AscendedPoison", "AscendedStrength", "Attack", "CharacterId", "Daggers", "Defense", "Description", "Draw", "Earth", "EffectMultiplier", "EnemyTargets", "Fire", "Heal", "LifeCost", "Mana", "ManaCost", "Momentum", "Name", "Permanent", "PlayerTargets", "Poison", "Rarity", "Spirit", "Strength", "Water" },
                values: new object[] { 603, null, 10, 15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7, null, 0, "Gain {{Armor}} {{Armor_IMG}}", 0, null, 0, 0, null, 0, 0, 0, 1, null, "Stone Skin", false, 0, 0, 1, null, 0, null });

            migrationBuilder.InsertData(
                table: "Cards",
                columns: new[] { "Id", "Air", "Armor", "AscendedArmor", "AscendedAttack", "AscendedDefense", "AscendedDraw", "AscendedEffectMultiplier", "AscendedEnemyTargets", "AscendedHeal", "AscendedLifeCost", "AscendedMagicAttack", "AscendedMana", "AscendedManaCost", "AscendedPlayerTargets", "AscendedPoison", "AscendedStrength", "Attack", "CharacterId", "Daggers", "Defense", "Description", "Draw", "Earth", "EffectMultiplier", "EnemyTargets", "Fire", "Heal", "LifeCost", "MagicAttack", "Mana", "ManaCost", "Momentum", "Name", "Permanent", "PlayerTargets", "Poison", "Rarity", "Spirit", "Strength", "Water" },
                values: new object[,]
                {
                    { 604, false, 0, 0, 0, 0, 0, 0, 1, 0, 0, 6, 0, 0, 0, 0, 0, 0, 7, null, 0, "Deal {{MagicAttack}} {{Magic_Damage_IMG}}", 0, false, 0, 1, true, 0, 0, 4, 0, 1, null, "Flame Spark", false, 0, 0, 0, null, 0, false },
                    { 605, false, 0, 0, 0, 0, 0, 0, 1, 0, 0, 9, 0, 0, 0, 0, 0, 0, 7, null, 0, "Deal {{MagicAttack}} {{Magic_Damage_IMG}}", 0, false, 0, 1, true, 0, 0, 10, 0, 2, null, "Flame Burst", false, 0, 0, 1, null, 0, false }
                });

            migrationBuilder.InsertData(
                table: "Cards",
                columns: new[] { "Id", "Air", "Armor", "AscendedArmor", "AscendedAttack", "AscendedDefense", "AscendedDraw", "AscendedEffectMultiplier", "AscendedEnemyTargets", "AscendedHeal", "AscendedLifeCost", "AscendedMagicArmor", "AscendedMana", "AscendedManaCost", "AscendedPlayerTargets", "AscendedPoison", "AscendedStrength", "Attack", "CharacterId", "Daggers", "Defense", "Description", "Draw", "Earth", "EffectMultiplier", "EnemyTargets", "Fire", "Heal", "LifeCost", "MagicArmor", "Mana", "ManaCost", "Momentum", "Name", "Permanent", "PlayerTargets", "Poison", "Rarity", "Spirit", "Strength", "Water" },
                values: new object[] { 606, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 8, 0, 0, 0, 0, 0, 0, 7, null, 0, "Gain {{MagicArmor}} {{Magic_Armor_IMG}}", 0, false, 0, 0, true, 0, 0, 5, 0, 1, null, "Ember Shield", false, 0, 0, 1, null, 0, false });

            migrationBuilder.InsertData(
                table: "Cards",
                columns: new[] { "Id", "Air", "Armor", "AscendedArmor", "AscendedAttack", "AscendedDefense", "AscendedDraw", "AscendedEffectMultiplier", "AscendedEnemyTargets", "AscendedHeal", "AscendedLifeCost", "AscendedMagicAttack", "AscendedMana", "AscendedManaCost", "AscendedPlayerTargets", "AscendedPoison", "AscendedStrength", "Attack", "CharacterId", "Daggers", "Defense", "Description", "Draw", "Earth", "EffectMultiplier", "EnemyTargets", "Fire", "Heal", "LifeCost", "MagicAttack", "Mana", "ManaCost", "Momentum", "Name", "Permanent", "PlayerTargets", "Poison", "Rarity", "Spirit", "Strength", "Water" },
                values: new object[] { 607, false, 0, 0, 0, 0, 0, 0, 1, 0, 0, 6, 0, 0, 0, 0, 0, 0, 7, null, 0, "Deal {{MagicAttack}} {{Magic_Damage_IMG}}", 0, false, 0, 1, false, 0, 0, 4, 0, 1, null, "Water Whip", null, 0, 0, 0, null, 0, true });

            migrationBuilder.InsertData(
                table: "Cards",
                columns: new[] { "Id", "Air", "Armor", "AscendedArmor", "AscendedAttack", "AscendedDefense", "AscendedDraw", "AscendedEffectMultiplier", "AscendedEnemyTargets", "AscendedHeal", "AscendedLifeCost", "AscendedMagicArmor", "AscendedMana", "AscendedManaCost", "AscendedPlayerTargets", "AscendedPoison", "AscendedStrength", "Attack", "CharacterId", "Daggers", "Defense", "Description", "Draw", "Earth", "EffectMultiplier", "EnemyTargets", "Fire", "Heal", "LifeCost", "MagicArmor", "Mana", "ManaCost", "Momentum", "Name", "Permanent", "PlayerTargets", "Poison", "Rarity", "Spirit", "Strength", "Water" },
                values: new object[] { 608, false, 2, 4, 0, 0, 0, 0, 0, 0, 0, 7, 0, 0, 0, 0, 0, 0, 7, null, 0, "Gain {{MagicArmor}} {{Magic_Armor_IMG}}{{NL}}{{Water}}+{{Earth}}: Gain {{Armor}} {{Armor_IMG}}", 0, false, 0, 0, false, 0, 0, 5, 0, 1, null, "Tidal Guard", null, 0, 0, 1, null, 0, true });

            migrationBuilder.InsertData(
                table: "Cards",
                columns: new[] { "Id", "Air", "Armor", "AscendedArmor", "AscendedAttack", "AscendedDefense", "AscendedDraw", "AscendedEffectMultiplier", "AscendedEnemyTargets", "AscendedHeal", "AscendedLifeCost", "AscendedMana", "AscendedManaCost", "AscendedPlayerTargets", "AscendedPoison", "AscendedStrength", "Attack", "CharacterId", "Daggers", "Defense", "Description", "Draw", "Earth", "EffectMultiplier", "EnemyTargets", "Fire", "Heal", "LifeCost", "Mana", "ManaCost", "Momentum", "Name", "Permanent", "PlayerTargets", "Poison", "Rarity", "Spirit", "Strength", "Water" },
                values: new object[] { 609, false, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 1, 0, 0, 0, 7, null, 0, "Heal {{Heal}} {{Heal_IMG}}", 0, false, 0, 0, false, 3, 0, 0, 1, null, "Healing Rain", null, 1, 0, 1, null, 0, true });

            migrationBuilder.InsertData(
                table: "Cards",
                columns: new[] { "Id", "Air", "Armor", "AscendedArmor", "AscendedAttack", "AscendedDefense", "AscendedDraw", "AscendedEffectMultiplier", "AscendedEnemyTargets", "AscendedHeal", "AscendedLifeCost", "AscendedMagicAttack", "AscendedMana", "AscendedManaCost", "AscendedPlayerTargets", "AscendedPoison", "AscendedStrength", "Attack", "CharacterId", "Daggers", "Defense", "Description", "Draw", "Earth", "EffectMultiplier", "EnemyTargets", "Fire", "Heal", "LifeCost", "MagicAttack", "Mana", "ManaCost", "Momentum", "Name", "Permanent", "PlayerTargets", "Poison", "Rarity", "Spirit", "Strength", "Water" },
                values: new object[] { 610, false, 0, 0, 0, 0, 1, 0, 1, 0, 0, 7, 0, 0, 0, 0, 0, 0, 7, null, 0, "Deal {{MagicAttack}} {{Magic_Damage_IMG}}{{NL}}{{Air}}: Draw {{Draw}} {{Card_IMG}}", 1, true, 0, 1, false, 0, 0, 5, 0, 1, null, "Rock Spike", null, 0, 0, 1, null, 0, false });

            migrationBuilder.InsertData(
                table: "Cards",
                columns: new[] { "Id", "Air", "Armor", "AscendedArmor", "AscendedAttack", "AscendedDefense", "AscendedDraw", "AscendedEffectMultiplier", "AscendedEnemyTargets", "AscendedHeal", "AscendedLifeCost", "AscendedMana", "AscendedManaCost", "AscendedPlayerTargets", "AscendedPoison", "AscendedStrength", "Attack", "CharacterId", "Daggers", "Defense", "Description", "Draw", "Earth", "EffectMultiplier", "EnemyTargets", "Fire", "Heal", "LifeCost", "Mana", "ManaCost", "Momentum", "Name", "Permanent", "PlayerTargets", "Poison", "Rarity", "Spirit", "Strength", "Water" },
                values: new object[] { 611, false, 8, 12, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7, null, 0, "Gain {{Armor}} {{Armor_IMG}}", 0, true, 0, 0, false, 0, 0, 0, 1, null, "Grounded", null, 0, 0, 1, null, 0, false });

            migrationBuilder.InsertData(
                table: "Cards",
                columns: new[] { "Id", "Air", "Armor", "AscendedArmor", "AscendedAttack", "AscendedDefense", "AscendedDraw", "AscendedEffectMultiplier", "AscendedEnemyTargets", "AscendedHeal", "AscendedLifeCost", "AscendedMagicArmor", "AscendedMana", "AscendedManaCost", "AscendedPlayerTargets", "AscendedPoison", "AscendedStrength", "Attack", "CharacterId", "Daggers", "Defense", "Description", "Draw", "Earth", "EffectMultiplier", "EnemyTargets", "Fire", "Heal", "LifeCost", "MagicArmor", "Mana", "ManaCost", "Momentum", "Name", "Permanent", "PlayerTargets", "Poison", "Rarity", "Spirit", "Strength", "Water" },
                values: new object[] { 612, false, 8, 12, 0, 0, 0, 0, 0, 0, 0, 6, 0, 0, 0, 0, 0, 0, 7, null, 0, "Gain {{Armor}} {{Armor_IMG}}{{NL}}{{Water}}: Gain {{MagicArmor}} {{Magic_Armor_IMG}}", 0, true, 0, 0, false, 0, 0, 4, 0, 1, null, "Mud Shield", null, 0, 0, 2, null, 0, false });

            migrationBuilder.InsertData(
                table: "Cards",
                columns: new[] { "Id", "Air", "Armor", "AscendedArmor", "AscendedAttack", "AscendedDefense", "AscendedDraw", "AscendedEffectMultiplier", "AscendedEnemyTargets", "AscendedHeal", "AscendedLifeCost", "AscendedMagicAttack", "AscendedMana", "AscendedManaCost", "AscendedPlayerTargets", "AscendedPoison", "AscendedStrength", "Attack", "CharacterId", "Daggers", "Defense", "Description", "Draw", "Earth", "EffectMultiplier", "EnemyTargets", "Fire", "Heal", "LifeCost", "MagicAttack", "Mana", "ManaCost", "Momentum", "Name", "Permanent", "PlayerTargets", "Poison", "Rarity", "Spirit", "Strength", "Water" },
                values: new object[] { 613, true, 0, 0, 0, 0, 0, 0, 1, 0, 0, 9, 0, 0, 0, 0, 0, 0, 7, null, 0, "Deal {{MagicAttack}} {{Magic_Damage_IMG}}", 0, false, 0, 1, false, 0, 0, 6, 0, 1, null, "Arcane Blast", false, 0, 0, 1, null, 0, false });

            migrationBuilder.InsertData(
                table: "Cards",
                columns: new[] { "Id", "Air", "Armor", "AscendedArmor", "AscendedAttack", "AscendedDefense", "AscendedDraw", "AscendedEffectMultiplier", "AscendedEnemyTargets", "AscendedHeal", "AscendedLifeCost", "AscendedMana", "AscendedManaCost", "AscendedPlayerTargets", "AscendedPoison", "AscendedStrength", "Attack", "CharacterId", "Daggers", "Defense", "Description", "Draw", "Earth", "EffectMultiplier", "EnemyTargets", "Fire", "Heal", "LifeCost", "Mana", "ManaCost", "Momentum", "Name", "Permanent", "PlayerTargets", "Poison", "Rarity", "Spirit", "Strength", "Water" },
                values: new object[,]
                {
                    { 614, true, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7, null, 0, "Draw {{Draw}} {{Card_IMG}}", 1, false, 0, 0, false, 0, 0, 0, 0, null, "Gust Step", false, 0, 0, 1, null, 0, false },
                    { 615, true, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 7, null, 0, "Gain {{Mana}} {{Mana_IMG}}", 0, false, 0, 0, false, 0, 0, 1, 0, null, "Static Charge", false, 0, 0, 1, null, 0, false }
                });

            migrationBuilder.InsertData(
                table: "Cards",
                columns: new[] { "Id", "Air", "Armor", "AscendedArmor", "AscendedAttack", "AscendedDefense", "AscendedDraw", "AscendedEffectMultiplier", "AscendedEnemyTargets", "AscendedHeal", "AscendedLifeCost", "AscendedMagicAttack", "AscendedMana", "AscendedManaCost", "AscendedPlayerTargets", "AscendedPoison", "AscendedStrength", "Attack", "CharacterId", "Daggers", "Defense", "Description", "Draw", "Earth", "EffectMultiplier", "EnemyTargets", "Fire", "Heal", "LifeCost", "MagicAttack", "Mana", "ManaCost", "Momentum", "Name", "Permanent", "PlayerTargets", "Poison", "Rarity", "Spirit", "Strength", "Water" },
                values: new object[] { 616, false, 0, 0, 0, 0, 0, 0, 1, 0, 0, 9, 0, 0, 0, 0, 0, 0, 7, null, 0, "Deal {{MagicAttack}} {{Magic_Damage_IMG}}{{NL}}{{Fire}}+{{Water}}: Deal {{MagicAttack}} {{Magic_Damage_IMG}} extra", 0, false, 0, 1, false, 0, 0, 6, 0, 1, null, "Steam Burst", null, 0, 0, 1, null, 0, false });

            migrationBuilder.InsertData(
                table: "Cards",
                columns: new[] { "Id", "Air", "Armor", "AscendedArmor", "AscendedAttack", "AscendedDefense", "AscendedDraw", "AscendedEffectMultiplier", "AscendedEnemyTargets", "AscendedHeal", "AscendedLifeCost", "AscendedMagicArmor", "AscendedMana", "AscendedManaCost", "AscendedPlayerTargets", "AscendedPoison", "AscendedStrength", "Attack", "CharacterId", "Daggers", "Defense", "Description", "Draw", "Earth", "EffectMultiplier", "EnemyTargets", "Fire", "Heal", "LifeCost", "MagicArmor", "Mana", "ManaCost", "Momentum", "Name", "Permanent", "PlayerTargets", "Poison", "Rarity", "Spirit", "Strength", "Water" },
                values: new object[] { 617, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 8, 0, 0, 0, 0, 0, 0, 7, null, 0, "Gain {{MagicArmor}} {{Magic_Armor_IMG}}{{NL}}{{Earth}}+{{Air}}: Deal 2x Damage", 0, false, 0, 0, false, 0, 0, 5, 0, 1, null, "Lightning Rod", null, 0, 0, 1, null, 0, false });

            migrationBuilder.InsertData(
                table: "Cards",
                columns: new[] { "Id", "Air", "Armor", "AscendedArmor", "AscendedAttack", "AscendedDefense", "AscendedDraw", "AscendedEffectMultiplier", "AscendedEnemyTargets", "AscendedHeal", "AscendedLifeCost", "AscendedMagicAttack", "AscendedMana", "AscendedManaCost", "AscendedPlayerTargets", "AscendedPoison", "AscendedStrength", "Attack", "CharacterId", "Daggers", "Defense", "Description", "Draw", "Earth", "EffectMultiplier", "EnemyTargets", "Fire", "Heal", "LifeCost", "MagicAttack", "Mana", "ManaCost", "Momentum", "Name", "Permanent", "PlayerTargets", "Poison", "Rarity", "Spirit", "Strength", "Water" },
                values: new object[,]
                {
                    { 618, false, 0, 0, 0, 0, 0, 0, 10, 0, 0, 7, 0, 1, 0, 0, 0, 0, 7, null, 0, "Deal {{MagicAttack}} {{Magic_Damage_IMG}} to all enemies{{NL}}{{Fire}}+{{Air}}: Deal {{MagicAttack}} extra {{Magic_Damage_IMG}}", 0, false, 0, 10, false, 0, 0, 5, 0, 2, null, "Wildfire", null, 0, 0, 2, null, 0, false },
                    { 619, false, 0, 0, 0, 0, 0, 0, 10, 0, 0, 8, 0, 1, 0, 0, 0, 0, 7, null, 0, "Deal {{MagicAttack}} {{Magic_Damage_IMG}} to all enemies{{NL}}{{Fire}}+{{Water}}: Repeat this attack", 0, false, 0, 10, false, 0, 0, 6, 0, 2, null, "Boiling Wave", null, 0, 0, 2, null, 0, false }
                });

            migrationBuilder.InsertData(
                table: "Cards",
                columns: new[] { "Id", "Air", "Armor", "AscendedArmor", "AscendedAttack", "AscendedDefense", "AscendedDraw", "AscendedEffectMultiplier", "AscendedEnemyTargets", "AscendedHeal", "AscendedLifeCost", "AscendedMana", "AscendedManaCost", "AscendedPlayerTargets", "AscendedPoison", "AscendedStrength", "Attack", "CharacterId", "Daggers", "Defense", "Description", "Draw", "Earth", "EffectMultiplier", "EnemyTargets", "Fire", "Heal", "LifeCost", "Mana", "ManaCost", "Momentum", "Name", "Permanent", "PlayerTargets", "Poison", "Rarity", "Spirit", "Strength", "Water" },
                values: new object[,]
                {
                    { 620, false, 0, 0, 0, 0, 3, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 7, null, 0, "Draw {{Draw}} {{Card_IMG}}{{NL}}{{Fire}} + {{Water}} + {{Earth}} + {{Air}}: Gain {{Mana}} {{Mana_IMG}}", 2, false, 0, 0, false, 0, 0, 1, 1, null, "Elemental Surge", false, 0, 0, 2, null, 0, false },
                    { 621, false, 5, 8, 0, 0, 2, 0, 0, 4, 0, 1, 0, 1, 0, 0, 0, 7, null, 0, "Gain {{Armor}} {{Armor_IMG}}{{NL}}Draw {{Draw}} {{Card_IMG}}{{NL}}{{Fire}} + {{Water}} + {{Earth}} + {{Air}}: Heal {{Heal}} {{Heal_IMG}}", 1, false, 0, 0, false, 2, 0, 0, 1, null, "Perfect Balance", false, 1, 0, 2, null, 0, false }
                });

            migrationBuilder.InsertData(
                table: "Cards",
                columns: new[] { "Id", "Air", "Armor", "AscendedArmor", "AscendedAttack", "AscendedDefense", "AscendedDraw", "AscendedEffectMultiplier", "AscendedEnemyTargets", "AscendedHeal", "AscendedLifeCost", "AscendedMagicAttack", "AscendedMana", "AscendedManaCost", "AscendedPlayerTargets", "AscendedPoison", "AscendedStrength", "Attack", "CharacterId", "Daggers", "Defense", "Description", "Draw", "Earth", "EffectMultiplier", "EnemyTargets", "Fire", "Heal", "LifeCost", "MagicAttack", "Mana", "ManaCost", "Momentum", "Name", "Permanent", "PlayerTargets", "Poison", "Rarity", "Spirit", "Strength", "Water" },
                values: new object[,]
                {
                    { 622, false, 0, 0, 0, 0, 0, 0, 1, 0, 0, 16, 0, 1, 0, 0, 0, 0, 7, null, 0, "Deal {{MagicAttack}} {{Magic_Damage_IMG}}{{NL}}{{Fire}} + {{Water}} + {{Earth}} + {{Air}}: Hit all enemies", 0, false, 0, 1, false, 0, 0, 12, 0, 2, null, "Primal Detonation", false, 0, 0, 2, null, 0, false },
                    { 623, false, 0, 0, 0, 0, 0, 0, 10, 0, 0, 11, 0, 1, 0, 0, 0, 0, 7, null, 0, "Deal {{MagicAttack}} {{Magic_Damage_IMG}} to all enemies{{NL}}{{Fire}} + {{Water}} + {{Earth}} + {{Air}}: Repeat this attack", 0, false, 0, 10, false, 0, 0, 8, 0, 2, null, "Elemental Nova", false, 0, 0, 2, null, 0, false },
                    { 624, false, 0, 0, 0, 0, 0, 0, 10, 0, 0, 22, 0, 2, 0, 0, 0, 0, 7, null, 0, "Deal {{MagicAttack}} {{Magic_Damage_IMG}} to all enemies{{NL}}{{Fire}} + {{Water}} + {{Earth}} + {{Air}}: Deal {{MagicAttack}} extra {{Magic_Damage_IMG}}", 0, false, 0, 10, false, 0, 0, 16, 0, 3, null, "Cataclysm", false, 0, 0, 3, null, 0, false }
                });

            migrationBuilder.InsertData(
                table: "Cards",
                columns: new[] { "Id", "Air", "Armor", "AscendedArmor", "AscendedAttack", "AscendedDefense", "AscendedDraw", "AscendedEffectMultiplier", "AscendedEnemyTargets", "AscendedHeal", "AscendedLifeCost", "AscendedMana", "AscendedManaCost", "AscendedPlayerTargets", "AscendedPoison", "AscendedStrength", "Attack", "CharacterId", "Daggers", "Defense", "Description", "Draw", "Earth", "EffectMultiplier", "EnemyTargets", "Fire", "Heal", "LifeCost", "Mana", "ManaCost", "Momentum", "Name", "Permanent", "PlayerTargets", "Poison", "Rarity", "Spirit", "Strength", "Water" },
                values: new object[,]
                {
                    { 625, false, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7, null, 1, "Whenever you gain essence,{{NL}}gain {{Defense}} {{Defense_IMG}}", 0, false, 0, 0, false, 0, 0, 0, 1, null, "Elemental Rhythm", false, 0, 0, 2, null, 0, false },
                    { 626, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 1, 0, 0, 0, 0, 7, null, 0, "When you gain your 4th essence,{{NL}}gain {{Mana}} {{Mana_IMG}}", 0, false, 0, 0, false, 0, 0, 1, 2, null, "Essence Engine", false, 0, 0, 3, null, 0, false },
                    { 627, false, 0, 0, 0, 0, 2, 0, 0, 6, 0, 1, 1, 1, 0, 0, 0, 7, null, 0, "Heal {{Heal}} {{Heal_IMG}}{{NL}}Draw {{Draw}} {{Card_IMG}}{{NL}}{{Fire}} + {{Water}} + {{Earth}} + {{Air}}: Gain {{Mana}} {{Mana_IMG}}", 1, false, 0, 0, false, 4, 0, 1, 2, null, "Balanced Soul", false, 1, 0, 3, null, 0, false }
                });

            migrationBuilder.InsertData(
                table: "Cards",
                columns: new[] { "Id", "Air", "Armor", "AscendedArmor", "AscendedAttack", "AscendedDefense", "AscendedDraw", "AscendedEffectMultiplier", "AscendedEnemyTargets", "AscendedHeal", "AscendedLifeCost", "AscendedMagicAttack", "AscendedMana", "AscendedManaCost", "AscendedPlayerTargets", "AscendedPoison", "AscendedStrength", "Attack", "CharacterId", "Daggers", "Defense", "Description", "Draw", "Earth", "EffectMultiplier", "EnemyTargets", "Fire", "Heal", "LifeCost", "MagicAttack", "Mana", "ManaCost", "Momentum", "Name", "Permanent", "PlayerTargets", "Poison", "Rarity", "Spirit", "Strength", "Water" },
                values: new object[] { 701, null, 0, 0, 0, 0, 0, 0, 1, 0, 0, 7, 0, 0, 0, 0, 0, 0, 8, null, 0, "Deal {{MagicAttack}} {{Magic_Damage_IMG}}", 0, null, 0, 1, null, 0, 0, 4, 0, 1, null, "Attack", false, 0, 0, 0, null, 0, null });

            migrationBuilder.InsertData(
                table: "Cards",
                columns: new[] { "Id", "Air", "Armor", "AscendedArmor", "AscendedAttack", "AscendedDefense", "AscendedDraw", "AscendedEffectMultiplier", "AscendedEnemyTargets", "AscendedHeal", "AscendedLifeCost", "AscendedMagicArmor", "AscendedMana", "AscendedManaCost", "AscendedPlayerTargets", "AscendedPoison", "AscendedStrength", "Attack", "CharacterId", "Daggers", "Defense", "Description", "Draw", "Earth", "EffectMultiplier", "EnemyTargets", "Fire", "Heal", "LifeCost", "MagicArmor", "Mana", "ManaCost", "Momentum", "Name", "Permanent", "PlayerTargets", "Poison", "Rarity", "Spirit", "Strength", "Water" },
                values: new object[] { 702, null, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7, 0, 0, 0, 0, 0, 0, 8, null, 0, "Gain {{MagicArmor}} {{Magic_Armor_IMG}}", 0, null, 0, 0, null, 0, 0, 4, 0, 1, null, "Defense", false, 0, 0, 0, null, 0, null });

            migrationBuilder.InsertData(
                table: "Cards",
                columns: new[] { "Id", "Air", "Armor", "AscendedArmor", "AscendedAttack", "AscendedDefense", "AscendedDraw", "AscendedEffectMultiplier", "AscendedEnemyTargets", "AscendedHeal", "AscendedLifeCost", "AscendedMana", "AscendedManaCost", "AscendedPlayerTargets", "AscendedPoison", "AscendedStrength", "Attack", "CharacterId", "Daggers", "Defense", "Description", "Draw", "Earth", "EffectMultiplier", "EnemyTargets", "Fire", "Heal", "LifeCost", "Mana", "ManaCost", "Momentum", "Name", "Permanent", "PlayerTargets", "Poison", "Rarity", "Spirit", "Strength", "Water" },
                values: new object[,]
                {
                    { 703, null, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 3, 0, 8, null, 0, "Give an ally {{Strength}} {{Strength_IMG}}", 0, null, 0, 0, null, 0, 0, 0, 1, null, "Nature's Gift", null, 1, 0, 1, null, 2, null },
                    { 704, null, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 5, 0, 0, 8, null, 0, "Inflict {{Poison}} {{Poison_IMG}}", 0, null, 0, 1, null, 0, 0, 0, 1, null, "Entangle", null, 0, 3, 2, null, 0, null }
                });

            migrationBuilder.InsertData(
                table: "Cards",
                columns: new[] { "Id", "Air", "Armor", "AscendedArmor", "AscendedAttack", "AscendedDefense", "AscendedDraw", "AscendedEffectMultiplier", "AscendedEnemyTargets", "AscendedHeal", "AscendedLifeCost", "AscendedMagicArmor", "AscendedMana", "AscendedManaCost", "AscendedPlayerTargets", "AscendedPoison", "AscendedStrength", "Attack", "CharacterId", "Daggers", "Defense", "Description", "Draw", "Earth", "EffectMultiplier", "EnemyTargets", "Fire", "Heal", "LifeCost", "MagicArmor", "Mana", "ManaCost", "Momentum", "Name", "Permanent", "PlayerTargets", "Poison", "Rarity", "Spirit", "Strength", "Water" },
                values: new object[] { 705, null, 0, 0, 0, 0, 0, 0, 0, 0, 0, 12, 0, 0, 0, 0, 0, 0, 8, null, 0, "Gain {{MagicArmor}} {{Magic_Armor_IMG}}", 0, null, 0, 0, null, 0, 0, 8, 0, 1, null, "Barkskin", null, 0, 0, 1, null, 0, null });

            migrationBuilder.InsertData(
                table: "Cards",
                columns: new[] { "Id", "Air", "Armor", "AscendedArmor", "AscendedAttack", "AscendedDefense", "AscendedDraw", "AscendedEffectMultiplier", "AscendedEnemyTargets", "AscendedHeal", "AscendedLifeCost", "AscendedMana", "AscendedManaCost", "AscendedPlayerTargets", "AscendedPoison", "AscendedStrength", "Attack", "CharacterId", "Daggers", "Defense", "Description", "Draw", "Earth", "EffectMultiplier", "EnemyTargets", "Fire", "Heal", "LifeCost", "Mana", "ManaCost", "Momentum", "Name", "Permanent", "PlayerTargets", "Poison", "Rarity", "Spirit", "Strength", "Water" },
                values: new object[,]
                {
                    { 706, null, 0, 0, 0, 0, 0, 0, 0, 8, 0, 0, 0, 1, 0, 0, 0, 8, null, 0, "Heal an ally{{NL}}for {{Heal}} {{Heal_IMG}}", 0, null, 0, 0, null, 5, 0, 0, 1, null, "Herbalist", null, 1, 0, 1, null, 0, null },
                    { 707, null, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 8, null, 0, "Inflict {{Poison}} {{Poison_IMG}} on{{NL}}all enemies", 0, null, 0, 0, null, 0, 0, 0, 2, null, "Swarm", null, 0, 2, 2, null, 0, null },
                    { 708, null, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 8, null, 0, "Transform.", 0, null, 0, 0, null, 0, 0, 0, 1, null, "Bear Spirit", null, 0, 0, 2, 2, 0, null },
                    { 709, null, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 8, null, 0, "Transform.", 0, null, 0, 0, null, 0, 0, 0, 1, null, "Wolf Spirit", null, 0, 0, 2, 1, 0, null },
                    { 710, null, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 8, null, 0, "Transform.", 0, null, 0, 0, null, 0, 0, 0, 1, null, "Deer Spirit", null, 0, 0, 2, 3, 0, null },
                    { 711, null, 0, 0, 9, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 6, 8, null, 0, "Deal {{Attack}} {{Damage_IMG}}.", 0, null, 0, 1, null, 0, 0, 0, 1, null, "Wolf Bite", null, 0, 0, 1, 1, 0, null },
                    { 712, null, 4, 6, 6, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 4, 8, null, 0, "Deal {{Attack}} {{Damage_IMG}}{{NL}}and gain {{Armor}} {{Armor_IMG}}.", 0, null, 0, 1, null, 0, 0, 0, 1, null, "Bear Maul", null, 0, 0, 1, 2, 0, null },
                    { 713, null, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 8, null, 0, "Gain 2 {{Speed_IMG}}{{NL}}and draw {{Draw}} {{Card_IMG}}.", 1, null, 0, 0, null, 0, 0, 0, 1, null, "Fleethoof Charge", null, 0, 0, 1, 3, 0, null },
                    { 714, null, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 8, null, 0, "Draw {{Draw}} {{Card_IMG}}.", 2, null, 0, 0, null, 0, 0, 0, 1, null, "Moonfeather Wisdom", null, 0, 0, 1, 4, 0, null },
                    { 715, null, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 8, null, 0, "Apply {{Strength}} {{Strength_IMG}}{{NL}}to all Allies", 0, null, 0, 0, null, 0, 0, 0, 1, null, "Wolf Pack", null, 0, 0, 1, null, 2, null },
                    { 716, null, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 8, null, 0, "Share {{Strength_IMG}} to all Allies", 0, null, 0, 0, null, 0, 0, 0, 2, null, "Alpha Wolf", null, 0, 0, 2, null, 0, null }
                });

            migrationBuilder.InsertData(
                table: "Cards",
                columns: new[] { "Id", "Air", "Armor", "AscendedArmor", "AscendedAttack", "AscendedDefense", "AscendedDraw", "AscendedEffectMultiplier", "AscendedEnemyTargets", "AscendedHeal", "AscendedLifeCost", "AscendedMagicArmor", "AscendedMana", "AscendedManaCost", "AscendedPlayerTargets", "AscendedPoison", "AscendedStrength", "Attack", "CharacterId", "Daggers", "Defense", "Description", "Draw", "Earth", "EffectMultiplier", "EnemyTargets", "Fire", "Heal", "LifeCost", "MagicArmor", "Mana", "ManaCost", "Momentum", "Name", "Permanent", "PlayerTargets", "Poison", "Rarity", "Spirit", "Strength", "Water" },
                values: new object[] { 717, null, 0, 0, 0, 0, 0, 0, 0, 0, 0, 9, 0, 0, 0, 0, 0, 0, 8, null, 0, "Gain {{MagicArmor}} {{Magic_Armor_IMG}}{{NL}}{{Bear_Spirit}}: Gain {{MagicArmor}} {{Armor_IMG}}", 0, null, 0, 0, null, 0, 0, 6, 0, 1, null, "Hibernate", null, 0, 0, 1, null, 0, null });

            migrationBuilder.InsertData(
                table: "Cards",
                columns: new[] { "Id", "Air", "Armor", "AscendedArmor", "AscendedAttack", "AscendedDefense", "AscendedDraw", "AscendedEffectMultiplier", "AscendedEnemyTargets", "AscendedHeal", "AscendedLifeCost", "AscendedMagicAttack", "AscendedMana", "AscendedManaCost", "AscendedPlayerTargets", "AscendedPoison", "AscendedStrength", "Attack", "CharacterId", "Daggers", "Defense", "Description", "Draw", "Earth", "EffectMultiplier", "EnemyTargets", "Fire", "Heal", "LifeCost", "MagicAttack", "Mana", "ManaCost", "Momentum", "Name", "Permanent", "PlayerTargets", "Poison", "Rarity", "Spirit", "Strength", "Water" },
                values: new object[] { 718, null, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 1, 2, 0, 0, 0, 0, 8, null, 0, "Deal {{MagicAttack}}  {{Magic_Damage_IMG}}{{NL}}to All enemies.{{NL}}{{Deer_Spirit}}: Gain {{Mana}} {{Mana_IMG}}", 0, null, 0, 0, null, 0, 0, 4, 1, 2, null, "Stampede", null, 0, 0, 1, null, 0, null });

            migrationBuilder.InsertData(
                table: "Cards",
                columns: new[] { "Id", "Air", "Armor", "AscendedArmor", "AscendedAttack", "AscendedDefense", "AscendedDraw", "AscendedEffectMultiplier", "AscendedEnemyTargets", "AscendedHeal", "AscendedLifeCost", "AscendedMana", "AscendedManaCost", "AscendedPlayerTargets", "AscendedPoison", "AscendedStrength", "Attack", "CharacterId", "Daggers", "Defense", "Description", "Draw", "Earth", "EffectMultiplier", "EnemyTargets", "Fire", "Heal", "LifeCost", "Mana", "ManaCost", "Momentum", "Name", "Permanent", "PlayerTargets", "Poison", "Rarity", "Spirit", "Strength", "Water" },
                values: new object[,]
                {
                    { 719, null, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 8, null, 0, "{{Wolf_Spirit}}: Draw {{Draw}} {{Card_IMG}} when attacking.", 1, null, 0, 0, null, 0, 0, 0, 3, null, "Predator's Instinct", true, 0, 0, 2, null, 0, null },
                    { 720, null, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 8, null, 0, "Transform: {{Wolf_Spirit}} {{Bear_Spirit}} {{Deer_Spirit}} {{Owl_Spirit}} or {{Tortoise_Spirit}}", 0, null, 0, 0, null, 0, 0, 0, 0, null, "Wild Shift", null, 0, 0, 1, null, 0, null },
                    { 721, null, 0, 0, 0, 3, 3, 0, 0, 0, 0, 6, 1, 0, 0, 3, 0, 8, null, 2, "{{Bear_Spirit}}: Gain {{Defense}} {{Defense_IMG}}{{NL}}{{Wolf_Spirit}}: Gain {{Strength}} {{Strength_IMG}}{{NL}}{{Deer_Spirit}}: {{Draw}} {{Card_IMG}} Cards{{NL}}{{Owl_Spirit}}: Gain {{Mana}} {{Mana_IMG}}", 2, null, 0, 0, null, 0, 0, 4, 1, null, "Primal Harmony", null, 0, 0, 2, null, 2, null }
                });

            migrationBuilder.InsertData(
                table: "Cards",
                columns: new[] { "Id", "Air", "Armor", "AscendedArmor", "AscendedAttack", "AscendedDefense", "AscendedDraw", "AscendedEffectMultiplier", "AscendedEnemyTargets", "AscendedHeal", "AscendedLifeCost", "AscendedMagicAttack", "AscendedMana", "AscendedManaCost", "AscendedPlayerTargets", "AscendedPoison", "AscendedStrength", "Attack", "CharacterId", "Daggers", "Defense", "Description", "Draw", "Earth", "EffectMultiplier", "EnemyTargets", "Fire", "Heal", "LifeCost", "MagicAttack", "Mana", "ManaCost", "Momentum", "Name", "Permanent", "PlayerTargets", "Poison", "Rarity", "Spirit", "Strength", "Water" },
                values: new object[] { 722, null, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 2, 0, 0, 0, 0, 8, null, 0, "{{Owl_Spirit}}: deal + {{MagicAttack}} {{Magic_Damage_IMG}}", 0, null, 0, 0, null, 0, 0, 2, 0, 3, null, "Night Watcher", true, 0, 0, 2, null, 0, null });

            migrationBuilder.InsertData(
                table: "Cards",
                columns: new[] { "Id", "Air", "Armor", "AscendedArmor", "AscendedAttack", "AscendedDefense", "AscendedDraw", "AscendedEffectMultiplier", "AscendedEnemyTargets", "AscendedHeal", "AscendedLifeCost", "AscendedMana", "AscendedManaCost", "AscendedPlayerTargets", "AscendedPoison", "AscendedStrength", "Attack", "CharacterId", "Daggers", "Defense", "Description", "Draw", "Earth", "EffectMultiplier", "EnemyTargets", "Fire", "Heal", "LifeCost", "Mana", "ManaCost", "Momentum", "Name", "Permanent", "PlayerTargets", "Poison", "Rarity", "Spirit", "Strength", "Water" },
                values: new object[,]
                {
                    { 723, null, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 5, 0, 8, null, 0, "Gain {{Strength}} {{Strength_IMG}}", 0, null, 0, 0, null, 0, 0, 0, 2, null, "Howl", null, 0, 0, 1, null, 3, null },
                    { 724, null, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 8, null, 0, "Prevent all damage{{NL}}{{Tortoise_Spirit}}:Your attacks are reduced by half while in.", 0, null, 0, 0, null, 0, 0, 0, 2, null, "Iron Shell", null, 0, 0, 2, 5, 0, null },
                    { 725, null, 0, 0, 0, 0, 0, 0, 0, 9, 0, 0, 0, 1, 0, 0, 0, 8, null, 0, "Heal {{Heal}} {{Heal_IMG}}.", 0, null, 0, 0, null, 6, 0, 0, 1, null, "Regrowth", null, 1, 0, 1, null, 0, null },
                    { 726, null, 0, 0, 8, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 5, 8, null, 0, "Deal {{Attack}} {{Damage_IMG}}{{NL}}and reduce the target's {{Speed_IMG}} by 3.", 0, null, 0, 1, null, 0, 0, 0, 1, null, "Vine Lash", null, 0, 0, 1, null, 0, null },
                    { 727, null, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 8, null, 0, "Transform.", 0, null, 0, 0, null, 0, 0, 0, 1, null, "Tortoise Spirit", null, 0, 0, 3, 5, 0, null },
                    { 801, null, 0, 0, 13, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 8, 9, null, 0, "Deal {{Attack}} {{Damage_IMG}}", 0, null, 0, 1, null, 0, 0, 0, 1, null, "Attack", false, 0, 0, 0, null, 0, null },
                    { 802, null, 3, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 9, null, 0, "Gain {{Armor}} {{Armor_IMG}}", 0, null, 0, 0, null, 0, 0, 0, 1, null, "Defense", false, 0, 0, 0, null, 0, null },
                    { 803, null, 0, 0, 8, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5, 9, null, 0, "Deal {{Attack}} {{Damage_IMG}} to all enemies", 0, null, 0, 0, null, 0, 0, 0, 2, null, "Cleave", false, 0, 0, 1, null, 0, null },
                    { 804, null, 0, 0, 0, -1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 0, 9, null, -2, "Gain {{Strength}} {{Strength_IMG}} and {{Defense}} {{Defense_IMG}}", 0, null, 0, 0, null, 0, 0, 0, 2, null, "Frenzy", false, 0, 0, 2, null, 4, null },
                    { 805, null, 0, 0, 20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 14, 9, null, 0, "Deal {{Attack}} {{Damage_IMG}}", 0, null, 0, 0, null, 0, 0, 0, 2, null, "Brutal Strike", false, 0, 0, 1, null, 0, null },
                    { 806, null, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 9, null, 0, "Gain {{Strength}} {{Strength_IMG}}", 0, null, 0, 0, null, 0, 0, 0, 0, null, "War Howl", false, 0, 0, 1, null, 2, null },
                    { 807, null, 0, 0, 9, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 6, 9, null, 0, "Deal {{Attack}} {{Damage_IMG}}. Increase {{Damage_IMG}} by {{EffectMultiplier}}", 0, null, 3, 0, null, 0, 0, 0, 1, null, "Rampage", false, 0, 0, 2, null, 0, null },
                    { 808, null, 0, 0, 0, -2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 8, 0, 9, null, -3, "Gain {{Strength}} {{Strength_IMG}} and {{Defense}} {{Defense_IMG}}", 0, null, 0, 0, null, 0, 0, 0, 2, null, "Berserker Rage", false, 0, 0, 3, null, 5, null },
                    { 809, null, 0, 0, 15, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 10, 9, null, 0, "Deal {{Attack}} {{Damage_IMG}} to all enemies", 0, null, 0, 0, null, 0, 0, 0, 3, null, "Earthshatter", false, 0, 0, 2, null, 0, null },
                    { 810, null, 0, 0, 35, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 25, 9, null, 0, "Deal {{Attack}} {{Damage_IMG}}", 0, null, 0, 0, null, 0, 0, 0, 3, null, "Death Blow", false, 0, 0, 3, null, 0, null },
                    { 811, null, 0, 0, 26, 0, 0, 0, 1, 0, 4, 0, 1, 0, 0, 0, 18, 9, null, 0, "Deal {{Attack}} {{Damage_IMG}}. Lose {{LifeCost}} HP", 0, null, 0, 1, null, 0, 6, 0, 2, null, "Greataxe Tribute", false, 0, 0, 2, null, 0, null },
                    { 812, null, 0, 0, 18, 0, 0, 0, 0, 0, 2, 0, 2, 0, 0, 0, 13, 9, null, 0, "Deal {{Attack}} {{Damage_IMG}} to all enemies. Lose {{LifeCost}} HP", 0, null, 0, 0, null, 0, 4, 0, 3, null, "Backblast Cleave", false, 0, 0, 3, null, 0, null },
                    { 813, null, 0, 0, 22, -2, 0, 0, 1, 0, 0, 0, 1, 0, 0, 4, 16, 9, null, -3, "Deal {{Attack}} {{Damage_IMG}}. Gain {{Strength}} {{Strength_IMG}} and {{Defense}} {{Defense_IMG}}", 0, null, 0, 1, null, 0, 0, 0, 2, null, "Executioner's Rush", false, 0, 0, 2, null, 2, null },
                    { 814, null, 0, 0, 12, 0, 0, 12, 1, 0, 3, 0, 1, 0, 0, 0, 8, 9, null, 0, "Roll the bone die: deal {{Attack}} + {{EffectMultiplier}} {{Damage_IMG}}. Lose {{LifeCost}} HP on a bad roll", 0, null, 9, 1, null, 0, 5, 0, 2, null, "Bone-Die Frenzy", false, 0, 0, 3, null, 0, null },
                    { 815, null, 0, 0, 30, 0, 0, 0, 1, 0, 3, 0, 2, 0, 0, 0, 22, 9, null, 0, "Hurl a boulder for {{Attack}} {{Damage_IMG}}. Lose {{LifeCost}} HP", 0, null, 0, 1, null, 0, 5, 0, 3, null, "Boulder Reprisal", false, 0, 0, 3, null, 0, null },
                    { 816, null, 0, 0, 20, -2, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 14, 9, null, -4, "Deal {{Attack}} {{Damage_IMG}} to all enemies and gain {{Defense}} {{Defense_IMG}}", 0, null, 0, 0, null, 0, 0, 0, 3, null, "Cyclone of Splinters", false, 0, 0, 3, null, 0, null },
                    { 817, null, 0, 0, 38, 0, 0, 0, 1, 0, 5, 0, 3, 0, 0, 0, 28, 9, null, 0, "Deal {{Attack}} {{Damage_IMG}}. Lose {{LifeCost}} HP", 0, null, 0, 1, null, 0, 8, 0, 4, null, "Ruinbound Oath", false, 0, 0, 3, null, 0, null },
                    { 818, null, 0, 0, 24, 0, 0, 0, 0, 0, 4, 0, 3, 0, 0, 0, 16, 9, null, 0, "Throw axes to deal {{Attack}} {{Damage_IMG}} to all enemies. Lose {{LifeCost}} HP", 0, null, 0, 0, null, 0, 6, 0, 4, null, "Axe Rain", false, 0, 0, 3, null, 0, null },
                    { 819, null, 0, 0, 27, 0, 0, 0, 1, 0, 2, 0, 2, 0, 0, 5, 20, 9, null, 0, "Deal {{Attack}} {{Damage_IMG}}. Gain {{Strength}} {{Strength_IMG}} and lose {{LifeCost}} HP", 0, null, 0, 1, null, 0, 4, 0, 3, null, "Spite of the Pit", false, 0, 0, 2, null, 3, null },
                    { 820, null, 0, 0, 46, 0, 0, 14, 1, 0, 7, 0, 4, 0, 0, 0, 34, 9, null, 0, "Roll the bone die: deal massive {{Attack}} {{Damage_IMG}} or lose {{LifeCost}} HP", 0, null, 10, 1, null, 0, 12, 0, 5, null, "Colossus Gamble", false, 0, 0, 3, null, 0, null },
                    { 901, null, 0, 0, 8, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 5, 10, null, 0, "Deal {{Attack}} {{Damage_IMG}}", 0, null, 0, 1, null, 0, 0, 0, 1, null, "Attack", false, 0, 0, 0, null, 0, null },
                    { 902, null, 5, 7, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 10, null, 0, "Gain {{Armor}} {{Armor_IMG}}", 0, null, 0, 0, null, 0, 0, 0, 1, null, "Defense", false, 0, 0, 0, null, 0, null },
                    { 903, null, 0, 0, 0, 0, 0, 0, 0, 9, 0, 0, 0, 0, 0, 0, 0, 10, null, 0, "Restore {{Heal}} {{Heal_IMG}}", 0, null, 0, 0, null, 6, 0, 0, 1, null, "Repair Kit", false, 0, 0, 1, null, 0, null },
                    { 904, null, 0, 0, 8, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 5, 10, null, 0, "Deal {{Attack}} {{Damage_IMG}}. Increase {{Damage_IMG}} by {{EffectMultiplier}}", 0, null, 2, 0, null, 0, 0, 0, 2, null, "Turret Deploy", false, 0, 0, 2, null, 0, null },
                    { 905, null, 6, 9, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 10, null, 0, "Gain {{Armor}} {{Armor_IMG}} and draw {{Draw}} {{Card_IMG}}", 1, null, 0, 0, null, 0, 0, 0, 1, null, "Smoke Bomb", false, 0, 0, 1, null, 0, null },
                    { 906, null, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 10, null, 0, "Gain {{Mana}} {{Mana_IMG}}", 0, null, 0, 0, null, 0, 0, 2, 0, null, "Overclock", false, 0, 0, 2, null, 0, null },
                    { 907, null, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 10, null, 0, "Inflict {{Poison}} {{Poison_IMG}} on an enemy", 0, null, 0, 0, null, 0, 0, 0, 1, null, "Sabotage", false, 0, 2, 2, null, 0, null },
                    { 908, null, 0, 0, 0, 0, 0, 0, 0, 11, 0, 0, 0, 0, 0, 0, 0, 10, null, 0, "Heal an ally for {{Heal}} {{Heal_IMG}}", 0, null, 0, 0, null, 7, 0, 0, 1, null, "Field Medic", false, 0, 0, 2, null, 0, null },
                    { 909, null, 0, 0, 12, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 8, 10, null, 0, "Deal {{Attack}} {{Damage_IMG}} and gain {{Mana}} {{Mana_IMG}}", 0, null, 0, 0, null, 0, 0, 1, 2, null, "Power Surge", false, 0, 0, 2, null, 0, null },
                    { 910, null, 0, 0, 30, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 22, 10, null, 0, "Deal {{Attack}} {{Damage_IMG}}", 0, null, 0, 0, null, 0, 0, 0, 3, null, "Mega Cannon", false, 0, 0, 3, null, 0, null }
                });

            migrationBuilder.InsertData(
                table: "CharacterClasses",
                columns: new[] { "Id", "Description", "Energy", "HitPoints", "Name", "Order", "Speed" },
                values: new object[,]
                {
                    { 1, "The Warrior class balances offense and defense, using their sturdy armor and precise strikes to withstand attacks while dealing consistent damage. They excel at adaptability, shielding allies and seizing opportunities to strike.", 3, 80, "Warrior", 0, 6 },
                    { 2, "The Assassin class excels in delivering rapid, high-impact attacks and inflicting poison to weaken enemies over time. Their abilities are designed for efficiency, with low mana costs allowing for sustained offensive pressure. Assassins thrive on dealing burst damage and outlasting opponents through relentless assaults.", 3, 60, "Assassin", 1, 9 },
                    { 3, "The Ranger class specializes in precision and versatility, excelling at landing critical strikes and dealing damage to multiple targets. With their keen aim and tactical prowess, Rangers are adept at controlling the battlefield, taking down enemies swiftly and efficiently.", 3, 70, "Ranger", 6, 8 },
                    { 4, "The Paladin class is a resilient combatant, specializing in empowering themselves through potent buffs and self-healing abilities. With a focus on durability and sustained strength, Paladins excel at enduring prolonged battles while maintaining their own vitality. Their abilities make them ideal for those who value self-reliance and unwavering perseverance on the battlefield.", 3, 90, "Paladin", 7, 5 },
                    { 5, "The White Mage class is a supportive powerhouse, specializing in healing and empowering allies with powerful buffs. While their primary role focuses on restoring health and enhancing team effectiveness, they also possess a few offensive abilities for self-defense and contributing to the fight. White Mages shine as the heart of any team, ensuring allies stay strong and capable in battle.", 3, 50, "White Mage", 8, 4 },
                    { 6, "The Black Mage class wields devastating magical attacks, harnessing the elements to deal massive damage to enemies. They also tap into the dark art of blood magic, sacrificing a portion of their own vitality to restore their power and sustain their offensive onslaught. Black Mages excel at overwhelming foes with raw magical force while maintaining their strength through calculated risk and mastery of forbidden techniques.", 3, 40, "Black Mage", 5, 5 },
                    { 7, "The Elementalist class commands the four primal elements, each offering unique versatility in battle. Earth provides defensive abilities to withstand attacks, Fire delivers devastating single-target damage, Air unleashes multi-target assaults to control the battlefield, and Water offers restorative powers to heal and sustain. Elementalists excel at adapting to any situation, making them a versatile and dynamic force in combat.", 3, 60, "Elementalist", 2, 7 },
                    { 8, "The Druid class harnesses the power of nature and its creatures to influence the battlefield. They specialize in buffing allies with empowering effects while crippling enemies with potent debuffs. With their deep connection to the natural world, Druids excel at providing versatile support and controlling the flow of combat, ensuring their team has the upper hand.", 3, 70, "Druid", 3, 6 },
                    { 9, "The Barbarian class unleashes raw power through massive, high-damage attacks, prioritizing offense over defense. With their relentless aggression and devastating strikes, Barbarians excel at overwhelming enemies quickly. However, their limited defensive abilities require careful timing and positioning to maximize their destructive potential on the battlefield.", 3, 100, "Barbarian", 4, 7 },
                    { 10, "The Engineer class is a versatile problem solver, excelling as a jack-of-all-trades on the battlefield. Using their ingenuity and creativity, they adapt to any situation with unique tools and clever strategies. Engineers thrive in unpredictable scenarios, offering a balanced mix of utility, offense, and support to turn the tide of battle.", 3, 70, "Engineer", 9, 6 }
                });

            migrationBuilder.InsertData(
                table: "Enemies",
                columns: new[] { "Id", "Guid", "MaxHitPoints", "MaxSpeed", "MinHitPoints", "MinSpeed", "Name" },
                values: new object[,]
                {
                    { 1, new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), 10, 7, 8, 5, "Shroomblade" },
                    { 2, new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901"), 12, 9, 10, 7, "Bee" },
                    { 3, new Guid("c3d4e5f6-a7b8-9012-cdef-123456789012"), 14, 8, 12, 6, "Frog" },
                    { 4, new Guid("d4e5f6a7-b8c9-0123-defa-234567890123"), 16, 10, 14, 8, "Drake" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Decks_CardId",
                table: "Decks",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_Decks_PlayerId",
                table: "Decks",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Dungeons_GameId",
                table: "Dungeons",
                column: "GameId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Enemies_Guid",
                table: "Enemies",
                column: "Guid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PartyMembers_ClassId",
                table: "PartyMembers",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_PartyMembers_InstanceId",
                table: "PartyMembers",
                column: "InstanceId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PartyMembers_PlayerId",
                table: "PartyMembers",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Players_GameId",
                table: "Players",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_Players_UserId",
                table: "Players",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_Token",
                table: "RefreshTokens",
                column: "Token",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId",
                table: "RefreshTokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_DungeonId",
                table: "Rooms",
                column: "DungeonId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserCharacters_CharacterId",
                table: "UserCharacters",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_UserCharacters_UserId",
                table: "UserCharacters",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Blessings");

            migrationBuilder.DropTable(
                name: "Decks");

            migrationBuilder.DropTable(
                name: "Enemies");

            migrationBuilder.DropTable(
                name: "PartyMembers");

            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "Rooms");

            migrationBuilder.DropTable(
                name: "UserCharacters");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Cards");

            migrationBuilder.DropTable(
                name: "Players");

            migrationBuilder.DropTable(
                name: "Dungeons");

            migrationBuilder.DropTable(
                name: "CharacterClasses");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Games");
        }
    }
}
