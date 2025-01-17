using System;
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
                    string query = @"SELECT e.*, d.* 
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
                                Employee emp = new Employee();

                                // Loop through all columns in the reader and map them to the Employee object
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    string columnName = reader.GetName(i);  // Get the column name
                                    object columnValue = reader.GetValue(i); // Get the column value

                                    // Match the column name with the Employee property
                                    var property = typeof(Employee).GetProperty(columnName,
                                                System.Reflection.BindingFlags.Public |
                                                System.Reflection.BindingFlags.Instance);

                                    // If no match is found in the class, print a debug message
                                    if (property == null)
                                    {
                                        Console.WriteLine($"No matching property found for column: {columnName}");
                                    }
                                    else if (columnValue != DBNull.Value)
                                    {
                                        property.SetValue(emp, Convert.ChangeType(columnValue, property.PropertyType));
                                    }
                                }
                                employees.Add(emp); // Add the employee to the list
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
