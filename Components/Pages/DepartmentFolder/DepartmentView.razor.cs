using Microsoft.AspNetCore.Components;
using EmployeeManagement.Web.Data.Models;
using EmployeeManagement.Web.Services;
using System.Threading.Tasks;

namespace EmployeeManagement.Web.Components.Pages.DepartmentFolder
{
    public partial class DepartmentView : ComponentBase
    {
        [Inject] public DepartmentService DepartmentService { get; set; }
        [Parameter] public int DepartmentId { get; set; }
        [Parameter] public EventCallback OnClose { get; set; }

        protected Department department;

        protected override async Task OnInitializedAsync()
        {
            department = await DepartmentService.GetDepartmentById(DepartmentId);
        }
    }
}
