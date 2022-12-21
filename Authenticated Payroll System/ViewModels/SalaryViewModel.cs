using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Authenticated_Payroll_System.ViewModels
{
    public class SalaryViewModel
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }

        [Display(Name = "Employee Name")]
        public string? EmployeeName { get; set; }

        public double Basic { get; set; }
        public double HomeAllowance { get; set; }
        public double MedicalExpense { get; set; }
    }
}
