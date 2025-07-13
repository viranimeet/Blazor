using Dapper;
using EmployeeManagement.Web.Data.Database;
using EmployeeManagement.Web.Data.Models;
using System.Data;

namespace EmployeeManagement.Web.Services
{
    public class DepartmentService
    {
        private readonly AppDbContext _dbContext;

        public DepartmentService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Department>> GetAllDepartments()
        {
            using var connection = _dbContext.CreateConnection();
            return await connection.QueryAsync<Department>("sp_GetAllDepartments",
                commandType: CommandType.StoredProcedure);
        }

        public async Task<Department?> GetDepartmentById(int id)
        {
            using var connection = _dbContext.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Department>(
                "sp_GetDepartmentById",
                new { DepartmentId = id },
                commandType: CommandType.StoredProcedure);
        }

        public async Task DeleteDepartment(int id)
        {
            using var connection = _dbContext.CreateConnection();
            await connection.ExecuteAsync("sp_DeleteDepartment",
                new { DepartmentId = id },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<int> UpdateDepartment(Department department)
        {
            using var connection = _dbContext.CreateConnection();
            return await connection.ExecuteAsync("sp_UpdateDepartment",
                new
                {
                    department.DepartmentId,
                    department.DepartmentName,
                    department.Description
                },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<int> AddDepartment(Department department)
        {
            using var connection = _dbContext.CreateConnection();
            return await connection.ExecuteAsync("sp_AddDepartment",
                new
                {
                    department.DepartmentName,
                    department.Description
                },
                commandType: CommandType.StoredProcedure);
        }
    }
}
