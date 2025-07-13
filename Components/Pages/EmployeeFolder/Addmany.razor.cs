using Microsoft.AspNetCore.Components;
using EmployeeManagement.Web.Data.Models;
using EmployeeManagement.Web.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EmployeeManagement.Web.Components.Pages.EmployeeFolder
{
    public partial class Addmany
    {
        [Inject] public EmployeeService? EmployeeService { get; set; }
        [Inject] public DepartmentService DepartmentService { get; set; }

        private List<Department> departments = new List<Department>();
        private int numberOfEmployees { get; set; }
        private List<Employee> employees = new List<Employee>();
        private List<string> errors = new List<string>();
        protected override async void OnInitialized()
        {
            Console.WriteLine("Addmany initialized");
            departments = (await DepartmentService.GetAllDepartments()).ToList();
        }

        private void GenerateRows()
        {
            Console.WriteLine($"Generating {numberOfEmployees} employees");

            employees.Clear();
            if (numberOfEmployees <= 0)
                return;

            for (int i = 0; i < numberOfEmployees; i++)
            {
                employees.Add(new Employee
                {
                    FirstName = "",
                    LastName = "",
                    Email = "",
                    DepartmentId = null,
                    IsActive = true
                });
            }

            Console.WriteLine($"Employees count after generate: {employees.Count}");
        }

        private async Task SubmitMany()
        {
            errors.Clear();

            if (employees.Any())
            {
                var result = await EmployeeService.BulkInsertEmployees(employees);

                if (result.Any())
                {
                    errors = result;
                }
                else
                {
                    employees.Clear();
                    numberOfEmployees = 0;
                }
            }
        }
    }
}
