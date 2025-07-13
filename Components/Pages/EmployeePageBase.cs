using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using EmployeeManagement.Web.Data.Models;
using EmployeeManagement.Web.Services;

namespace EmployeeManagement.Web.Pages
{
    public class EmployeePageBase : ComponentBase
    {
        [Inject] protected EmployeeService? EmployeeService { get; set; }
        [Inject] protected NavigationManager? NavigationManager { get; set; }
        [Inject] protected IJSRuntime? JSRuntime { get; set; }

        protected IEnumerable<Employee>? employees;

        protected override async Task OnInitializedAsync()
        {
            employees = await EmployeeService.GetAllEmployees();
        }

        protected void EditEmployee(int id)
        {
            NavigationManager.NavigateTo($"/edit/{id}");
        }

        protected async Task DeleteEmployee(int id)
        {
            await EmployeeService.DeleteEmployee(id);
            employees = await EmployeeService.GetAllEmployees();
        }

        protected void NavigateToAdd()
        {
            NavigationManager.NavigateTo("/add", forceLoad: true);
        }
    }
}
