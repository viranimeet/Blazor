using Microsoft.Data.SqlClient;
using System.Data;

namespace EmployeeManagement.Web.Data.Database;

public class AppDbContext
{
    private IConfiguration _config;
    private string _connectionString;

    public AppDbContext(IConfiguration config)
    {
        _config = config;
        _connectionString = _config.GetConnectionString("DefaultConnection");
    }

    public IDbConnection CreateConnection() => new SqlConnection(_connectionString);
}