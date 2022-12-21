using Authenticated_Payroll_System.Common_Daterange_Attribute;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Authenticated_Payroll_System.ViewModels
{
    public class AttendanceViewModel
    {
        [Display(Name = "Attendance Id")]
        public int AttendanceId { get; set; }

        [Display(Name = "Employee Id")]
        public int EmployeeId { get; set; }

        [Display(Name = "Employee Name")]
        public string? EmployeeName { get; set; }

        [Display(Name = "In Time")]
        public DateTime InTime { get; set; }

        [Display(Name = "Out Time")]
        public DateTime OutTime { get; set; }

        //[CurrentDate]
        public DateTime Date { get; set; }

        public int Status { get; set; }
    }
}
