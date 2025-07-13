using Microsoft.AspNetCore.Components;
using EmployeeManagement.Web.Data.Models;
using EmployeeManagement.Web.Services;
using System.Threading.Tasks;

namespace EmployeeManagement.Web.Components.Pages.DepartmentFolder
{
    public partial class AddEditDepartment : ComponentBase
    {
        [Parameter] public int? DepartmentId { get; set; }
        [Inject] public DepartmentService DepartmentService { get; set; }
        [Inject] public NavigationManager NavManager { get; set; }

        protected Department department = new();
        protected bool IsEditMode => DepartmentId.HasValue;

        protected override async Task OnInitializedAsync()
        {
            if (IsEditMode)
            {
                department = await DepartmentService.GetDepartmentById(DepartmentId.Value);
            }
            else
            {
                department = new Department();
            }
        }
        protected async Task HandleValidSubmit()
        {
            if (IsEditMode)
            {
                await DepartmentService.UpdateDepartment(department);
            }
            else
            {
                await DepartmentService.AddDepartment(department);
            }
            NavManager.NavigateTo("/departments");
        }
    }
}
