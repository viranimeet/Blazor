using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using EmployeeManagement.Web.Data.Models;
using EmployeeManagement.Web.Services;

namespace EmployeeManagement.Web.Components.Pages.EmployeeFolder
{
    public class AddEditBase : ComponentBase
    {
        [Parameter]
        public int EmployeeId { get; set; }
        [Inject] protected EmployeeService? EmployeeService { get; set; }
        [Inject] protected DepartmentService? DepartmentService { get; set; }
        [Inject] protected FileUploadService? FileUploadService { get; set; }
        [Inject] protected NavigationManager? NavigationManager { get; set; }
        [Inject] protected IJSRuntime? JSRuntime { get; set; }
        protected Department department = new();
        protected Employee employee = new();
        protected List<Department> departments = new();
        protected string Title = "Add Employee";
        protected IBrowserFile? selectedFile;

        protected override async Task OnInitializedAsync()
        {
            departments = (await DepartmentService.GetAllDepartments()).ToList();

            if (EmployeeId > 0)
            {
                Title = "Edit Employee";
                employee = await EmployeeService.GetEmployeeById(EmployeeId);
            }
            else
            {
                employee.DateOfBirth = DateTime.Now;
                employee.IsActive = true;
            }
        }

        protected async Task HandleValidSubmit()
        {
            try
            {
                if (selectedFile != null)
                {
                    if (selectedFile.Size > 5 * 1024 * 1024)
                    {
                        await JSRuntime.InvokeVoidAsync("alert", "File size must be less than 5MB");
                        return;
                    }

                    using var stream = selectedFile.OpenReadStream();
                    employee.AadharPath = await FileUploadService.UploadFileAsync(
                        stream,
                        Path.GetFileName(selectedFile.Name)
                    );
                }

                if (EmployeeId > 0)
                {
                    await EmployeeService.UpdateEmployee(employee);
                }
                else
                {
                    await EmployeeService.CreateEmployee(employee);
                }

                NavigationManager.NavigateTo("/employees", forceLoad: true);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error saving employee: {ex}");
                await JSRuntime.InvokeVoidAsync("alert", "Error saving employee data. Please try again.");
            }
        }

        protected void HandleFileUpload(InputFileChangeEventArgs e)
        {
            selectedFile = e.File;
        }

        protected void Cancel()
        {
            NavigationManager.NavigateTo("/employees");
        }
    }
}
