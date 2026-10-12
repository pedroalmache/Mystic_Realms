1. **Create Spirit Enum**
   - Create `src/MysticRealms.Domain/Enums/Spirit.cs`.
   - Add values: `None`, `Wolf`, `Bear`, `Deer`, `Owl`, `Tortoise` (note: correcting 'Own' to 'Owl' based on standard druid spirits and typo context).
2. **Update Domain Entities**
   - In `Card.cs`: Add `public Spirit? Spirit { get; private set; } = Enums.Spirit.None;`
   - In `PartyMember.cs`: Add `public Spirit? Spirit { get; private set; } = Enums.Spirit.None;`
3. **Update DTOs and Records**
   - In `SavedGameDto.cs`:
     - Update `CardDto` to include `Spirit? Spirit`.
     - Update `PartyMemberDto` to include `Spirit? Spirit`.
   - In `GameAction.cs`:
     - Update `CardRecord` to include `Spirit? Spirit`.
     - Update `PartyMemberRecord` to include `Spirit? Spirit`.
4. **Update Mappers**
   - In `CombatTurnHelper.cs`: Update `ToRecord` methods to include `Spirit` mapping for both `Card` and `PartyMember`.
   - Update any `CardDto` and `PartyMemberDto` constructors to initialize `Spirit`.
5. **Database Migration**
   - The user has stated: "To generate a new EF Core migration, ensure the `dotnet-ef` tool is installed globally (`dotnet tool install --global dotnet-ef`) and run: `dotnet ef migrations add <MigrationName> -p src/MysticRealms.Infrastructure/MysticRealms.Infrastructure.csproj -s src/MysticRealms.ServerApi/MysticRealms.ServerApi.csproj`"
   - I need to run this command after making changes to the Domain models to generate the EF migration for the `Spirit` enum.
6. **Pre-commit and Submit**
   - Run pre-commit instructions.
   - Submit the changes.
