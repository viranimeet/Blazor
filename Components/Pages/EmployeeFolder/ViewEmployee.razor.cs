using Microsoft.AspNetCore.Components;
using EmployeeManagement.Web.Data.Models;
using EmployeeManagement.Web.Services;

namespace EmployeeManagement.Web.Components.Pages.EmployeeFolder
{
    public partial class ViewEmployee : ComponentBase
    {
        [Inject] public required EmployeeService EmployeeService { get; set; }

        [Parameter] public int EmployeeId { get; set; }
        [Parameter] public EventCallback OnClose { get; set; }

        private Employee? employee;

        protected override async Task OnInitializedAsync()
        {
            employee = await EmployeeService.GetEmployeeById(EmployeeId);
        }
    }
}
