using System.ComponentModel.DataAnnotations;
using Authenticated_Payroll_System.Common_Daterange_Attribute;

namespace Authenticated_Payroll_System.ViewModels
{
    public class LeaveApllicationViewModel
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }

        [Display(Name = "Employee Name")]
        public string? EmployeeName { get; set; }

        [Display(Name = "Type of Leave")]
        public string? LeaveType { get; set; }

        [Display(Name = "Reason Of Leave")]
        public string? ReasonOfLeave { get; set; }

        [CurrentorGreaterDate]
        public DateTime FromDate { get; set; }

        [CurrentorGreaterDate]
        public DateTime ToDate { get; set; }

        public DateTime ApplicationDate { get; set; }

        public DateTime? ApprovalDate { get; set; }

        [Display(Name = "Approaved By")]
        public string? ApproavedBy { get; set; }

        public string? Email { get; set; }

    }
}
