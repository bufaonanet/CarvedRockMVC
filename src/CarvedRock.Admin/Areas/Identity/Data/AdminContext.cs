using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CarvedRock.Admin.Data;

public class AdminContext : IdentityDbContext<AdminUser>
{
    private readonly string _dbPath;

    public AdminContext(IConfiguration config)
    {
        var path = Directory.GetCurrentDirectory();
        _dbPath = Path.Join(path, config.GetConnectionString("UserDbFilename"));
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite($"Data Source={_dbPath}");
        base.OnConfiguring(optionsBuilder);
    }
}