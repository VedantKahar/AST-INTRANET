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
            return View();
        }

        public ActionResult Index()
        {
            // Fetch the required data from the database
            int totalEmployees = EmployeeDBConnector.GetTotalEmployees();
            int newJoiners = EmployeeDBConnector.GetNewJoiners();
            int totalDepartments = EmployeeDBConnector.GetTotalDepartments();
            List<EmployeeDBConnector.Department> departments = EmployeeDBConnector.GetDepartments();

            // Create the view model
            var model = new EmployeeViewModel
            {
                TotalEmployees = totalEmployees,
                NewJoiners = newJoiners,
                TotalDepartments = totalDepartments,
                Departments = departments
            };

            return View(model);  // Ensure model is passed to the view
        }


    }
}
