using System.ComponentModel.DataAnnotations;

namespace Authenticated_Payroll_System.Models;

public class Salary
{
    [Key]
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public double Basic { get; set; }
    public double HomeAllowance { get; set; }
    public double MedicalExpense { get; set; }

}
