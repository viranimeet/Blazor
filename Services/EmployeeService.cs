using Dapper;
using EmployeeManagement.Web.Data.Database;
using EmployeeManagement.Web.Data.Models;
using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace EmployeeManagement.Web.Services;

public class EmployeeService
{
    private AppDbContext _dbContext;

    public EmployeeService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Employee>> GetAllEmployees()
    {
        using var connection = _dbContext.CreateConnection();
        return await connection.QueryAsync<Employee>("sp_GetAllEmployees",
            commandType: CommandType.StoredProcedure);
    }

    public async Task<Employee?> GetEmployeeById(int id)
    {
        using var connection = _dbContext.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<Employee>("sp_GetEmployeeById",
            new { EmployeeId = id },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<(IEnumerable<Employee> employees, int totalRecords)> GetEmployeesPaged(int pageNumber, int pageSize, string? department = null, string? gender = null, bool? status = null)
    {
        using var connection = _dbContext.CreateConnection();
        using var multi = await connection.QueryMultipleAsync("sp_GetAllEmployeesPaged",
            new { PageNumber = pageNumber, PageSize = pageSize, Department = department, Gender = gender, Status = status },
            commandType: CommandType.StoredProcedure);

        var totalRecords = await multi.ReadFirstAsync<int>();
        var employees = await multi.ReadAsync<Employee>();

        return (employees, totalRecords);
    }

    public async Task<int> CreateEmployee(Employee employee)
    {
        using var connection = _dbContext.CreateConnection();
        var parameters = new DynamicParameters();
        parameters.Add("@FirstName", employee.FirstName);
        parameters.Add("@LastName", employee.LastName);
        parameters.Add("@Email", employee.Email);
        parameters.Add("@Phone", employee.Phone);
        parameters.Add("@Gender", employee.Gender);
        parameters.Add("@DateOfBirth", employee.DateOfBirth);
        parameters.Add("@DepartmentId", employee.DepartmentId);
        parameters.Add("@IsActive", employee.IsActive);
        parameters.Add("@AadharPath", employee.AadharPath);
        parameters.Add("@TermsAccepted", employee.TermsAccepted);
        parameters.Add("@EmployeeId", dbType: DbType.Int32, direction: ParameterDirection.Output);

        await connection.ExecuteAsync("sp_InsertEmployee", parameters,
            commandType: CommandType.StoredProcedure);

        return parameters.Get<int>("@EmployeeId");
    }

    public async Task<List<string>> BulkInsertEmployees(List<Employee> employees)
    {
        var validationErrors = new List<string>();

        var dt = new DataTable();
        dt.Columns.Add("FirstName", typeof(string));
        dt.Columns.Add("LastName", typeof(string));
        dt.Columns.Add("Email", typeof(string));
        dt.Columns.Add("Phone", typeof(string));
        dt.Columns.Add("Gender", typeof(string));
        dt.Columns.Add("DateOfBirth", typeof(DateTime));
        dt.Columns.Add("DepartmentId", typeof(int));
        dt.Columns.Add("IsActive", typeof(bool));
        dt.Columns.Add("JoinDate", typeof(DateTime));
        dt.Columns.Add("AadharPath", typeof(string));
        dt.Columns.Add("TermsAccepted", typeof(bool));

        foreach (var emp in employees)
        {
            var context = new ValidationContext(emp, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            if (!Validator.TryValidateObject(emp, context, results, true))
            {
                foreach (var error in results)
                {
                    validationErrors.Add($"Employee [{emp.FirstName} {emp.LastName}]: {error.ErrorMessage}");
                }
            }
            else
            {
                dt.Rows.Add(
                    emp.FirstName,
                    emp.LastName,
                    emp.Email,
                    emp.Phone ?? (object)DBNull.Value,
                    emp.Gender ?? (object)DBNull.Value,
                    emp.DateOfBirth == DateTime.MinValue ? (object)DBNull.Value : emp.DateOfBirth,
                    emp.DepartmentId,
                    emp.IsActive,
                    emp.JoinDate == DateTime.MinValue ? (object)DBNull.Value : emp.JoinDate,
                    emp.AadharPath ?? (object)DBNull.Value,
                    emp.TermsAccepted
                );
            }
        }

        if (!validationErrors.Any())
        {
            using (var connection = _dbContext.CreateConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Employees", dt.AsTableValuedParameter("dbo.EmployeeType"));
                await connection.ExecuteAsync("sp_AddEmployeesBulk", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        return validationErrors;
    }



    public async Task UpdateEmployee(Employee employee)
    {
        using var connection = _dbContext.CreateConnection();
        await connection.ExecuteAsync("sp_UpdateEmployee", new
        {
            employee.EmployeeId,
            employee.FirstName,
            employee.LastName,
            employee.Email,
            employee.Phone,
            employee.Gender,
            employee.DateOfBirth,
            employee.DepartmentId,
            employee.IsActive,
            employee.AadharPath,
            employee.TermsAccepted
        }, commandType: CommandType.StoredProcedure);
    }

    public async Task DeleteEmployee(int id)
    {
        using var connection = _dbContext.CreateConnection();
        await connection.ExecuteAsync("sp_DeleteEmployee",
            new { EmployeeId = id },
            commandType: CommandType.StoredProcedure);
    }

}