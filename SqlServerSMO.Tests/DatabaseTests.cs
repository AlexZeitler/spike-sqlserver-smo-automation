using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using static SqlServerSMO.Tests.Library.Database;

namespace SqlServerSMO.Tests;

public class DatabaseTests
{
  [Fact]
  public async Task SmoShouldCreateDatabase()
  {
    var connectionString = "Server=localhost,1433;User Id=sa;Password=secretpassw0rd!;";
    await WaitForConnection(connectionString, 20000);
    var sqlConnection = new SqlConnection(connectionString);
    var serverConnection = new ServerConnection(sqlConnection);
    var server = new Server(serverConnection);
    var dbName = $"TestDb-{Guid.NewGuid()}";
    ;
    var db = new Database(server, dbName);

    db.Create();

    var exists = server.Databases.Contains(dbName);
    db.DropIfExists();

    Assert.True(exists);
  }

  [Fact]
  public async Task SmoShouldExecuteScript()
  {
    var connectionString = "Server=localhost,1433;User Id=sa;Password=secretpassw0rd!;";
    await WaitForConnection(connectionString, 20000);
    var sqlConnection = new SqlConnection(connectionString);
    var serverConnection = new ServerConnection(sqlConnection);
    var server = new Server(serverConnection);
    var dbName = $"TestDb-{Guid.NewGuid()}";
    var db = new Database(server, dbName);
    db.Create();
    var script =
      File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "Resources", "Scripts", "TestTable.sql"));
    
    db.ExecuteNonQuery(script);

    var queryConnectionString = $"Server=localhost,1433;Database={dbName};User Id=sa;Password=secretpassw0rd!;";
    var querySqlConnection = new SqlConnection(queryConnectionString);
    var result = await querySqlConnection.QueryAsync("SELECT * FROM TestTable");
    await querySqlConnection.CloseAsync();
    await sqlConnection.CloseAsync();
    
    Assert.Equal(2,result.Count());
  }
}
