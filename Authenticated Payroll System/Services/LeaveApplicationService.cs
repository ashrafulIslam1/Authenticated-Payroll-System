using Authenticated_Payroll_System.Data;
using Authenticated_Payroll_System.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Authenticated_Payroll_System.Models;
using System;

namespace Authenticated_Payroll_System.Services;

public class LeaveApplicationService
{
    // here I initialize an object (_dbContext) where I store all the data from database table.
    private readonly ApplicationDbContext _dbContext;

	public LeaveApplicationService(ApplicationDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public void Create(LeaveApllicationViewModel viewModel)
	{
		var model = new LeaveApplication // here this 'LeaveApplication' is the main model.
        {
            // Here I assign the viewModel properties to the model properties
            Id = viewModel.Id,
			EmployeeId = viewModel.EmployeeId,
			ApplicationDate = DateTime.Now,
			FromDate = viewModel.FromDate,
			ToDate = viewModel.ToDate,
			ReasonOfLeave = viewModel.ReasonOfLeave,
			LeaveType = viewModel.LeaveType,
            isApproved = 0,
		};

		_dbContext.LeaveApplications.Add(model); // Here 'LeaveApplications' is the table name
        _dbContext.SaveChanges();
	}

	public void Update(LeaveApllicationViewModel viewModel)
	{
		var model = _dbContext.LeaveApplications.Find(viewModel.Id);

		if (model == null)
			throw new Exception();

		model.ApplicationDate = DateTime.Now;
		model.FromDate = viewModel.FromDate;
		model.ToDate = viewModel.ToDate;
		model.ReasonOfLeave = viewModel.ReasonOfLeave;
		model.LeaveType = viewModel.LeaveType;

		_dbContext.LeaveApplications.Update(model);
		_dbContext.SaveChanges();
	}

	public void Delete(int id)
	{
		var model = _dbContext.LeaveApplications.Find(id);

		if (model == null)
			throw new Exception();

		_dbContext.LeaveApplications.Remove(model);
		_dbContext.SaveChanges();
	}

	public List<LeaveApllicationViewModel> GetAll(int? EmployeeId, DateTime? fromDate, DateTime? toDate)
	{
		var query = (from s in _dbContext.LeaveApplications
                     join e in _dbContext.Employees on s.EmployeeId equals e.Id
                     select new LeaveApllicationViewModel
					{
						Id = s.Id,
						EmployeeId = s.EmployeeId,
                        EmployeeName = e.Name,
                        ApplicationDate = s.ApplicationDate,
						FromDate = s.FromDate,
						ToDate = s.ToDate,
						ReasonOfLeave = s.ReasonOfLeave,
						LeaveType = s.LeaveType,
						ApprovalDate = s.ApprovalDate,
						ApproavedBy = s.ApproavedBy,
                        isApproved  = s.isApproved,
					}).AsQueryable();

        if (EmployeeId != null)
        {
            query = query.Where(s => s.EmployeeId == (EmployeeId));
        }
        if (fromDate.HasValue && toDate.HasValue)
		{
            query = query.Where(s => s.FromDate >= fromDate && s.ToDate <= toDate);
        }
        return query.ToList();
	}

    public List<LeaveApllicationViewModel> GetEmployee(DateTime? fromDate, DateTime? toDate, String userName)
    {
        var query = (from s in _dbContext.LeaveApplications
                     join e in _dbContext.Employees on s.EmployeeId equals e.Id
                     select new LeaveApllicationViewModel
                     {
                         Id = s.Id,
                         EmployeeId = s.EmployeeId,
                         EmployeeName = e.Name,
                         Email = e.Email,
                         ApplicationDate = s.ApplicationDate,
                         FromDate = s.FromDate,
                         ToDate = s.ToDate,
                         ReasonOfLeave = s.ReasonOfLeave,
                         LeaveType = s.LeaveType,
                         ApprovalDate = s.ApprovalDate,
                         ApproavedBy = s.ApproavedBy,
                         isApproved = s.isApproved,
                     }).AsQueryable();

        if (userName != null)
        {
            query = query.Where(s => s.Email == (userName));
        }
        if (fromDate.HasValue && toDate.HasValue)
        {
            query = query.Where(s => s.FromDate >= fromDate && s.ToDate <= toDate);
        }
        return query.ToList();
    }

    public LeaveApllicationViewModel? GetById(int id)
	{
        var data = (from s in _dbContext.LeaveApplications
					where s.Id == id
                    select new LeaveApllicationViewModel
                    {
                        Id = s.Id,
                        EmployeeId = s.EmployeeId,
                        ApplicationDate = s.ApplicationDate,
                        FromDate = s.FromDate,
                        ToDate = s.ToDate,
                        ReasonOfLeave = s.ReasonOfLeave,
                        LeaveType = s.LeaveType,
                        ApprovalDate = s.ApprovalDate,
                        ApproavedBy = s.ApproavedBy,

                    }).SingleOrDefault();
        return data;
    }

    public void ApprovedByHr(int? id)
    {
        var model = _dbContext.LeaveApplications.Find(id);
  
        if (model != null)
        {
            model.isApproved = 1;
            model.ApprovalDate = DateTime.Now;
            model.ApproavedBy = "HR";

            _dbContext.LeaveApplications.Update(model);
            _dbContext.SaveChanges();
        }
        
    }

    public void RejectByHr(int? id)
    {
        var model = _dbContext.LeaveApplications.Find(id);

        if (model != null)
        {
            model.isApproved = 2;
            model.ApprovalDate = DateTime.Now;
            model.ApproavedBy = "HR";

            _dbContext.LeaveApplications.Update(model);
            _dbContext.SaveChanges();
        }

    }
}
