using Dapper;
using EmployeeManagement.Web.Data.Database;
using EmployeeManagement.Web.Data.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace EmployeeManagement.Web.Services
{
    public class TeamService
    {
        private AppDbContext _dbContext;

        public TeamService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Team>> GetTeamsByDepartment(string departmentName)
        {
            using (var connection = _dbContext.CreateConnection())
            {
                var teams = (await connection.QueryAsync<Team>(
                    "sp_GetTeamsByDepartmentName",
                    new { DepartmentName = departmentName },
                    commandType: CommandType.StoredProcedure)).ToList();

                Console.WriteLine($"TeamService fetched {teams.Count} teams for {departmentName}");
                foreach (var t in teams)
                {
                    Console.WriteLine($"TeamId: {t.TeamId}, TeamName: {t.TeamName}");
                }

                return teams;
            }
        }

    }
}
