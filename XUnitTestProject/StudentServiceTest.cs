using Interfaces;
using Model;
using Moq;
using Services;
using System;
using System.Collections.Generic;
using Xunit;

namespace XUnitTestProject
{
    public class StudentServiceTest
    {
        private Mock<IStudentRepository> repoMock;

        public StudentServiceTest()
        {
            repoMock = new Mock<IStudentRepository>();
        }

        [Fact]
        public void CreateStudentService()
        {
            // arrange
            IStudentRepository repo = repoMock.Object;

            // act
            StudentService service = new StudentService(repo);

            // assert
            Assert.NotNull(service);
        }

        [Fact]
        public void CreateStudentServiceWithMissingRepositoryExpectArgumentException()
        {
            // arrange

            StudentService service = null;
            // act + assert
            var ex = Assert.Throws<ArgumentException>(() =>
            {
                StudentService service = new StudentService(null);
            });

            Assert.Equal("Missing Student Repository", ex.Message);
            Assert.Null(service);
        }

        [Theory]
        [InlineData(1, "Name", "Address", 1111, "PostalDistrict", "e@mail")]
        [InlineData(1, "Name", "Address", 1111, "PostalDistrict", null)]
        public void AddValidStudent(int id, string name, string address, int zip, string postal, string email)
        {
            // arrange
            Student s = new Student()
            {
                Id = id,
                Name = name,
                Address = address,
                ZipCode = zip,
                PostalDistrict = postal,
                Email = email
            };

            IStudentRepository repo = repoMock.Object;

            StudentService service = new StudentService(repo);

            // act
            service.AddStudent(s);

            // assert
            //Assert.True(fakeDB.Count == 1);
            //Assert.True(fakeDB.ContainsKey(s.Id));
            //Assert.True(fakeDB[s.Id] == s);
            repoMock.Verify(repo => repo.Add(It.Is<Student>(student => student == s)), Times.Once);
        }

        [Fact]
        public void AddStudentIsNullExpectArgumentException()
        {
            // arrange
            IStudentRepository repo = repoMock.Object;
            StudentService service = new StudentService(repo);

            // act + assert
            var ex = Assert.Throws<ArgumentException>(() =>
            {
                service.AddStudent(null);
            });

            Assert.Equal("Missing student", ex.Message);
            repoMock.Verify(repo => repo.Add(It.Is<Student>(s => s == null)), Times.Never);
        }

        [Theory]
        [InlineData(-1, "Name", "Address", 0, "PostalDistrict", "e@mail")]
        [InlineData(0, "Name", "Address", 0, "PostalDistrict", "e@mail")]

        [InlineData(1, null, "Address", 1111, "PostalDistrict", "e@mail")]
        [InlineData(1, "", "Address", 1111, "PostalDistrict", "e@mail")]

        [InlineData(1, "Name", null, 1111, "PostalDistrict", "e@mail")]
        [InlineData(1, "Name", "", 1111, "PostalDistrict", "e@mail")]

        [InlineData(1, "Name", "Address", 0, "PostalDistrict", "e@mail")]
        [InlineData(1, "Name", "Address", -1, "PostalDistrict", "e@mail")]

        [InlineData(1, "Name", "Address", 1111, null, "e@mail")]
        [InlineData(1, "Name", "Address", 1111, "", "e@mail")]

        [InlineData(1, "Name", "Address", 1111, "PostalDistrict", "")]
        public void AddInvalidStudentExpectArgumentException(int id, string name, string address, int zip, string postal, string email)
        {
            // arrange
            Student s = new Student()
            {
                Id = id,
                Name = name,
                Address = address,
                ZipCode = zip,
                PostalDistrict = postal,
                Email = email
            };

            IStudentRepository repo = repoMock.Object;

            StudentService service = new StudentService(repo);

            // act + assert
            var ex = Assert.Throws<ArgumentException>(() =>
            {
                service.AddStudent(s);
            });

            Assert.Equal("Invalid student", ex.Message);
            repoMock.Verify(repo => repo.Add(It.Is<Student>(student => student == s)), Times.Never);
        }
    }
}
