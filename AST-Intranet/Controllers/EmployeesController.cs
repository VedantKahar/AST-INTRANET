using System.Web.Mvc;
using AST_Intranet.Models;
using AST_Intranet.Models.Database;
using System.Collections.Generic;

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
        public JsonResult GetEmployeesByDepartment(string departmentName)
        {
            var employees = EmployeeDBConnector.GetEmployeesByDepartment(departmentName);
            return Json(employees, JsonRequestBehavior.AllowGet);
        }

    }
}
