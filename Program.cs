using System;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

AppContext.SetSwitch("Switch.Microsoft.Data.SqlClient.UseManagedNetworkingOnWindows", true);

await using var connection = new SqlConnection("Server=tcp:YourDb;Initial Catalog=YourDb;Persist Security Info=False;User ID= YourUser;Password=YourPass;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;");
await connection.OpenAsync();
await using var command = connection.CreateCommand();
command.CommandText = "EXECUTE [dbo].[sp_SqlRaceCondition] @areaList";
command.Parameters.Add(new SqlParameter("@areaList", await GetAreaTable()) { TypeName = "AreaList", SqlDbType = SqlDbType.Structured });

await using var reader = await command.ExecuteReaderAsync();
if (reader.HasRows)
{
    while (await reader.ReadAsync())
    {
        Console.WriteLine($"Result: {reader.GetInt32(0)}");
    }    
}
await connection.CloseAsync();

static async Task<DataTable> GetAreaTable()
{
    var table = new DataTable();
    table.Columns.Add(new DataColumn("area", typeof(string)));
    foreach (var area in await File.ReadAllLinesAsync("./areas.txt"))
    {
        var row = table.NewRow();
        row[0] = area;
        table.Rows.Add(row);
    }

    return table;
}