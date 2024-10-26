using System;

namespace ExamSys.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }

        public User(int id, string namne, string surname, string login, string password)
        {
            Id = id;
            Name = namne;
            Surname = surname;
            Login = login;
            Password = password;
        }
    }
}
