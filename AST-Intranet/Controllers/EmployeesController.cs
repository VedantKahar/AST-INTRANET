using System;
using System.Web.Mvc;
using AST_Intranet.Models;
using AST_Intranet.Models.Database;
using System.Collections.Generic;
using System.Linq;

namespace AST_Intranet.Controllers
{
    public class EmployeesController : Controller
    {
        // GET: employeesView
        public ActionResult employeesView()
        {
            // Fetch data from the database
            int totalEmployees = EmployeeDBConnector.GetTotalEmployees();
            int newJoiners = EmployeeDBConnector.GetNewJoiners();
            int totalDepartments = EmployeeDBConnector.GetTotalDepartments();
            var departments = EmployeeDBConnector.GetDepartments();

            // Pass data to the view using ViewBag
            ViewBag.TotalEmployees = totalEmployees;
            ViewBag.NewJoiners = newJoiners;
            ViewBag.TotalDepartments = totalDepartments;
            ViewBag.Departments = departments;

            return View();
        }

        // GET: Employees by Department
        // Action to display employees by department
        public ActionResult EmployeesByDepartment(string departmentName)
        {
            // Debug the department name
            Console.WriteLine($"Department Name: {departmentName}");

            // Fetch the employees based on the department name
            var employees = EmployeeDBConnector.GetEmployeesByDepartment(departmentName);

            Console.WriteLine($"Employees Count for {departmentName}: {employees.Count}");

            // Pass the list of employees and department name to the view
            ViewBag.DepartmentName = departmentName;
            return View(employees);
        }

    }
}