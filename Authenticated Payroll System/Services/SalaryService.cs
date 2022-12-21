using Authenticated_Payroll_System.Data;
using Authenticated_Payroll_System.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Authenticated_Payroll_System.Models;
using System;

namespace Authenticated_Payroll_System.Services;

public class SalaryService
{
    // here I initialize an object (_dbContext) where I store all the data from database table.
    private readonly ApplicationDbContext _dbContext;

	public SalaryService(ApplicationDbContext dbContext)
	{
		_dbContext = dbContext;	
	}

	public void Create(SalaryViewModel viewModel)
	{
		var model = new Salary // here this 'Salary' is the main model.
        {
            // Here I assign the viewModel properties to the model properties
            Id = viewModel.Id,
			EmployeeId = viewModel.EmployeeId,
            Basic = viewModel.Basic,
            HomeAllowance = viewModel.HomeAllowance,
            MedicalExpense = viewModel.MedicalExpense,
        };

		_dbContext.SalarySetup.Add(model); // Here 'Salarys' is the table name
        _dbContext.SaveChanges();
	}

	public void Update(SalaryViewModel viewModel)
	{
		var model = _dbContext.SalarySetup.Find(viewModel.Id);

		if (model == null)
			throw new Exception();

		model.Id = viewModel.Id;
		//model.EmployeeId = viewModel.EmployeeId;
        model.Basic = viewModel.Basic;
        model.HomeAllowance = viewModel.HomeAllowance;
        model.MedicalExpense = viewModel.MedicalExpense;

		_dbContext.SalarySetup.Update(model);
		_dbContext.SaveChanges();
	}

	public void Delete(int id)
	{
		var model = _dbContext.SalarySetup.Find(id);

        if (model == null)
            throw new Exception();

		_dbContext.SalarySetup.Remove(model);
		_dbContext.SaveChanges();
    }

	public List<SalaryViewModel> GetAll(int? employeeId, int? mm, int? yy)
	{
		var query = (from s in _dbContext.SalarySetup
                     join e in _dbContext.Employees on s.EmployeeId equals e.Id
                     select new SalaryViewModel
					{
						Id = s.Id,
						EmployeeId = s.EmployeeId,
                        EmployeeName = e.Name,
                        Basic = s.Basic,
                        HomeAllowance = s.HomeAllowance,
                        MedicalExpense = s.MedicalExpense,
                    }).AsQueryable();

        if (employeeId != null)
        {
            query = query.Where(s => s.EmployeeId == (employeeId));
        }

        return query.ToList();
	}

    public SalaryViewModel? GetById(int id)
    {
        var data = (from s in _dbContext.SalarySetup
                    where s.Id == id
                    select new SalaryViewModel
                    {
                        Id = s.Id,
						EmployeeId = s.EmployeeId,
                        Basic = s.Basic,
                        HomeAllowance = s.HomeAllowance,
                        MedicalExpense = s.MedicalExpense,
                    }).SingleOrDefault();
        return data;
    }
}
