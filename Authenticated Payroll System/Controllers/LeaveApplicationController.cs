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
    public class LeaveApplicationController : Controller
    {
        private LeaveApplicationService _leaveApplicationService;
        private EmployeeService _employeeService;

        public LeaveApplicationController(LeaveApplicationService leaveApplicationService, EmployeeService employeeService)
        {
            _leaveApplicationService = leaveApplicationService;
            _employeeService = employeeService;
        }

        public IActionResult Index(int? EmployeeId, DateTime? fromDate, DateTime? toDate)
        {
            ViewBag.employeelist = new SelectList(_employeeService.GetDropDown(), "Value", "Text");
            ViewData["fromDate"] = fromDate;
            ViewData["toDate"] = toDate;

            //var query = _leaveApplicationService.GetAll(EmployeeId, fromDate, toDate);
            //return View(query);

            //string email = User.UserName;

            string email = User.Identity?.Name;

            if (User.IsInRole("Hr"))
            {
                var query = _leaveApplicationService.GetAll(EmployeeId, fromDate, toDate);
                return View(query);
            }
            else
            {
                var query = _leaveApplicationService.GetEmployee(fromDate, toDate, email);
                return View(query);
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.employeelist = new SelectList(_employeeService.GetDropDown(), "Value", "Text");

            List<SelectListItem> leaveList = new List<SelectListItem>()
            {
                new SelectListItem { Text = "Medical Leave"},
                new SelectListItem { Text = "Maternity Leave"},
                new SelectListItem { Text = "Casual Leave"},
                new SelectListItem { Text = "Wthout Leave"},
                new SelectListItem { Text = "Earning Leave"},
            };
            ViewBag.Leave = leaveList;
            return View();
        }

        [HttpPost]
        public IActionResult Create(LeaveApllicationViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                _leaveApplicationService.Create(viewModel);
                return RedirectToAction("Index");
            }
            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            if(id == 0)
            {
                return NotFound();
            }
            var updateLeaveApplicatioin = _leaveApplicationService.GetById(id);

            if(updateLeaveApplicatioin == null)
            {
                return NotFound();
            }

            List<SelectListItem> leaveList = new List<SelectListItem>()
            {
                new SelectListItem { Text = "Medical Leave"},
                new SelectListItem { Text = "Maternity Leave"},
                new SelectListItem { Text = "Casual Leave"},
                new SelectListItem { Text = "Wthout Leave"},
                new SelectListItem { Text = "Earning Leave"},
            };
            ViewBag.Leave = leaveList;

            ViewBag.employeelist = new SelectList(_employeeService.GetDropDown(), "Value", "Text");
            return View(updateLeaveApplicatioin);
        }

        [HttpGet]
        public void ApprovedByHr(int id)
        {
            if (id == 0)
            {
                return;
            }
            var updateLeaveApplicatioin = _leaveApplicationService.GetById(id);

            if (updateLeaveApplicatioin == null)
            {
                return;
            }
            _leaveApplicationService.ApprovedByHr(id);
        }

        [HttpGet]
        public void RejectByHr(int id)
        {
            if (id == 0)
            {
                return;
            }
            var updateLeaveApplicatioin = _leaveApplicationService.GetById(id);

            if (updateLeaveApplicatioin == null)
            {
                return;
            }
            _leaveApplicationService.RejectByHr(id);
        }

        [HttpPost]
        public IActionResult Update(LeaveApllicationViewModel viewModel)
        {
            if(ModelState.IsValid)
            {
                _leaveApplicationService.Update(viewModel);
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
            var deleteApplication = _leaveApplicationService.GetById(id);

            if(deleteApplication == null)
            {
                return NotFound();
            }

            return View(deleteApplication);
        }

        [HttpPost]
        public IActionResult DeleteAll(int id)
        {
            if(id == 0)
            {
                return NotFound();
            }
            _leaveApplicationService.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
