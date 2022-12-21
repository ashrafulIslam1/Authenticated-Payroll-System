using System.ComponentModel.DataAnnotations;

namespace Authenticated_Payroll_System.Models;

public class Employee
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public int DepartmentId { get; set; }
    public string? PresentAddress { get; set; }  
    public string? PermanentAddressBn { get; set; }
    public DateTime JoinDate { get; set; }
    public string? Email { get; set; }
    public string? MobileNo { get; set; }
    

}
