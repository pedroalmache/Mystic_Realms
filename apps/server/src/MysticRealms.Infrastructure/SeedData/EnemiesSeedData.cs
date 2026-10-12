namespace MysticRealms.Infrastructure.SeedData;

public static class EnemiesSeedData
{
    public static object[] Get() =>
    [
        new { Id = 1, Guid = new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), Name = "Shroomblade", MinHitPoints = 8,  MaxHitPoints = 10, MinSpeed = 5, MaxSpeed = 7  },
        new { Id = 2, Guid = new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901"), Name = "Bee",         MinHitPoints = 10, MaxHitPoints = 12, MinSpeed = 7, MaxSpeed = 9  },
        new { Id = 3, Guid = new Guid("c3d4e5f6-a7b8-9012-cdef-123456789012"), Name = "Frog",        MinHitPoints = 12, MaxHitPoints = 14, MinSpeed = 6, MaxSpeed = 8  },
        new { Id = 4, Guid = new Guid("d4e5f6a7-b8c9-0123-defa-234567890123"), Name = "Drake",       MinHitPoints = 14, MaxHitPoints = 16, MinSpeed = 8, MaxSpeed = 10 },
    ];
}
