using ExamSys.Models;
using System;

namespace ExamSys.Models
{
    public class Admin : User
    {
        public int type = 1;

        public Admin(int id, string ad, string soyad, string login, string password)
          : base(id, ad, soyad, login, password)
        {
        }
    }
}
