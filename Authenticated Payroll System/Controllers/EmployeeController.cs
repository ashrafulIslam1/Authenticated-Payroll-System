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

namespace Authenticated_Payroll_System.Controllers
{
    [Authorize(Roles = "Employee, Hr")]
    public class EmployeeController : Controller
    {
        private EmployeeService _employeeService;
        private DepartmentService _departmentService;
        public EmployeeController(EmployeeService employeeService, DepartmentService departmentService)
        {
            _employeeService = employeeService;
            _departmentService = departmentService;
        }

        public IActionResult Index(string searchString, int? departmentId, string sortOrder)
        {
            var query = _employeeService.GetAll(searchString, departmentId, sortOrder);

            ViewData["currentFilter"] = searchString;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";

            ViewBag.departmentlist = new SelectList(_departmentService.GetDropDown(), "Value", "Text");

            return View(query);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.departmentlist = new SelectList(_departmentService.GetDropDown(), "Value", "Text");
            return View();
        }

        [HttpPost]
        public IActionResult Create(EmployeeViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                _employeeService.Create(viewModel);
                return RedirectToAction("Index");
            }
            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Update(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var updateEmployee = _employeeService.GetById((int)id);

            if (updateEmployee == null)
            {
                return NotFound();
            }
            ViewBag.departmentlist = new SelectList(_departmentService.GetDropDown(), "Value", "Text");
            return View(updateEmployee);
        }

        [HttpPost]
        public IActionResult Update(EmployeeViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                _employeeService.Update(viewModel);
                return RedirectToAction("Index");
            }
            return View(viewModel);
        }

        public IActionResult Delete(int? id)
        {
            if(id == null || id == 0)
            {
                return NotFound();
            }

            var deleteEmployee = _employeeService.GetById((int)id);

            if(deleteEmployee == null)
            {
                return NotFound();
            }

            return View(deleteEmployee);
        }

        [HttpPost]
        public IActionResult DeleteAll(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            _employeeService.Delete((int)id);

            return RedirectToAction("Index");
        }
    }
}
