using System;
using System.Web.Mvc;
using AST_Intranet.Models.Database;

namespace AST_Intranet.Controllers
{
    public class UpdatesController : Controller
    {
        // GET: Updates
        public ActionResult UpdatesView()
        {
            // Fetch employee birthdays from the database
            var birthdays = UpdateDBConnector.GetEmployeeBirthdays();

            // Debugging: Check the number of birthdays
            Console.WriteLine($"Number of birthdays retrieved: {birthdays.Count}");

            // Pass the birthdays to the view
            ViewBag.Birthdays = birthdays;

            return View();
        }

    }
}
