using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Mvc;
using RegistrarApp.Models;

namespace RegistrarApp.Controllers
{
  public class HomeController : Controller
  {
    //Home and routes
    [HttpGet("/")]
    public ActionResult Index()
    {
      return View();
    }

    [HttpGet("/student/add")]
    public ActionResult AddStudent()
    {
      return View();
    }

    [HttpPost("/student/view-all")]
    public ActionResult AddStudentViewStudent()
    {
      Student newStudent = new Student(Request.Form["student-name"], Request.Form["enrol-date"]);
      newStudent.Save();

      List<Student> allStudents = Student.GetAll();

      return View("StudentList", allStudents);

    }

    [HttpGet("/student/view-all")]
    public ActionResult ViewStudent()
    {
      List<Student> allStudents = Student.GetAll();

      return View("StudentList", allStudents);
    }

    [HttpPost("/student/{studentId}/course/update")]
    public ActionResult ViewStudent(int studentId)
    {
      Student thisStudent = Student.Find(studentId);
      Course thisCourse = Course.Find(Int32.Parse(Request.Form["select-courses"]));
      // var myBoxes = Request.Form["select-courses"];
      // Console.WriteLine(myBoxes);
      thisStudent.AddCourse(thisCourse);
      List<Student> allStudents = Student.GetAll();

      return View("StudentList", allStudents);
    }

    [HttpGet("/course/add")]
    public ActionResult AddCourse()
    {
      return View();
    }

    [HttpPost("/course/view-all")]
    public ActionResult AddCourseViewCourse()
    {
      Course newCourse = new Course(Request.Form["course-name"], Request.Form["course-number"]);
      newCourse.Save();

      List<Course> allCourses = Course.GetAll();

      return View("CourseList", allCourses);
    }

    [HttpGet("/course/view-all")]
    public ActionResult ViewCourse()
    {
      List<Course> allCourses = Course.GetAll();

      return View("CourseList", allCourses);
    }

    [HttpGet("/student/{studentId}/courses/add")]
    public ActionResult AddCoursesToStudent(int studentId)
    {
      Student thisStudent = Student.Find(studentId);
      List<Course> allCourses = Course.GetAll();
      Dictionary<string, object> model = new Dictionary<string, object>();

      model.Add("student", thisStudent);
      model.Add("courses", allCourses);

      return View("StudentProfile", model);
    }

  }
}
