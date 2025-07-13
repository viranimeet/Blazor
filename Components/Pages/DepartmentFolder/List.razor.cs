using Microsoft.AspNetCore.Components;
using EmployeeManagement.Web.Data.Models;
using EmployeeManagement.Web.Services;
using System.Diagnostics;

namespace EmployeeManagement.Web.Components.Pages.DepartmentFolder
{
    public partial class List : ComponentBase
    {
        [Inject] public DepartmentService DepartmentService { get; set; }
        [Inject] public NavigationManager NavManager { get; set; }

        protected List<Department> department = new();
        protected bool showView = false;
        protected int? selectedDepartmentId;

        protected override async Task OnInitializedAsync()
        {
            Stopwatch timer = new Stopwatch();
            timer.Start();
            await LoadDepartments();
            timer.Stop();
            Console.WriteLine($"time department {timer.Elapsed}");
        }

        protected async Task LoadDepartments()
        {
            department = (await DepartmentService.GetAllDepartments()).ToList();
        }

        protected void ViewDepartment(int id)
        {
            selectedDepartmentId = id;
            showView = true;
        }
        protected void NavigateToAdd()
        {
            NavManager.NavigateTo("/add-Department");
        }

        protected void CloseView()
        {
            showView = false;
            selectedDepartmentId = null;
        }

        protected void EditDepartment(int id)
        {
            NavManager.NavigateTo($"/edit-department/{id}");
        }

        protected async Task DeleteDepartment(int id)
        {
            await DepartmentService.DeleteDepartment(id);
            await LoadDepartments();
        }
    }
}



