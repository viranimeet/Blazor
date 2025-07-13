using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using EmployeeManagement.Web.Data.Models;
using EmployeeManagement.Web.Services;
using System.Diagnostics;
using System.Web;

namespace EmployeeManagement.Web.Components.Pages.EmployeeFolder
{
    public partial class Index : ComponentBase
    {
        [Inject] public EmployeeService EmployeeService { get; set; }
        [Inject] public TeamService TeamService { get; set; }
        [Inject] public NavigationManager NavManager { get; set; }

        protected List<Employee> employees = new();
        protected List<Employee> filteredEmployees = new();
        protected List<string> departments = new();
        protected List<string> genders = new();

        protected string selectedDepartment = "";
        protected string selectedGender = "";
        protected string selectedStatus = "";
        protected string currentSortColumn = "";
        protected bool sortAscending = true;
        protected Employee? selectedEmployee = null;

        protected int currentPage = 1;
        protected int pageSize = 50;
        protected int totalRecords;
        protected int totalPages;
        protected bool isLoading = false;
        protected string selectedTeam = "";
        protected List<string> teams = new();

        protected async Task OnDepartmentChanged()
        {
            Console.WriteLine("Department changed to " + selectedDepartment);

            selectedTeam = "";

            if (!string.IsNullOrEmpty(selectedDepartment))
            {
                var fetchedTeams = await TeamService.GetTeamsByDepartment(selectedDepartment);
                Console.WriteLine($"Fetched {fetchedTeams.Count} teams for department {selectedDepartment}");

                teams = fetchedTeams.Select(t => t.TeamName).ToList();
            }
            else
            {
                teams.Clear();
            }

            StateHasChanged();
        }

        protected async Task OnDepartmentChangedNative(ChangeEventArgs e)
        {
            selectedDepartment = e.Value?.ToString() ?? "";
            //Console.WriteLine("Native select changed to " + selectedDepartment);

            selectedTeam = "";

            if (!string.IsNullOrEmpty(selectedDepartment))
            {
                var fetchedTeams = await TeamService.GetTeamsByDepartment(selectedDepartment);
                //Console.WriteLine($"Fetched {fetchedTeams.Count} teams for department {selectedDepartment}");

                teams = fetchedTeams.Select(t => t.TeamName).ToList();
            }
            else
            {
                teams.Clear();
            }
        }

        protected override async Task OnInitializedAsync()
        {
            Stopwatch timer = new Stopwatch();
            timer.Start();
            await LoadEmployeesPaged();
            timer.Stop();
            Console.WriteLine($"time is {timer.Elapsed}");
        }
        protected bool CanPreviousPage => currentPage > 1;
        protected bool CanNextPage => currentPage < totalPages;

        protected async Task LoadEmployeesPaged()
        {
            isLoading = true;
            //await Task.Delay(5000);
            Console.WriteLine("load employee call");
            bool? statusFilter = null;
            if (!string.IsNullOrEmpty(selectedStatus))
                statusFilter = bool.Parse(selectedStatus);

            var result = await EmployeeService.GetEmployeesPaged(currentPage, pageSize,
                string.IsNullOrEmpty(selectedDepartment) ? null : selectedDepartment,
                string.IsNullOrEmpty(selectedGender) ? null : selectedGender,
                statusFilter);

            filteredEmployees = result.employees.ToList();
            departments = filteredEmployees.Select(e => e.DepartmentName).Distinct().ToList();
            genders = filteredEmployees.Select(e => e.Gender).Distinct().ToList();
            totalRecords = result.totalRecords;

            totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            isLoading = false;
        }

        protected async Task PreviousPage()
        {
            if (CanPreviousPage)
            {
                currentPage--;
                await LoadEmployeesPaged();
            }
        }

        protected async Task NextPage()
        {

            if (CanNextPage)
            {
                currentPage++;

                await LoadEmployeesPaged();
            }
        }

        protected async Task LoadEmployees()
        {
            employees = (await EmployeeService.GetAllEmployees()).ToList();
            filteredEmployees = employees.ToList();
            departments = employees.Select(e => e.DepartmentName).Distinct().ToList();
            genders = employees.Select(e => e.Gender).Distinct().ToList();
        }

        protected async Task ApplyFilters()
        {
            await LoadEmployeesPaged();
        }

        protected void NavigateToAdd()
        {
            NavManager.NavigateTo("/add");
        }

        protected void NavigateToAddMany()
        {
            NavManager.NavigateTo("/addmany");
        }
        protected async Task ResetFilters()
        {
            selectedDepartment = "";
            selectedGender = "";
            selectedStatus = "";
            selectedTeam = "";
            await LoadEmployeesPaged();
        }

        protected void SortBy(string columnName)
        {
            if (currentSortColumn == columnName)
                sortAscending = !sortAscending;
            else
            {
                currentSortColumn = columnName;
                sortAscending = true;
            }
            SortData();
        }

        protected void SortData()
        {
            var prop = typeof(Employee).GetProperty(currentSortColumn);
            if (prop == null) return;

            filteredEmployees = sortAscending
                ? filteredEmployees.OrderBy(e => prop.GetValue(e)).ToList()
                : filteredEmployees.OrderByDescending(e => prop.GetValue(e)).ToList();
        }

        protected string GetSortIcon(string columnName)
        {
            if (currentSortColumn != columnName) return "";
            return sortAscending ? "↑" : "↓";
        }

        protected void EditEmployee(int id)
        {
            NavManager.NavigateTo($"/edit/{id}");
        }

        protected async Task DeleteEmployee(int id)
        {
            await EmployeeService.DeleteEmployee(id);
            await LoadEmployees();
        }
        protected async Task ViewEmployee(int id)
        {
            selectedEmployee = await EmployeeService.GetEmployeeById(id);
        }
        protected void CloseView()
        {
            selectedEmployee = null;
            StateHasChanged();
        }
    }
}
