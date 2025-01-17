﻿using System;
using System.Collections.Generic;
using System.Configuration;
using Oracle.ManagedDataAccess.Client;

namespace AST_Intranet.Models.Database
{
    public class EmployeeDBConnector
    {
        // Method to get total number of employees
        public static int GetTotalEmployees()
        {
            int totalEmployees = 0;
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["OracleDbConnection"].ConnectionString;

                using (OracleConnection connection = new OracleConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT COUNT(*) FROM cim_emp_master";

                    using (OracleCommand command = new OracleCommand(query, connection))
                    {
                        totalEmployees = Convert.ToInt32(command.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching total employees: {ex.Message}");
            }
            return totalEmployees;
        }

        // Method to get new joiners (past month)
        public static int GetNewJoiners()
        {
            int newJoiners = 0;
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["OracleDbConnection"].ConnectionString;

                using (OracleConnection connection = new OracleConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT COUNT(*) FROM cim_emp_master WHERE join_date >= :joinDate";

                    using (OracleCommand command = new OracleCommand(query, connection))
                    {
                        command.Parameters.Add(new OracleParameter(":joinDate", OracleDbType.Date)).Value = DateTime.Now.AddMonths(-1);
                        newJoiners = Convert.ToInt32(command.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching new joiners: {ex.Message}");
            }
            return newJoiners;
        }

        // Method to get total number of departments
        public static int GetTotalDepartments()
        {
            int totalDepartments = 0;
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["OracleDbConnection"].ConnectionString;

                using (OracleConnection connection = new OracleConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT COUNT(*) FROM department_master";

                    using (OracleCommand command = new OracleCommand(query, connection))
                    {
                        totalDepartments = Convert.ToInt32(command.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching total departments: {ex.Message}");
            }
            return totalDepartments;
        }

        // Method to get department details
        public static List<string> GetDepartments()
        {
            List<string> departments = new List<string>();
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["OracleDbConnection"].ConnectionString;

                using (OracleConnection connection = new OracleConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT DEPT_NAME FROM department_master";

                    using (OracleCommand command = new OracleCommand(query, connection))
                    {
                        using (OracleDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                departments.Add(reader.GetString(reader.GetOrdinal("DEPT_NAME")));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching departments: {ex.Message}");
            }
            return departments;
        }
        // Method to get all employees from a specific department
        public static List<Employee> GetEmployeesByDepartment(string departmentName)
        {
            List<Employee> employees = new List<Employee>();
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["OracleDbConnection"].ConnectionString;
                using (OracleConnection connection = new OracleConnection(connectionString))
                {
                    connection.Open();
                    string query = @"SELECT e.EMP_CODE, e.EMP_NAME, e.DESIGNATION, e.DOB, e.DOJ, e.DOL, e.DOR, 
                             e.EMAIL, e.STATUS, e.PHONE_NO, e.EMP_TYPE, e.AGENCY_NAME, e.DEPUTED_FOR, 
                             e.LOCATION, e.MANAGER_CODE, e.CIM_LOGIN_PASSWORD, e.ADDRESS, e.RESIGN_SOURCE, 
                             e.EMP_GROUP, e.BLOOD_GROUP, e.EXT_NO, e.ENTITY_CODE, e.GENDER, e.REMARKS, 
                             e.EMAIL_PERSONAL
                             FROM cim_emp_master e
                             JOIN department_master d
                             ON e.DEPARTMENT = d.DEPT_ID
                             WHERE d.DEPT_NAME = :departmentName";

                    using (OracleCommand command = new OracleCommand(query, connection))
                    {
                        command.Parameters.Add(new OracleParameter(":departmentName", OracleDbType.Varchar2)).Value = departmentName;
                        using (OracleDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Employee emp = new Employee
                                {
                                    EmpCode = reader.GetInt32(reader.GetOrdinal("EMP_CODE")),
                                    EmpName = reader.GetString(reader.GetOrdinal("EMP_NAME")),
                                    Designation = reader.GetString(reader.GetOrdinal("DESIGNATION")),
                                    Dob = reader.GetDateTime(reader.GetOrdinal("DOB")),
                                    Doj = reader.GetDateTime(reader.GetOrdinal("DOJ")),
                                    Dol = reader.IsDBNull(reader.GetOrdinal("DOL")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("DOL")),
                                    Dor = reader.IsDBNull(reader.GetOrdinal("DOR")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("DOR")),
                                    Email = reader.GetString(reader.GetOrdinal("EMAIL")),
                                    Status = reader.GetInt32(reader.GetOrdinal("STATUS")),
                                    PhoneNo = reader.GetString(reader.GetOrdinal("PHONE_NO")),
                                    EmpType = reader.GetInt32(reader.GetOrdinal("EMP_TYPE")),
                                    AgencyName = reader.GetString(reader.GetOrdinal("AGENCY_NAME")),
                                    DeputedFor = reader.GetString(reader.GetOrdinal("DEPUTED_FOR")),
                                    Location = reader.GetString(reader.GetOrdinal("LOCATION")),
                                    ManagerCode = reader.GetInt32(reader.GetOrdinal("MANAGER_CODE")),
                                    CimLoginPassword = reader.GetString(reader.GetOrdinal("CIM_LOGIN_PASSWORD")),
                                    Address = reader.GetString(reader.GetOrdinal("ADDRESS")),
                                    ResignSource = reader.GetString(reader.GetOrdinal("RESIGN_SOURCE")),
                                    EmpGroup = reader.GetInt32(reader.GetOrdinal("EMP_GROUP")),
                                    BloodGroup = reader.GetString(reader.GetOrdinal("BLOOD_GROUP")),
                                    ExtNo = reader.GetString(reader.GetOrdinal("EXT_NO")),
                                    EntityCode = reader.GetInt32(reader.GetOrdinal("ENTITY_CODE")),
                                    Gender = reader.GetString(reader.GetOrdinal("GENDER")),
                                    Remarks = reader.GetString(reader.GetOrdinal("REMARKS")),
                                    EmailPersonal = reader.GetString(reader.GetOrdinal("EMAIL_PERSONAL"))
                                };
                                employees.Add(emp);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching employees by department: {ex.Message}");
            }
            return employees;
        }

    }
}
