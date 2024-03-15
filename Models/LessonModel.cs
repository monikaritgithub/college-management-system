using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;

namespace OracleSampleProject.Models
{
    public class LessonModel
    {
        public string CourseId { get; set; }
        public int LessonNo { get; set; }
        public string LessonTitle { get; set; }

               // Add ConnectionString property
        public string ConnectionString { get; }


        public List<LessonModel> LessonList = new List<LessonModel>();

        public LessonModel()
        {
            // Default connection string
            ConnectionString = "User Id=icp;Password=123;Data Source=localhost:1521/orcl;";
        }

        public LessonModel(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public void GetLessons()
        {
            try
            {
                using (OracleConnection con = new OracleConnection(ConnectionString))
                {
                    string queryString = "SELECT CourseId, LessonNo, LessonTitle FROM Lesson";
                    OracleCommand cmd = new OracleCommand(queryString, con);
                    cmd.BindByName = true;
                    cmd.CommandType = CommandType.Text;

                    con.Open();
                    OracleDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        LessonModel lesson = new LessonModel
                        {
                            CourseId = reader.GetString(0),
                            LessonNo = reader.GetInt32(1),
                            LessonTitle = reader.GetString(2)
                        };

                        LessonList.Add(lesson);
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

        private int GetLastLessonNo(OracleConnection con, string courseId)
        {
            string queryString = "SELECT MAX(LessonNo) FROM Lesson WHERE CourseId = :courseId";

            using (OracleCommand cmd = new OracleCommand(queryString, con))
            {
                con.Open();
                cmd.Parameters.Add(":courseId", OracleDbType.Varchar2, 50).Value = courseId;

                int lastLessonNo = Convert.ToInt32(cmd.ExecuteScalar());
                con.Close();

                return lastLessonNo;
            }
        }

        public void AddLesson(LessonModel lesson)
        {
            try
            {
                using (OracleConnection con = new OracleConnection(ConnectionString))
                {
                    //int lastLessonNo = GetLastLessonNo(con, lesson.CourseId);
                   // int newLessonNo = lastLessonNo + 1;

                    //lesson.LessonNo = newLessonNo;

                    string queryString = "INSERT INTO Lesson (CourseId, LessonNo, LessonTitle) " +
                                         "VALUES (:courseId, :lessonNo, :lessonTitle)";

                    using (OracleCommand cmd = new OracleCommand(queryString, con))
                    {
                        con.Open();

                        cmd.Parameters.Add(":courseId", OracleDbType.Varchar2, 50).Value = lesson.CourseId;
                        cmd.Parameters.Add(":lessonNo", OracleDbType.Int32).Value = lesson.LessonNo;
                        cmd.Parameters.Add(":lessonTitle", OracleDbType.Varchar2, 255).Value = lesson.LessonTitle;

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public LessonModel GetLessonByCourseIdAndLessonNo(string courseId, int lessonNo)
        {
            try
            {
                using (OracleConnection con = new OracleConnection(ConnectionString))
                {
                    string queryString = "SELECT CourseId, LessonNo, LessonTitle FROM Lesson WHERE CourseId = :courseId AND LessonNo = :lessonNo";

                    using (OracleCommand cmd = new OracleCommand(queryString, con))
                    {
                        cmd.BindByName = true;
                        cmd.Parameters.Add(":courseId", OracleDbType.Varchar2, 50).Value = courseId;
                        cmd.Parameters.Add(":lessonNo", OracleDbType.Int32).Value = lessonNo;

                        con.Open();
                        OracleDataReader reader = cmd.ExecuteReader();

                        if (reader.Read())
                        {
                            LessonModel lesson = new LessonModel
                            {
                                CourseId = reader.GetString(0),
                                LessonNo = reader.GetInt32(1),
                                LessonTitle = reader.GetString(2)
                            };

                            reader.Dispose();
                            con.Close();

                            return lesson;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            // Return null if no lesson is found or an error occurs
            return null;
        }


        public void EditLesson(LessonModel lesson)
        {
            try
            {
                using (OracleConnection con = new OracleConnection(ConnectionString))
                {
                    // Implement logic to update the lesson details in the database
                    string queryString = "UPDATE Lesson SET LessonTitle = :lessonTitle " +
                                         "WHERE CourseId = :courseId AND LessonNo = :lessonNo";

                    using (OracleCommand cmd = new OracleCommand(queryString, con))
                    {
                        con.Open();

                        cmd.Parameters.Add(":lessonTitle", OracleDbType.Varchar2, 255).Value = lesson.LessonTitle;
                        cmd.Parameters.Add(":courseId", OracleDbType.Varchar2, 50).Value = lesson.CourseId;
                        cmd.Parameters.Add(":lessonNo", OracleDbType.Int32).Value = lesson.LessonNo;

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle the exception (log, throw, etc.)
                Console.WriteLine($"Error in EditLesson: {ex.Message}");
            }
        }

        public void DeleteLesson(string courseId, int lessonNo)
        {
            try
            {
                using (OracleConnection con = new OracleConnection(ConnectionString))
                {
                    // Implement logic to delete the lesson with the specified CourseId and LessonNo
                    string queryString = "DELETE FROM Lesson WHERE CourseId = :courseId AND LessonNo = :lessonNo";

                    using (OracleCommand cmd = new OracleCommand(queryString, con))
                    {
                        con.Open();

                        cmd.Parameters.Add(":courseId", OracleDbType.Varchar2, 50).Value = courseId;
                        cmd.Parameters.Add(":lessonNo", OracleDbType.Int32).Value = lessonNo;

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle the exception (log, throw, etc.)
                Console.WriteLine($"Error in DeleteLesson: {ex.Message}");
            }
        }

    }
}
