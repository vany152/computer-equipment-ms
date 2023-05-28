using Npgsql;

namespace ComputerEquipmentMS.DataAccess.Util;

public static class NpgsqlUtil
{
    public static NpgsqlConnection CreateNpgsqlConnectionWithNodaTime(string connectionString)
    {
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
        dataSourceBuilder.UseNodaTime();
        var dataSource = dataSourceBuilder.Build();

        return dataSource.OpenConnection();
    }
}