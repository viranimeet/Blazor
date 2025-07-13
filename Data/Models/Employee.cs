using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Web.Data.Models;

public class Employee
{
    public int EmployeeId { get; set; }

    [Required(ErrorMessage = "First name is required")]
    [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "Last name is required")]
    [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters")]
    public string LastName { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Gender { get; set; }
    public DateTime DateOfBirth { get; set; }
    [Required(ErrorMessage = "Department is required")]
    public int? DepartmentId { get; set; }
    public bool IsActive { get; set; }
    public DateTime JoinDate { get; set; }
    public string AadharPath { get; set; }
    public bool TermsAccepted { get; set; }
    public string DepartmentName { get; set; }
}