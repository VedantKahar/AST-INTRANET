using System;
using System.Web.Mvc;
using AST_Intranet.Models;
using AST_Intranet.Models.Database;
using System.Collections.Generic;
using System.Linq;
using Oracle.ManagedDataAccess.Client;
using System.Configuration;

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

        // Action method to get employees by department
        public ActionResult GetEmployeesByDepartment(string departmentName, int page = 1)
        {
            int departmentId = GetDepartmentIdByName(departmentName);

            if (departmentId == 0)
            {
                return Content("Department not found");
            }

            // Define page size (15 employees per page)
            int pageSize = 15;

            // Get the total number of employees
            var totalEmployees = EmployeeDBConnector.GetTotalEmployeesInDepartment(departmentId);

            // Get the employees for the current page
            var employees = EmployeeDBConnector.GetEmployeesByDepartment(departmentId, page, pageSize);

            if (employees == null || !employees.Any())
            {
                return Content("No employees found for this department.");
            }

            // Calculate total pages
            var totalPages = (int)Math.Ceiling((double)totalEmployees / pageSize);

            // Pass data to the view
            ViewBag.DepartmentName = departmentName;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return View("_EmployeeList", employees);
        }


        // Helper method to get department ID from department name
        private int GetDepartmentIdByName(string departmentName)
        {
            int departmentId = 0; // Default to 0 if not found

            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["OracleDbConnection"].ConnectionString;

                using (OracleConnection connection = new OracleConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT DEPT_ID FROM department_master WHERE DEPT_NAME = :departmentName";

                    using (OracleCommand command = new OracleCommand(query, connection))
                    {
                        command.Parameters.Add(new OracleParameter("departmentName", departmentName));

                        object result = command.ExecuteScalar();
                        if (result != DBNull.Value && result != null)
                        {
                            departmentId = Convert.ToInt32(result); // Convert to integer if found
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching department ID: {ex.Message}");
            }

            return departmentId; // Return the department ID or 0 if not found
        }
    }
}