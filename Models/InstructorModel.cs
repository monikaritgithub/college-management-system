using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;


namespace OracleSampleProject.Models
{
    public class InstructorModel
    {
        public int InstructorId { get; set; }
        public string InstructorName { get; set; }
        public string InstructorCountry { get; set; }
        public string ContactNo { get; set; }
        public string Email { get; set; }
        public string CourseId { get; set; }
        public string ConnectionString { get; }

        public List<InstructorModel> InstructorList = new List<InstructorModel>();

        public InstructorModel()
        {
            // Default connection string
            ConnectionString = "User Id=icp;Password=123;Data Source=localhost:1521/orcl;";
        }

        public InstructorModel(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public void GetInstructors()
        {
            try
            {
                using (OracleConnection con = new OracleConnection(ConnectionString))
                {
                    string queryString = "SELECT InstructorId, InstructorName, InstructorCountry, ContactNo, Email, CourseId FROM Instructor";
                    OracleCommand cmd = new OracleCommand(queryString, con);
                    cmd.BindByName = true;
                    cmd.CommandType = CommandType.Text;

                    con.Open();
                    OracleDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        InstructorModel instructor = new InstructorModel
                        {
                            InstructorId = reader.GetInt32(0),
                            InstructorName = reader.GetString(1),
                            InstructorCountry = reader.GetString(2),
                            ContactNo = reader.GetString(3),
                            Email = reader.GetString(4),
                            CourseId = reader.GetString(5)
                        };

                        InstructorList.Add(instructor);
                    }

                    reader.Dispose();
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private int GetLastInstructorId(OracleConnection con)
        {
            string queryString = "SELECT MAX(InstructorId) FROM Instructor";

            using (OracleCommand cmd = new OracleCommand(queryString, con))
            {
                con.Open();
                object result = cmd.ExecuteScalar();
                con.Close();

                if (result == DBNull.Value || result == null)
                {
                    // If the result is DBNull or null, return a default value (e.g., 0 or 1)
                    return 0;
                }
                else
                {
                    // Convert the result to int
                    return Convert.ToInt32(result);
                }
            }
        }
        public InstructorModel GetInstructorById(int id)
        {
            try
            {
                using (OracleConnection con = new OracleConnection(ConnectionString))
                {
                    string queryString = "SELECT InstructorId, InstructorName, InstructorCountry, ContactNo, Email, CourseId FROM Instructor WHERE InstructorId = :id";
                    OracleCommand cmd = new OracleCommand(queryString, con);
                    cmd.Parameters.Add(":id", OracleDbType.Int32).Value = id;
                    cmd.BindByName = true;
                    cmd.CommandType = CommandType.Text;

                    con.Open();
                    OracleDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        InstructorModel instructor = new InstructorModel
                        {
                            InstructorId = reader.GetInt32(0),
                            InstructorName = reader.GetString(1),
                            InstructorCountry = reader.GetString(2),
                            ContactNo = reader.GetString(3),
                            Email = reader.GetString(4),
                            CourseId = reader.GetString(5)
                        };

                        reader.Dispose();
                        con.Close();
                        return instructor;
                    }
                    else
                    {
                        reader.Dispose();
                        con.Close();
                        return null; // Instructor not found
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null; // Handle exception as needed
            }
        }
        public void UpdateInstructor(InstructorModel instructor)
        {
            try
            {
                using (OracleConnection con = new OracleConnection(ConnectionString))
                {
                    string queryString = "UPDATE Instructor SET InstructorName = :instructorName, " +
                                         "InstructorCountry = :instructorCountry, ContactNo = :contactNo, " +
                                         "Email = :email, CourseId = :courseId WHERE InstructorId = :instructorId";

                    using (OracleCommand cmd = new OracleCommand(queryString, con))
                    {
                        con.Open();

                        cmd.Parameters.Add(":instructorName", OracleDbType.Varchar2, 255).Value = instructor.InstructorName;
                        cmd.Parameters.Add(":instructorCountry", OracleDbType.Varchar2, 50).Value = instructor.InstructorCountry;
                        cmd.Parameters.Add(":contactNo", OracleDbType.Varchar2, 20).Value = instructor.ContactNo;
                        cmd.Parameters.Add(":email", OracleDbType.Varchar2, 255).Value = instructor.Email;
                        cmd.Parameters.Add(":courseId", OracleDbType.Varchar2, 50).Value = instructor.CourseId;
                        cmd.Parameters.Add(":instructorId", OracleDbType.Int32).Value = instructor.InstructorId;

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public void DeleteInstructorById(int instructorId)
        {
            try
            {
                using (OracleConnection con = new OracleConnection(ConnectionString))
                {
                    con.Open();

                    // Implement the SQL query to delete the instructor record
                    string deleteQuery = "DELETE FROM Instructor WHERE InstructorId = :instructorId";

                    using (OracleCommand cmd = new OracleCommand(deleteQuery, con))
                    {
                        cmd.Parameters.Add(":instructorId", OracleDbType.Int32).Value = instructorId;

                        cmd.ExecuteNonQuery();
                    }

                    con.Close();
                }
            }
            catch (Exception ex)
            {
                // Handle the exception as needed (logging, rethrow, etc.)
                Console.WriteLine(ex.Message);
                throw;
            }
        }


        public void AddInstructor(InstructorModel instructor)
        {
            try
            {
                using (OracleConnection con = new OracleConnection(ConnectionString))
                {
                    int lastId = GetLastInstructorId(con);
                    int newId = lastId + 1;

                    instructor.InstructorId = newId;


                    string queryString = "INSERT INTO Instructor (InstructorId, InstructorName, InstructorCountry, ContactNo, Email, CourseId) " +
                                         "VALUES (:instructorId, :instructorName, :instructorCountry, :contactNo, :email, :courseId)";

                    using (OracleCommand cmd = new OracleCommand(queryString, con))
                    {
                        con.Open();

                        cmd.Parameters.Add(":instructorId", OracleDbType.Int32).Value = instructor.InstructorId;
                        cmd.Parameters.Add(":instructorName", OracleDbType.Varchar2, 255).Value = instructor.InstructorName;
                        cmd.Parameters.Add(":instructorCountry", OracleDbType.Varchar2, 50).Value = instructor.InstructorCountry;
                        cmd.Parameters.Add(":contactNo", OracleDbType.Varchar2, 20).Value = instructor.ContactNo;
                        cmd.Parameters.Add(":email", OracleDbType.Varchar2, 255).Value = instructor.Email;
                        cmd.Parameters.Add(":courseId", OracleDbType.Varchar2, 50).Value = instructor.CourseId;

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }



    }
}
