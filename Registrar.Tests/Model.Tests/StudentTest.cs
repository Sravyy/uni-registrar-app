using Microsoft.VisualStudio.TestTools.UnitTesting;
using RegistrarApp.Models;
using System.Collections.Generic;
using System;

namespace RegistrarApp.Tests
{
  [TestClass]
  // public class StudentTests : IDisposable
  public class StudentTests : IDisposable
  {
    public StudentTests()
    {
      DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=registrar_test;";
    }

    public void Dispose()
    {
      Student.DeleteAll();
      Course.DeleteAll();
    }

    [TestMethod]
    public void GetAll_DatabaseEmptyAtFirst_0()
    {
      //Arrange, Act
      int result = Student.GetAll().Count;

      //Assert
      Assert.AreEqual(0, result);
    }

    [TestMethod]
    public void Equals_OverrideTrueIfDescriptionsAreTheSame_Student()
    {
      // Arrange, Act
      Student firstStudent = new Student("Adam Sandler", "2017-01-01");
      Student secondStudent = new Student("Adam Sandler", "2017-01-01");

      // Assert
      Assert.AreEqual(firstStudent, secondStudent);
    }

    [TestMethod]
    public void Save_SavesToDatabase_StudentList()
    {
      //Arrange
      Student testStudent = new Student("Adam Sandler", "2017-01-01");

      //Act
      testStudent.Save();
      List<Student> result = Student.GetAll();
      List<Student> testList = new List<Student>{testStudent};

      //Assert
      CollectionAssert.AreEqual(testList, result);
    }

    [TestMethod]
    public void Save_AssignsIdToObject_Id()
    {
      //Arrange
      Student testStudent = new Student("Adam Sandler", "2017-01-01");

      //Act
      testStudent.Save();
      Student savedStudent = Student.GetAll()[0];

      int result = savedStudent.GetId();
      int testId = testStudent.GetId();

      //Assert
      Assert.AreEqual(testId, result);
    }

    [TestMethod]
    public void AddCourse_AddsCourseToStudent_CourseList()
    {
      //Arrange
      Student testStudent = new Student("Adam Sandler", "2017-01-01");
      testStudent.Save();

      Course testCourse = new Course("Intro to History", "HIST100");
      testCourse.Save();

      //Act
      testStudent.AddCourse(testCourse);

      List<Course> result = testStudent.GetCourses();
      List<Course> testList = new List<Course>{testCourse};

      //Assert
      CollectionAssert.AreEqual(testList, result);
    }

    [TestMethod]
    public void Find_FindsStudentInDatabase_Student()
    {
      //Arrange
      Student testStudent = new Student("Adam Sandler", "2017-01-01");
      testStudent.Save();

      //Act
      Student foundStudent = Student.Find(testStudent.GetId());

      //Assert
      Assert.AreEqual(testStudent, foundStudent);
    }

    [TestMethod]
    public void GetCourses_ReturnsAllStudentCourses_CourseList()
    {
      //Arrange
      Student testStudent = new Student("Adam Sandler", "2017-01-01");
      testStudent.Save();

      Course testCourse1 = new Course("Intro to History", "HIST100");
      testCourse1.Save();

      Course testCourse2 = new Course("Intro to History", "HIST100");
      testCourse2.Save();

      //Act
      testStudent.AddCourse(testCourse1);
      List<Course> result = testStudent.GetCourses();
      List<Course> testList = new List<Course> {testCourse1};

      //Assert
      CollectionAssert.AreEqual(testList, result);
    }



  }
}
