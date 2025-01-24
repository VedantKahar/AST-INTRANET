using System;
using System.Collections.Generic;
using System.Web.Mvc;
using AST_Intranet.Models.Database;

namespace AST_Intranet.Controllers
{
    public class UpdatesController : Controller
    {
        // GET: Updates
        public ActionResult UpdatesView()
        {
            List<string> birthdays;
            List<string> anniversaries;

            // Fetch birthdays and anniversaries using the UpdateDBConnector method
            UpdateDBConnector.GetEmployeeBirthdaysAndAnniversaries(out birthdays, out anniversaries);

            // Pass the fetched lists to the view
            ViewBag.Birthdays = birthdays;
            ViewBag.Anniversaries = anniversaries;

            return View();
        }
    }
}
