namespace Authenticated_Payroll_System.Models;

public class Attendance
{
    public int AttendanceId { get; set; }
    public int EmployeeId { get; set; }
    public DateTime InTime { get; set; }
    public DateTime OutTime { get; set; }
    public DateTime Date { get; set; }
}
