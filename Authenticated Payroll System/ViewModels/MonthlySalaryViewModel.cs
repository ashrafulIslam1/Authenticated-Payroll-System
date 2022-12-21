using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Authenticated_Payroll_System.ViewModels
{
    public class MonthlySalaryViewModel
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }

        [Display(Name = "Employee Name")]
        public string? EmployeeName { get; set; }
        public int PresentDays { get; set; }
        public int WorkingDays { get; set; }
        public int LeaveDays { get; set; }
        public int? Month { get; set; }
        public int? Year { get; set; }
        public double Basic { get; set; }
        public double HomeAllowance { get; set; }
        public double MedicalExpense { get; set; }

        public double TotalPay { get; set; }
    }
}
