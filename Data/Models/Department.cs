using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Web.Data.Models;

public class Department
{
    public int DepartmentId { get; set; }
    [Required(ErrorMessage = "Department Name is required")]
    public string? DepartmentName { get; set; }
    [Required(ErrorMessage = "Description is required")]
    public string? Description { get; set; }
}