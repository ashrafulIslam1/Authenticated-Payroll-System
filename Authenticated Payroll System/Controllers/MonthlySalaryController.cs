using Authenticated_Payroll_System.Data;
using Authenticated_Payroll_System.Models;
using Microsoft.AspNetCore.Mvc;
using Authenticated_Payroll_System.ViewModels;
using Authenticated_Payroll_System.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using System;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.AspNetCore.Identity;

namespace Authenticated_Payroll_System.Controllers
{
    [Authorize(Roles = "Employee, Hr")]
    public class MonthlySalaryController : Controller
    {
        private MonthlySalaryService _monthlysalaryService;
        private EmployeeService _employeeService;

        public MonthlySalaryController(MonthlySalaryService monthlysalaryService, EmployeeService employeeService)
        {
            _monthlysalaryService = monthlysalaryService;
            _employeeService = employeeService;
        }
        public IActionResult Index(int? employeeId, DateTime? monthYear)
        {
            int? _month = monthYear == null ? null : monthYear.Value.Month;
            int? _year = monthYear == null ? null : monthYear.Value.Year;
            ViewBag.employeelist = new SelectList(_employeeService.GetDropDown(), "Value", "Text");

            //var query = _monthlysalaryService.GetAll(employeeId, _month, _year);
            //return View(query);


            string email = User.Identity?.Name;
            if (User.IsInRole("Hr"))
            {
                var query = _monthlysalaryService.GetAll(employeeId, _month, _year);
                return View(query);
            }
            else
            {
                var query = _monthlysalaryService.GetEmployee(_month, _year, email);
                return View(query);
            }


        }

        [HttpGet]
        public IActionResult Genarate()
        {
            ViewBag.employeelist = new SelectList(_employeeService.GetDropDown(), "Value", "Text");
            return View();
        }

        [HttpPost]
        public IActionResult Genarate(int year, int month)
        {
            if (ModelState.IsValid)
            {
                _monthlysalaryService.Genarate(year, month);
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
            //return View(viewModel);
        }
    }
}
