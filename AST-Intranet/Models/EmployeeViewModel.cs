using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AST_Intranet.Models.Database;
namespace AST_Intranet.Models
{
    public class EmployeeViewModel
    {
        public int TotalEmployees { get; set; }
        public int NewJoiners { get; set; }
        public int TotalDepartments { get; set; }
        public List<EmployeeDBConnector.Department> Departments { get; set; }
    }
}
