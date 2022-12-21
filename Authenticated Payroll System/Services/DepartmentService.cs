using Authenticated_Payroll_System.Data;
using Authenticated_Payroll_System.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Authenticated_Payroll_System.Models;
using System;


namespace Authenticated_Payroll_System.Services
{
    public class DepartmentService
    {
        // here I initialize an object (_dbContext) where I store all the data from database table.
        private ApplicationDbContext _dbContext;

        public DepartmentService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Create(DepartmentViewModel viewModel)
        {
            var model = new Department // here this 'Department' is the main model.
            {
                // Here I assign the viewModel properties to model properties
                Name = viewModel.Name,
            };

            _dbContext.Departments.Add(model); // Here 'Departments' is the table name
            _dbContext.SaveChanges();
        }

        public void Update(DepartmentViewModel viewModel)
        {
            var model = _dbContext.Departments.Find(viewModel.Id);

            if (model == null)
                throw new Exception();

            model.Name = viewModel.Name;

            _dbContext.Departments.Update(model);
            _dbContext.SaveChanges();
        }

        public void Delete(int id)
        {
            var model = _dbContext.Departments.Find(id);

            if (model == null)
                throw new Exception();

            _dbContext.Departments.Remove(model);
            _dbContext.SaveChanges();
        }

        public List<DepartmentViewModel> GetAll()
        {
            var data = (from s in _dbContext.Departments
                        select new DepartmentViewModel
                        {
                            Id = s.Id,
                            Name = s.Name
                        }).ToList();

            return data;
        }

        public DepartmentViewModel? GetById(int id)
        {
            var data = (from s in _dbContext.Departments
                        where s.Id == id
                        select new DepartmentViewModel
                        {
                            Id = s.Id,
                            Name = s.Name
                        }).SingleOrDefault();

            return data;
        }

        public List<DropDownViewModel> GetDropDown()
        {
            var data = (from s in _dbContext.Departments
                        select new DropDownViewModel
                        {  
                            Value = s.Id,       
                            Text = s.Name
                        }).ToList();
            return data;
        }
    }
}
