using ExamSys.Models;
using System;

namespace ExamSys.Models
{
    public class Student : User
    {
        public int type = 2;

        public Student(int id, string ad, string soyad, string login, string password)
          : base(id, ad, soyad, login, password)
        {
        }
    }
}
