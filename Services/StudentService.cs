using Castle.Core.Internal;
using Interfaces;
using Model;
using System;

namespace Services
{
    public class StudentService
    {
        private readonly IStudentRepository repo;

        public StudentService(IStudentRepository repo)
        {
            if (repo == null)
            {
                throw new ArgumentException("Missing Student Repository");
            }
            this.repo = repo;
        }

        public void AddStudent(Student s)
        {
            if (s == null)
            {
                throw new ArgumentException("Missing student");
            }
            if (!IsValidStudent(s))
            {
                throw new ArgumentException("Invalid student");
            }
            repo.Add(s);
        }

        private bool IsValidStudent(Student s)
        {
            return (s.Id > 0
                && !s.Name.IsNullOrEmpty()
                && !s.Address.IsNullOrEmpty()
                && s.ZipCode > 0
                && !s.PostalDistrict.IsNullOrEmpty()
                && s.Email != "");
        }
    }
}
