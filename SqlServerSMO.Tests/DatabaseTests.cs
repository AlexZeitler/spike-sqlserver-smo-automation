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
    var dbName = "TestDb";
    var db = new Database(server, dbName);
    
    db.Create();
    
    var exists = server.Databases.Contains(dbName);
    db.DropIfExists();
    
    Assert.True(exists);
  }
  
}
