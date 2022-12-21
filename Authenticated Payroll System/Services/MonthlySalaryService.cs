using Authenticated_Payroll_System.Data;
using Authenticated_Payroll_System.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Authenticated_Payroll_System.Models;
using System;

namespace Authenticated_Payroll_System.Services;

public class MonthlySalaryService
{
    // here I initialize an object (_dbContext) where I store all the data from database table.
    private readonly ApplicationDbContext _dbContext;

    public MonthlySalaryService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    private int GetNoOfHolidays(int year, int month)
    {
        var firstDayOfMonth = new DateTime(year, month, 1);
        var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

        int holidays = 0;

        while (firstDayOfMonth <= lastDayOfMonth)
        {
            if (firstDayOfMonth.DayOfWeek == DayOfWeek.Friday || firstDayOfMonth.DayOfWeek == DayOfWeek.Saturday)
                holidays++;
            firstDayOfMonth = firstDayOfMonth.AddDays(1);
        }

        return holidays;
    }
    public void Genarate(int year, int month)
    {
        var _existingSalaries = _dbContext.MonthlySalary.Where(x => x.Year == year && x.Month == month).ToList();

        if(_existingSalaries.Count > 0)
        {
            _dbContext.MonthlySalary.RemoveRange(_existingSalaries);
        }

        var employees = (from emp in _dbContext.Employees
                         join sal in _dbContext.SalarySetup on emp.Id equals sal.EmployeeId
                         select new
                         {
                             EmployeeId = emp.Id,
                             Basic = sal.Basic,
                             HomeAllowance = sal.HomeAllowance,
                             MedicalExpense = sal.MedicalExpense
                         }).ToList();

        var attendance = (from s in _dbContext.Attendances
                          where s.Date.Year == year && s.Date.Month == month
                          group s by s.EmployeeId into c
                          select new
                          {
                              EmployeeId = c.Key,
                              PresentDays = c.Count()
                          }).ToList();

        var leaves = (from s in _dbContext.LeaveApplications
                     where s.FromDate.Month == month && s.ToDate.Month == month && s.FromDate.Year == year && s.ToDate.Year == year
                     group s by s.EmployeeId into c
                     select new
                     {
                         EmployeeId = c.Key,
                         LeaveDays = c.Sum(x => EF.Functions.DateDiffDay(x.FromDate, x.ToDate) + 1)
                     }).ToList();

        var noOfHolidays = GetNoOfHolidays(year, month);
        var totalDaysInMonth = DateTime.DaysInMonth(year, month);

        var data = (from e in employees
                    join a in attendance on e.EmployeeId equals a.EmployeeId
                    join lv in leaves on e.EmployeeId equals lv.EmployeeId into lg
                    from l in lg.DefaultIfEmpty()
                    select new
                    {
                        EmployeeId = e.EmployeeId,
                        Basic = e.Basic,
                        HomeAllowance = e.HomeAllowance,
                        MedicalExpense = e.MedicalExpense,
                        PresentDays = a.PresentDays,
                        TotalAbsentDays = totalDaysInMonth - a.PresentDays - (l == null ? 0 : l.LeaveDays) - noOfHolidays
                    }).ToList();

        foreach (var item in data)
        {
            _dbContext.MonthlySalary.Add(new MonthlySalary
            {
                Year = year,
                Month = month,
                EmployeeId = item.EmployeeId,

                Basic = item.Basic,
                HomeAllowance = item.HomeAllowance,
                MedicalExpense = item.MedicalExpense,

                WorkingDays = totalDaysInMonth - noOfHolidays,
                PresentDays = item.PresentDays,
                LeaveDays = item.TotalAbsentDays,

                TotalPay = Math.Round(item.Basic - ((item.Basic / totalDaysInMonth) * item.TotalAbsentDays))
            });
        }

        _dbContext.SaveChanges();
    }

    public List<MonthlySalaryViewModel> GetAll(int? employeeId, int? mm, int? yy)
    {
        var query = (from s in _dbContext.MonthlySalary
                     join e in _dbContext.Employees on s.EmployeeId equals e.Id
                     select new MonthlySalaryViewModel
                     {
                         Id = s.Id,
                         EmployeeId = s.EmployeeId,
                         EmployeeName = e.Name,
                         Basic = s.Basic,
                         HomeAllowance = s.HomeAllowance,
                         MedicalExpense = s.MedicalExpense,
                         TotalPay = s.TotalPay,
                         PresentDays = s.PresentDays,
                         WorkingDays= s.WorkingDays,
                         LeaveDays= s.LeaveDays,
                         Year = s.Year,
                         Month = s.Month,
                     }).AsQueryable();

        if (employeeId != null)
        {
            query = query.Where(s => s.EmployeeId == (employeeId));
        }

        if (mm != null && yy != null)
        {
            query = query.Where(s => s.Year == yy && s.Month == mm);
        }

        return query.ToList();
    }
}
