using System.Data.SqlClient;

namespace SqlServerSMO.Tests.Library;

public static class Database
{
  public static async Task WaitForConnection(
    string connectionString,
    int timeout = int.MaxValue
  )
  {
    if (timeout < 0)
    {
      throw new ArgumentException($"{nameof(timeout)} must be greater or equal to zero.");
    }

    var connection = new SqlConnection(connectionString);

    var millisecondsToWait = (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond) + timeout;
    while (true)
    {
      try
      {
        await connection.OpenAsync();
        var command = new SqlCommand(
          "SELECT 1",
          connection
        );
        await command.ExecuteNonQueryAsync();
        break;
      }
      catch (SqlException e)
      {
        await connection.CloseAsync();
        await Task.Delay(500);
        var now = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        if (now >= millisecondsToWait)
        {
          await connection.CloseAsync();
          throw new ApplicationException(
            $"Timeout waiting for SQL Server connection {connection.DataSource}/{connection.Database}",
            e
          );
        }

        await connection.CloseAsync();
      }
    }
  }
}
