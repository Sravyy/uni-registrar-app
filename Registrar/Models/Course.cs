using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

namespace RegistrarApp.Models
{
  public class Course
  {
    private string _courseName;
    private string _courseNumber;
    private int _id;

    public Course(string courseName, string courseNumber, int id = 0)
    {
      _courseName = courseName;
      _courseNumber = courseNumber;
      _id = id;
    }

    public override bool Equals(System.Object otherCourse)
    {
      if (!(otherCourse is Course))
      {
        return false;
      }
      else
      {
        Course newCourse = (Course) otherCourse;
        return this.GetId().Equals(newCourse.GetId());
      }
    }

    public override int GetHashCode()
    {
      return this.GetId().GetHashCode();
    }

    public string GetCourseName()
    {
      return _courseName;
    }

    public string GetCourseNumber()
    {
      return _courseNumber;
    }

    public int GetId()
    {
      return _id;
    }

    public static List<Course> GetAll()
    {
      List<Course> allCourses = new List<Course> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM courses;";
      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        int CourseId = rdr.GetInt32(0);
        string CourseName = rdr.GetString(1);
        string CourseNumber = rdr.GetString(2);
        Course newCourse = new Course(CourseName, CourseNumber, CourseId);
        allCourses.Add(newCourse);
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return allCourses;
    }

    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO courses (name, course_number) VALUES (@courseName, @courseNumber);";

      MySqlParameter courseName = new MySqlParameter();
      courseName.ParameterName = "@courseName";
      courseName.Value = this._courseName;
      cmd.Parameters.Add(courseName);

      MySqlParameter courseNumber = new MySqlParameter();
      courseNumber.ParameterName = "@courseNumber";
      courseNumber.Value = this._courseNumber;
      cmd.Parameters.Add(courseNumber);

      cmd.ExecuteNonQuery();
      _id = (int) cmd.LastInsertedId;
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public static Course Find(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM courses WHERE id = (@searchId);";

      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@searchId";
      searchId.Value = id;
      cmd.Parameters.Add(searchId);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      int CourseId = 0;
      string CourseName = "";
      string CourseNumber = "";

      while(rdr.Read())
      {
        CourseId = rdr.GetInt32(0);
        CourseName = rdr.GetString(1);
        CourseNumber = rdr.GetString(2);
      }
      Course newCourse = new Course(CourseName, CourseNumber, CourseId);
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return newCourse;
    }

    public void AddStudent(Student newStudent)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO courses_students (course_id, student_id) VALUES (@CourseId, @StudentId);";

      MySqlParameter course_id = new MySqlParameter();
      course_id.ParameterName = "@CourseId";
      course_id.Value = _id;
      cmd.Parameters.Add(course_id);

      MySqlParameter student_id = new MySqlParameter();
      student_id.ParameterName = "@StudentId";
      student_id.Value = newStudent.GetId();
      cmd.Parameters.Add(student_id);

      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public void Delete()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      MySqlCommand cmd = new MySqlCommand("DELETE FROM courses WHERE id = @CoursesId; DELETE FROM courses_students WHERE course_id = @CoursesId;", conn);
      MySqlParameter courseIdParameter = new MySqlParameter();
      courseIdParameter.ParameterName = "@CoursesId";
      courseIdParameter.Value = this.GetId();

      cmd.Parameters.Add(courseIdParameter);
      cmd.ExecuteNonQuery();

      if (conn != null)
      {
        conn.Close();
      }
    }

    public static void DeleteAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM courses;";
      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }
  }
}
