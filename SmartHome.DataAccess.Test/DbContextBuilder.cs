using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using SmartHome.DataAccess.Contexts;

namespace SmartHome.DataAccess.Test;

internal sealed class DbContextBuilder
{
    private static readonly SqliteConnection _connection = new ("Data Source=:memory:");

    public static SmartHomeEFCoreContext BuildTestDbContext()
    {
        var options = new DbContextOptionsBuilder<SmartHomeEFCoreContext>()
            .UseSqlite(_connection)
            .Options;

        _connection.Open();

        var context = new SmartHomeEFCoreContext(options);
        context.Database.EnsureCreated();

        return context;
    }
}
