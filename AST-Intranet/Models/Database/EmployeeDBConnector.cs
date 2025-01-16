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
                    connection.Open(); // Open connection
                    string query = "SELECT COUNT(*) FROM cim_emp_master";

                    using (OracleCommand command = new OracleCommand(query, connection))
                    {
                        totalEmployees = Convert.ToInt32(command.ExecuteScalar());
                    }
                }
            }
            catch (OracleException ex)
            {
                Console.WriteLine($"Oracle Database Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            return totalEmployees;
        }

        // Method to get new joiners (for simplicity, using a fixed number of days for recent joiners)
        public static int GetNewJoiners()
        {
            int newJoiners = 0;
            DateTime dateThreshold = DateTime.Now.AddMonths(-1);  // Example: Consider joiners in the last month

            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["OracleDbConnection"].ConnectionString;

                using (OracleConnection connection = new OracleConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT COUNT(*) FROM cim_emp_master WHERE join_date >= :joinDate";

                    using (OracleCommand command = new OracleCommand(query, connection))
                    {
                        command.Parameters.Add(new OracleParameter(":joinDate", OracleDbType.Date)).Value = dateThreshold;
                        newJoiners = Convert.ToInt32(command.ExecuteScalar());
                    }
                }
            }
            catch (OracleException ex)
            {
                Console.WriteLine($"Oracle Database Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
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
                    connection.Open(); // Open connection
                    string query = "SELECT COUNT(*) FROM department_master";

                    using (OracleCommand command = new OracleCommand(query, connection))
                    {
                        totalDepartments = Convert.ToInt32(command.ExecuteScalar());
                    }
                }
            }
            catch (OracleException ex)
            {
                Console.WriteLine($"Oracle Database Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            return totalDepartments;
        }

        // Method to get all departments
        public static List<Department> GetDepartments()
        {
            List<Department> departments = new List<Department>();

            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["OracleDbConnection"].ConnectionString;

                using (OracleConnection connection = new OracleConnection(connectionString))
                {
                    connection.Open(); // Open connection
                    string query = "SELECT DEPT_NAME FROM department_master"; // Only select DEPT_NAME

                    using (OracleCommand command = new OracleCommand(query, connection))
                    {
                        using (OracleDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                departments.Add(new Department
                                {
                                    DEPT_NAME = reader.GetString(reader.GetOrdinal("DEPT_NAME"))
                                });
                            }
                        }
                    }
                }
            }
            catch (OracleException ex)
            {
                Console.WriteLine($"Oracle Database Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            return departments;
        }

        // Model for Department
        public class Department
        {
            public string DEPT_NAME { get; set; }
        }

    }
}