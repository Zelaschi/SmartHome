using System.Reflection;
using Microsoft.Data.SqlClient;

public static class SqlExceptionHelper
{
    public static SqlException CreateSqlException()
    {
        var sqlErrorCtor = typeof(SqlError).GetConstructor(
            BindingFlags.NonPublic | BindingFlags.Instance,
            null,
            new[] { typeof(int), typeof(byte), typeof(byte), typeof(string), typeof(string), typeof(string), typeof(int) },
            null);

        var sqlError = sqlErrorCtor.Invoke(new object[] { 0, (byte)0, (byte)0, "server", "Forced SqlException", "procedure", 0 });

        var errors = Activator.CreateInstance(typeof(SqlErrorCollection), true);
        typeof(SqlErrorCollection).GetMethod("Add", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(errors, new[] { sqlError });

        var sqlExceptionCtor = typeof(SqlException).GetConstructor(
            BindingFlags.NonPublic | BindingFlags.Instance,
            null,
            new[] { typeof(string), typeof(SqlErrorCollection), typeof(Exception), typeof(Guid) },
            null);

        return (SqlException)sqlExceptionCtor.Invoke(new object[] { "Forced SqlException", errors, null, Guid.NewGuid() });
    }
}
