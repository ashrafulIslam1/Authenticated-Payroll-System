using System.ComponentModel.DataAnnotations;

namespace Authenticated_Payroll_System.Models
{
    public class MonthlySalary
    {
        [Key]
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int? Year { get; set; }
        public int? Month { get; set; }
        public int PresentDays { get; set; }
        public int WorkingDays { get; set; }
        public int LeaveDays { get; set; }
        public double Basic { get; set; }
        public double HomeAllowance { get; set; }
        public double MedicalExpense { get; set; }
        public double TotalPay { get; set; }
    }
}
