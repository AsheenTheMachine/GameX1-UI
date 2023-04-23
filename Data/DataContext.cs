
namespace GameX1.Data;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

public class DataContext : DbContext
{
    protected readonly IConfigurationRoot configuration;
    
    public DataContext()
    {
        configuration = new ConfigurationBuilder()
        .SetBasePath(System.AppDomain.CurrentDomain.BaseDirectory)
        .AddJsonFile("appsettings.json")
        .Build();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    => options.UseSqlite(configuration.GetConnectionString("Picture Database"));

    public DbSet<Picture> Pictures { get; set; }

    public DbSet<ViewedPicture> ViewedPicture { get; set; }

}

