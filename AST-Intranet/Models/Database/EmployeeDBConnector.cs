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
                    string query = "SELECT COUNT(*) FROM cim_emp_master_list WHERE STATUS = 'Active'";

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
                    string query = "SELECT COUNT(*) FROM cim_emp_master_list WHERE join_date >= :joinDate";

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
                    string query = "SELECT COUNT (DISTINCT DEPARTMENT) FROM  cim_emp_master_list";

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

        public static List<string> GetDepartments()
        {
            List<string> departments = new List<string>();
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["OracleDbConnection"].ConnectionString;

                using (OracleConnection connection = new OracleConnection(connectionString))
                {
                    connection.Open();
                    // Select distinct department names from cim_emp_master_list
                    string query = "SELECT DISTINCT DEPARTMENT FROM cim_emp_master_list WHERE DEPARTMENT IS NOT NULL";

                    using (OracleCommand command = new OracleCommand(query, connection))
                    {
                        using (OracleDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var departmentName = reader.GetString(reader.GetOrdinal("DEPARTMENT"));
                                Console.WriteLine($"Found Department: {departmentName}");  // Debugging line
                                departments.Add(departmentName);
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


        public static List<Dictionary<string, object>> GetEmployeesByDepartment(string departmentName, int page, int pageSize)
        {
            List<Dictionary<string, object>> employees = new List<Dictionary<string, object>>();

            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["OracleDbConnection"].ConnectionString;

                using (OracleConnection connection = new OracleConnection(connectionString))
                {
                    connection.Open();

                    // Calculate the starting row for pagination
                    int startRow = (page - 1) * pageSize;

                    // Query for employees in a specific department using department name
                      string query = @"
                            SELECT * 
                            FROM (
                                SELECT emp.*, ROWNUM AS rn 
                                FROM cim_emp_master_list emp 
                                WHERE emp.DEPARTMENT = :departmentName AND emp.STATUS = 'Active'
                            ) 
                            WHERE rn BETWEEN :startRow AND :endRow";

                    using (OracleCommand command = new OracleCommand(query, connection))
                    {
                        command.Parameters.Add(new OracleParameter("departmentName", departmentName));
                        command.Parameters.Add(new OracleParameter("startRow", startRow + 1));  // Oracle ROWNUM starts from 1
                        command.Parameters.Add(new OracleParameter("endRow", startRow + pageSize));

                        using (OracleDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var employee = new Dictionary<string, object>();
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    string columnName = reader.GetName(i);
                                    object columnValue = reader.GetValue(i);
                                    employee.Add(columnName, columnValue);
                                }

                                employees.Add(employee);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching employees: {ex.Message}");
            }

            return employees;
        }



        public static int GetTotalEmployeesInDepartment(string departmentName)
        {
            int totalEmployees = 0;
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["OracleDbConnection"].ConnectionString;

                using (OracleConnection connection = new OracleConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT COUNT(*) FROM cim_emp_master_list WHERE DEPARTMENT = :departmentName and STATUS = 'Active' ";

                    using (OracleCommand command = new OracleCommand(query, connection))
                    {
                        command.Parameters.Add(new OracleParameter("departmentName", departmentName));
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


    }
}
