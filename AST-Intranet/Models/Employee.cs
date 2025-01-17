using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AST_Intranet.Models
{
    public class Employee
    {
        public int EmpCode { get; set; }
        public string EmpName { get; set; }
        public string Designation { get; set; }
        public DateTime Dob { get; set; }
        public DateTime Doj { get; set; }
        public DateTime? Dol { get; set; }
        public DateTime? Dor { get; set; }
        public string Email { get; set; }
        public int Status { get; set; }
        public string PhoneNo { get; set; }
        public int EmpType { get; set; }
        public string AgencyName { get; set; }
        public string DeputedFor { get; set; }
        public string Location { get; set; }
        public int ManagerCode { get; set; }
        public string CimLoginPassword { get; set; }
        public string Address { get; set; }
        public string ResignSource { get; set; }
        public int EmpGroup { get; set; }
        public string BloodGroup { get; set; }
        public string ExtNo { get; set; }
        public int EntityCode { get; set; }
        public string Gender { get; set; }
        public string Remarks { get; set; }
        public string EmailPersonal { get; set; }
    }
}