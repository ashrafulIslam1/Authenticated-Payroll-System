using Authenticated_Payroll_System.Data;
using Authenticated_Payroll_System.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Authenticated_Payroll_System.Models;
using System;

namespace Authenticated_Payroll_System.Services;

public class EmployeeService
{
    // here I initialize an object (_dbContext) where I store all the data from database table.
    private readonly ApplicationDbContext _dbContext;

    public EmployeeService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void Create(EmployeeViewModel viewModel)
    {
        var model = new Employee // here this 'Employee' is the main model.
        {
            // Here I assign the viewModel properties to the model properties
            Name = viewModel.Name,
            Id = viewModel.Id,
            DepartmentId = viewModel.DepartmentId,
            PresentAddress = viewModel.PresentAddress,
            PermanentAddressBn = viewModel.PermanentAddressBn,
            JoinDate = viewModel.JoinDate,
            Email = viewModel.Email,
            MobileNo = viewModel.MobileNo,
        };
        _dbContext.Employees.Add(model); // Here 'Employees' is the table name
        _dbContext.SaveChanges();
    }

    public void Update(EmployeeViewModel viewModel)
    {
        var model = _dbContext.Employees.Find(viewModel.Id);

        if (model == null)
            throw new Exception();

        model.Name = viewModel.Name;
        model.Id = viewModel.Id;
        //model.Dept = viewModel.Dept;
        model.DepartmentId = viewModel.DepartmentId;
        model.PresentAddress = viewModel.PresentAddress;
        model.JoinDate = viewModel.JoinDate;
        model.PresentAddress = viewModel.PresentAddress;
        model.PermanentAddressBn = viewModel.PermanentAddressBn;
        model.MobileNo = viewModel.MobileNo;
        model.Email = viewModel.Email;

        _dbContext.Employees.Update(model);
        _dbContext.SaveChanges();
    }

    public void Delete(int id)
    {
        var model = _dbContext.Employees.Find(id);

        if (model == null)
            throw new Exception();

        _dbContext.Employees.Remove(model);
        _dbContext.SaveChanges();
    }

    public List<EmployeeViewModel> GetAll(string searchString, int? DepartmentId, string sortOrder)
    {
        var query = (from s in _dbContext.Employees
                     join d in _dbContext.Departments on s.DepartmentId equals d.Id
                     select new EmployeeViewModel
                     {
                         Name = s.Name,
                         DepartmentId = s.DepartmentId,
                         DepartmentName = d.Name,
                         Id = s.Id,
                         MobileNo = s.MobileNo,
                         PermanentAddressBn = s.PermanentAddressBn,
                         PresentAddress = s.PresentAddress,
                         Email = s.Email,
                         JoinDate = s.JoinDate
                     }).AsQueryable();

        if (!string.IsNullOrEmpty(searchString))
        {
            query = query.Where(s => s.Name.Contains(searchString));
        }

        if(DepartmentId != null)
        {
            query = query.Where(s => s.DepartmentId == (DepartmentId));
        }

        switch (sortOrder)
        {
            case "name_desc":
                query = query.OrderByDescending(s => s.Name);
                break;
            case "Date":
                query = query.OrderBy(s => s.JoinDate);
                break;
            case "date_desc":
                query = query.OrderByDescending(s => s.JoinDate);
                break;
            default:
                query = query.OrderBy(s => s.Name);
                break;
        }
        return query.ToList();
    }

    public EmployeeViewModel? GetById(int id)
    {
        var data = (from s in _dbContext.Employees
                    where s.Id == id
                    select new EmployeeViewModel
                    {
                        Name = s.Name,
                        //Dept = s.Dept,
                        DepartmentId = s.DepartmentId,
                        Id = s.Id,
                        MobileNo = s.MobileNo,
                        PermanentAddressBn = s.PermanentAddressBn,
                        PresentAddress = s.PresentAddress,
                        Email = s.Email,
                        JoinDate = s.JoinDate
                    }).SingleOrDefault();
        return data;
    }

    public List<DropDownViewModel> GetDropDown()
    {
        var data = (from s in _dbContext.Employees
                    select new DropDownViewModel
                    {
                        Value = s.Id,
                        Text = s.Name
                    }).ToList();
        return data;
    }
}
