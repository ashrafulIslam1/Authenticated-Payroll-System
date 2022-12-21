using Authenticated_Payroll_System.Data;
using Authenticated_Payroll_System.Models;
using Microsoft.AspNetCore.Mvc;
using Authenticated_Payroll_System.ViewModels;
using Authenticated_Payroll_System.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using System;

namespace Authenticated_Payroll_System.Controllers
{
    public class SalaryController : Controller
    {
        private SalaryService _salaryService;
        private EmployeeService _employeeService;

        public SalaryController(SalaryService salaryService, EmployeeService employeeService)
        {
            _salaryService = salaryService;
            _employeeService = employeeService;
        }
        public IActionResult Index(int? employeeId, DateTime? monthYear)
        {  
            int? _month = monthYear == null ? null : monthYear.Value.Month;
            int? _year = monthYear == null ? null : monthYear.Value.Year;

            var query = _salaryService.GetAll(employeeId, _month, _year);

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
        public IActionResult Create(SalaryViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                _salaryService.Create(viewModel);
                return RedirectToAction("Index");
            }
            return View(viewModel);
        }

        public IActionResult Update(int id)
        {
            if (id == 0)
                return NotFound();

            var updateSalary = _salaryService.GetById(id);

            if (updateSalary == null)
            {
                return NotFound();
            }
            ViewBag.employeelist = new SelectList(_employeeService.GetDropDown(), "Value", "Text");
            return View(updateSalary);
        }

        [HttpPost]
        public IActionResult Update(SalaryViewModel viewModel)
        {
            if(ModelState.IsValid)
            {
                _salaryService.Update(viewModel);
                return RedirectToAction("Index");
            }
            
            return View(viewModel);
        }

        public IActionResult Delete(int id)
        {
            if(id == 0)
            {
                return NotFound();
            }

            var deleteSalary = _salaryService.GetById(id);

            if(deleteSalary == null)
            {
                return NotFound();
            }

            return View(deleteSalary);
        }

        [HttpPost]
        public IActionResult DeleteALL(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            _salaryService.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
