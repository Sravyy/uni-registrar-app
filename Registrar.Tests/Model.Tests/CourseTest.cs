using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;
using RegistrarApp.Models;

namespace RegistrarApp.Tests
{
  [TestClass]
  public class CourseTests :IDisposable
  {
    public CourseTests()
    {
      DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889 ;database=registrar_test;";
    }

    public void Dispose()
    {
      Student.DeleteAll();
      Course.DeleteAll();
    }

    [TestMethod]
    public void GetAll_CoursesEmptyAtFirst_0()
    {
      //Arrange, Act
      int result = Course.GetAll().Count;

      //Assert
      Assert.AreEqual(0, result);
    }

    [TestMethod]
    public void Save_SavesCoursesToDatabase_CourseList()
    {
      //Arrange
      Course testCourse = new Course("Intro to History", "HIST100");
      testCourse.Save();

      //Act
      List<Course> result = Course.GetAll();
      List<Course> testList = new List<Course>{testCourse};

      //Assert
      CollectionAssert.AreEqual(testList, result);
    }

    [TestMethod]
     public void Save_DatabaseAssignsIdToCourse_Id()
     {
       //Arrange
       Course testCourse = new Course("Intro to History", "HIST100");
       testCourse.Save();

       //Act
       Course savedCourse = Course.GetAll()[0];

       int result = savedCourse.GetId();
       int testId = testCourse.GetId();

       //Assert
       Assert.AreEqual(testId, result);
    }

    [TestMethod]
    public void Find_FindsCourseInDatabase_Course()
    {
      //Arrange
      Course testCourse = new Course("Intro to History", "HIST100");
      testCourse.Save();

      //Act
      Course foundCourse = Course.Find(testCourse.GetId());

      //Assert
      Assert.AreEqual(testCourse, foundCourse);
    }


    [TestMethod]
    public void Delete_DeletesCourseAssociationsFromDatabase_CourseList()
    {
      //Arrange
      Student testStudent = new Student("Adam Sandler", "2017-01-01");
      testStudent.Save();

      Course testCourse = new Course("Intro to History", "HIST100");
      testCourse.Save();

      //Act
      testCourse.AddStudent(testStudent);
      testCourse.Delete();

      List<Course> resultStudentCourses = testStudent.GetCourses();
      List<Course> testStudentCourses = new List<Course> {};

      //Assert
      CollectionAssert.AreEqual(testStudentCourses, resultStudentCourses);
    }

    // [TestMethod]
    // public void Test_AddStudent_AddsStudentToCourse()
    // {
    //   //Arrange
    //   Course testCourse = new Course("Intro to History", "HIST100");
    //   testCourse.Save();
    //
    //   Student testStudent = new Student("Adam Sandler", "2017-01-01");
    //   testStudent.Save();
    //
    //   Student testStudent2 = new Student("Water the garden", "2017-01-01");
    //   testStudent2.Save();
    //
    //   //Act
    //   testCourse.AddStudent(testStudent);
    //   testCourse.AddStudent(testStudent2);
    //
    //   List<Student> result = testCourse.GetStudents();
    //   List<Student> testList = new List<Student>{testStudent, testStudent2};
    //
    //   //Assert
    //   CollectionAssert.AreEqual(testList, result);
    // }
  }
}
