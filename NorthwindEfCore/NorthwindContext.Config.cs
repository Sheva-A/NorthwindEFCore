using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace NorthwindEfCore.Models;

// Доповнення до згенерованого NorthwindContext.
// Окремий файл, щоб повторний scaffolding його не перезаписав
public partial class NorthwindContext
{
    public NorthwindContext()
    {
    }

    // Рядок підключення береться з appsettings.json (файл не потрапляє на GitHub)
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        optionsBuilder.UseSqlServer(config.GetConnectionString("Northwind"));
    }
}
