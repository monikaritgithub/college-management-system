// CourseModel.cs
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;

namespace OracleSampleProject.Models
{
    public class CourseModel
    {
        public string CourseId { get; set; }
        public string CourseTitle { get; set; }
        public string ConnectionString { get; }

        public List<CourseModel> CourseList = new List<CourseModel>();
        public List<Enrollment> Enrollments { get; set; }
        public CourseModel()
        {
            // Default connection string
            ConnectionString = "User Id=icp;Password=123;Data Source=localhost:1521/orcl;";
        }
        public CourseModel(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public void GetCourses()
        {
            try
            {
                string conString = "User Id=icp;Password=123;Data Source=localhost:1521/orcl;";
                using (OracleConnection con = new OracleConnection(conString))
                {
                    string queryString = "SELECT courseId, courseTitle FROM Course";
                    OracleCommand cmd = new OracleCommand(queryString, con);
                    cmd.BindByName = true;
                    cmd.CommandType = CommandType.Text;

                    con.Open();
                    OracleDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        CourseModel course = new CourseModel
                        {
                            CourseId = reader.GetString(0),
                            CourseTitle = reader.GetString(1)
                        };

                        CourseList.Add(course);
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
        public List<CourseModel> GetCoursesList()
        {
            List<CourseModel> courses = new List<CourseModel>();

            try
            {
                string conString = "User Id=icp;Password=123;Data Source=localhost:1521/orcl;";
                using (OracleConnection con = new OracleConnection(conString))
                {
                    string queryString = "SELECT courseId, courseTitle FROM Course";
                    OracleCommand cmd = new OracleCommand(queryString, con);
                    cmd.BindByName = true;
                    cmd.CommandType = CommandType.Text;

                    con.Open();
                    OracleDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        CourseModel course = new CourseModel
                        {
                            CourseId = reader.GetString(0),
                            CourseTitle = reader.GetString(1)
                        };

                        courses.Add(course);
                    }

                    reader.Dispose();
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return courses;
        }

        private int GetLastCourseId(OracleConnection con)
        {
            string queryString = "SELECT MAX(courseId) FROM Course";

            using (OracleCommand cmd = new OracleCommand(queryString, con))
            {
                con.Open();
                string lastId = cmd.ExecuteScalar() as string;
                con.Close();

                return !string.IsNullOrEmpty(lastId) ? Convert.ToInt32(lastId) : 0;
            }
        }

        public void AddCourse(CourseModel course)
        {
            try
            {
                using (OracleConnection con = new OracleConnection("User Id=icp;Password=123;Data Source=localhost:1521/orcl;"))
                {
                   // int lastId = GetLastCourseId(con);
                    //int newId = lastId + 1;

                    //course.CourseId = newId.ToString();

                    string queryString = "INSERT INTO Course (courseId, courseTitle) " +
                                         "VALUES (:id, :title)";

                    using (OracleCommand cmd = new OracleCommand(queryString, con))
                    {
                        con.Open();

                        cmd.Parameters.Add(":id", OracleDbType.Varchar2, 50).Value = course.CourseId;
                        cmd.Parameters.Add(":title", OracleDbType.Varchar2, 255).Value = course.CourseTitle;

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        public CourseModel GetCourseById(string courseId)
        {
            try
            {
                using (OracleConnection con = new OracleConnection(ConnectionString))
                {
                    string queryString = "SELECT courseId, courseTitle FROM Course WHERE courseId = :id";
                    OracleCommand cmd = new OracleCommand(queryString, con);
                    cmd.BindByName = true;
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add(":id", OracleDbType.Varchar2, 50).Value = courseId;

                    con.Open();
                    OracleDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        CourseModel course = new CourseModel
                        {
                            CourseId = reader.GetString(0),
                            CourseTitle = reader.GetString(1)
                        };

                        reader.Dispose();
                        con.Close();

                        return course;
                    }

                    reader.Dispose();
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return null;
        }

        public void UpdateCourse(CourseModel course)
        {
            try
            {
                using (OracleConnection con = new OracleConnection(ConnectionString))
                {
                    string queryString = "UPDATE Course SET courseTitle = :title WHERE courseId = :id";
                    using (OracleCommand cmd = new OracleCommand(queryString, con))
                    {
                        con.Open();

                        cmd.Parameters.Add(":title", OracleDbType.Varchar2, 255).Value = course.CourseTitle;
                        cmd.Parameters.Add(":id", OracleDbType.Varchar2, 50).Value = course.CourseId;

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void DeleteCourse(string courseId)
        {
            try
            {
                using (OracleConnection con = new OracleConnection(ConnectionString))
                {
                    string queryString = "DELETE FROM Course WHERE courseId = :id";
                    using (OracleCommand cmd = new OracleCommand(queryString, con))
                    {
                        con.Open();
                        cmd.Parameters.Add(":id", OracleDbType.Varchar2, 50).Value = courseId;

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        public void EnrollStudent(int studentId, string courseId)
        {
            try
            {
                using (OracleConnection con = new OracleConnection(ConnectionString))
                {
                    string queryString = "INSERT INTO Enrollment (StudentId, CourseId) VALUES (:studentId, :courseId)";
                    using (OracleCommand cmd = new OracleCommand(queryString, con))
                    {
                        con.Open();

                        cmd.Parameters.Add(":studentId", OracleDbType.Int32).Value = studentId;
                        cmd.Parameters.Add(":courseId", OracleDbType.Varchar2, 50).Value = courseId;

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void WithdrawStudent(int studentId, string courseId)
        {
            try
            {
                using (OracleConnection con = new OracleConnection(ConnectionString))
                {
                    string queryString = "DELETE FROM Enrollment WHERE StudentId = :studentId AND CourseId = :courseId";
                    using (OracleCommand cmd = new OracleCommand(queryString, con))
                    {
                        con.Open();

                        cmd.Parameters.Add(":studentId", OracleDbType.Int32).Value = studentId;
                        cmd.Parameters.Add(":courseId", OracleDbType.Varchar2, 50).Value = courseId;

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
