using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

namespace RegistrarApp.Models
{
  public class Student
  {
    private int _id;
    private string _studentName;
    private string _enrolmentDate;

    public Student(string studentName, string enrolmentDate, int Id = 0)
    {
      _id = Id;
      _studentName = studentName;
      _enrolmentDate = enrolmentDate;
    }

    public override bool Equals(System.Object otherStudent)
    {
      if (!(otherStudent is Student))
      {
        return false;
      }
      else
      {
        Student newStudent = (Student) otherStudent;
        bool idEquality = (this.GetId() == newStudent.GetId());
        bool studentNameEquality = (this.GetStudentName() == newStudent.GetStudentName());
        bool enrolmentDateEquality = this.GetEnrolmentDate() == newStudent.GetEnrolmentDate();
        return (idEquality && studentNameEquality && enrolmentDateEquality);
      }
    }

    public override int GetHashCode()
    {
      return this.GetStudentName().GetHashCode();
    }

    public int GetId()
    {
      return _id;
    }

    public string GetStudentName()
    {
      return _studentName;
    }

    public string GetEnrolmentDate()
    {
      return _enrolmentDate;
    }

    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO students (name, date_add) VALUES (@name, @dateAdd);";

      MySqlParameter name = new MySqlParameter();
      name.ParameterName = "@name";
      name.Value = this._studentName;
      cmd.Parameters.Add(name);

      MySqlParameter dateAdd = new MySqlParameter();
      dateAdd.ParameterName = "@dateAdd";
      dateAdd.Value = this._enrolmentDate;
      cmd.Parameters.Add(dateAdd);

      cmd.ExecuteNonQuery();
      _id = (int) cmd.LastInsertedId;
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public static Student Find(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM `students` WHERE id = @thisId;";

      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@thisId";
      searchId.Value = id;
      cmd.Parameters.Add(searchId);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;

      int studentId = 0;
      string studentName = "";
      string studentEnrolDate = "";

      while (rdr.Read())
      {
        studentId = rdr.GetInt32(0);
        studentName = rdr.GetString(1);
        studentEnrolDate = rdr.GetString(2);
      }

      Student newStudent= new Student(studentName, studentEnrolDate, studentId);
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return newStudent;
    }

    public List<Course> GetCourses()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT course_id FROM courses_students WHERE student_id = @studentId;";

      MySqlParameter studentIdParameter = new MySqlParameter();
      studentIdParameter.ParameterName = "@studentId";
      studentIdParameter.Value = _id;
      cmd.Parameters.Add(studentIdParameter);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;

      List<int> courseIds = new List<int> {};
      while(rdr.Read())
      {
        int courseId = rdr.GetInt32(0);
        courseIds.Add(courseId);
      }
      rdr.Dispose();

      List<Course> courses = new List<Course> {};
      foreach (int courseId in courseIds)
      {
        var courseQuery = conn.CreateCommand() as MySqlCommand;
        courseQuery.CommandText = @"SELECT * FROM courses WHERE id = @CourseId;";

        MySqlParameter courseIdParameter = new MySqlParameter();
        courseIdParameter.ParameterName = "@CourseId";
        courseIdParameter.Value = courseId;
        courseQuery.Parameters.Add(courseIdParameter);

        var courseQueryRdr = courseQuery.ExecuteReader() as MySqlDataReader;
        while(courseQueryRdr.Read())
        {
          int thisCourseId = courseQueryRdr.GetInt32(0);
          string courseName = courseQueryRdr.GetString(1);
          string courseNumber = courseQueryRdr.GetString(2);
          Course foundCourse = new Course(courseName, courseNumber, thisCourseId);
          courses.Add(foundCourse);
        }
        courseQueryRdr.Dispose();
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return courses;
    }

    public List<Course> GetAvailableCourses()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT course_id FROM courses WHERE student_id = @studentId;";

      MySqlParameter studentIdParameter = new MySqlParameter();
      studentIdParameter.ParameterName = "@studentId";
      studentIdParameter.Value = _id;
      cmd.Parameters.Add(studentIdParameter);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;

      List<int> courseIds = new List<int> {};
      while(rdr.Read())
      {
        int courseId = rdr.GetInt32(0);
        courseIds.Add(courseId);
      }
      rdr.Dispose();

      List<Course> courses = new List<Course> {};
      foreach (int courseId in courseIds)
      {
        var courseQuery = conn.CreateCommand() as MySqlCommand;
        courseQuery.CommandText = @"SELECT * FROM courses WHERE id = @CourseId;";

        MySqlParameter courseIdParameter = new MySqlParameter();
        courseIdParameter.ParameterName = "@CourseId";
        courseIdParameter.Value = courseId;
        courseQuery.Parameters.Add(courseIdParameter);

        var courseQueryRdr = courseQuery.ExecuteReader() as MySqlDataReader;
        while(courseQueryRdr.Read())
        {
          int thisCourseId = courseQueryRdr.GetInt32(0);
          string courseName = courseQueryRdr.GetString(1);
          string courseNumber = courseQueryRdr.GetString(2);
          Course foundCourse = new Course(courseName, courseNumber, thisCourseId);
          courses.Add(foundCourse);
        }
        courseQueryRdr.Dispose();
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return courses;
    }



    public static List<Student> GetAll()
    {
      List<Student> allStudents = new List<Student> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM students ORDER BY name ASC;";
      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        int studentId = rdr.GetInt32(0);
        string studentName = rdr.GetString(1);
        string studentEnrolmentDate = rdr.GetString(2);
        Student newStudent = new Student(studentName, studentEnrolmentDate, studentId);
        allStudents.Add(newStudent);
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return allStudents;
    }

    public void AddCourse(Course newCourse)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO courses_students (course_id, student_id) VALUES (@CourseId, @StudentId);";

      MySqlParameter course_id = new MySqlParameter();
      course_id.ParameterName = "@CourseId";
      course_id.Value = newCourse.GetId();
      cmd.Parameters.Add(course_id);

      MySqlParameter student_id = new MySqlParameter();
      student_id.ParameterName = "@StudentId";
      student_id.Value = _id;
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
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM students WHERE id = @StudentId; DELETE FROM Courses_students WHERE student_id = @StudentId;";

      MySqlParameter studentIdParameter = new MySqlParameter();
      studentIdParameter.ParameterName = "@StudentId";
      studentIdParameter.Value = this.GetId();
      cmd.Parameters.Add(studentIdParameter);

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
      cmd.CommandText = @"DELETE FROM students;";
      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

  }
}
