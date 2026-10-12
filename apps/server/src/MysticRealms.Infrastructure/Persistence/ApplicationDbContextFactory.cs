using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace MysticRealms.Infrastructure.Persistence;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var basePath = FindSettingsBasePath();
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

        var configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile($"appsettings.{environment}.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseSqlServer(connectionString);

        return new ApplicationDbContext(optionsBuilder.Options);
    }

    /// <summary>
    /// Locates the directory containing appsettings.json by walking up from the
    /// current working directory. This handles cases where EF tools are invoked
    /// from an output directory (e.g. bin/Debug/net10.0) or directly from the
    /// startup-project root (the recommended approach via --startup-project).
    /// </summary>
    private static string FindSettingsBasePath()
    {
        var directory = new DirectoryInfo(Directory.GetCurrentDirectory());

        while (directory != null)
        {
            if (File.Exists(Path.Combine(directory.FullName, "appsettings.json")))
                return directory.FullName;

            directory = directory.Parent;
        }

        return Directory.GetCurrentDirectory();
    }
}
