using Authenticated_Payroll_System.Data;
using Authenticated_Payroll_System.Models;
using Microsoft.AspNetCore.Mvc;
using Authenticated_Payroll_System.ViewModels;
using Authenticated_Payroll_System.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;
using System;

namespace Authenticated_Payroll_System.Controllers
{
    [Authorize(Roles = "Employee,HR")]
    public class AttendanceController : Controller
    {
        private AttendanceService _attendanceService;
        private EmployeeService _employeeService;

        public AttendanceController(AttendanceService attendanceService, EmployeeService employeeService)
        {
            _attendanceService = attendanceService;
            _employeeService = employeeService; 
        }

        public IActionResult Index(int? EmployeeId, DateTime? fromDate, DateTime toDate)
        {
            var query = _attendanceService.GetAll(EmployeeId, fromDate, toDate);

            ViewData["fromDate"] = fromDate;
            ViewData["toDate"] = toDate;

            ViewBag.employeelist = new SelectList(_employeeService.GetDropDown(), "Value", "Text");

            return View(query);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.employeelist = new SelectList(_employeeService.GetDropDown(), "Value", "Text");
            return View();
        }

        [HttpPost]
        public IActionResult Create(AttendanceViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                _attendanceService.Create(viewModel);
                return RedirectToAction("Index");
            }
            return View(viewModel);
        }


        public IActionResult Update(int? id) 
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var updateAttendance = _attendanceService.GetById((int)id);

            if (updateAttendance == null)
            {
                return NotFound();
            }
            ViewBag.employeelist = new SelectList(_employeeService.GetDropDown(), "Value", "Text");

            return View(updateAttendance);
        }

        [HttpPost]
        public IActionResult Update(AttendanceViewModel viewModel)
        {
            if(ModelState.IsValid)
            {
                _attendanceService.Update(viewModel);
                return RedirectToAction("Index");
            }
            return View(viewModel) ;
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var deleteAttendance = _attendanceService.GetById((int)id);

            if(deleteAttendance == null)
            {
                return NotFound();
            }

            return View(deleteAttendance);
        }

        [HttpPost]
        public IActionResult DeleteAll(int? id)
        {
            if(id == null || id == 0)
            {
                return NotFound();
            }

            _attendanceService.Delete((int)id);

            return RedirectToAction("Index");
        }
    }
}
