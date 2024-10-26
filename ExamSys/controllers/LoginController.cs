using ExamSys.Models;
using ExamSys.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExamSys.controllers
{
    internal class LoginController
    {
        public static List<Admin> admins = ExcelService.GetAdmins();
        public static List<Student> students = ExcelService.GetStudents();
        public static bool AdminLogin(ref int userId, ref int userType)
        {
            bool getDatas = true;
            while (getDatas)
            {
                Console.WriteLine("Ana səhifəyə qayıtmaq üçün 0 seçin");
                Console.WriteLine("Logininizi daxil edin");
                string login = Console.ReadLine();
                if (login == "0")
                {
                    getDatas = false;
                    break;
                }
                Console.WriteLine("Şifrənizi daxil edin");
                string password = Console.ReadLine();
                if (password == "0")
                {
                    getDatas = false;
                    break;
                }

                var matchingAdmins = admins.Where(x => x.Login == login && x.Password == PasswordService.HashPassword(password)).ToList();

                if (matchingAdmins.Count > 0)
                {
                    userId = matchingAdmins[0].Id;
                    userType = matchingAdmins[0].type;
                    return true;
                }
                else
                {
                    Console.WriteLine("Login və ya şifrə yanlışdır");
                }
            }
            return false;
        }


        public static bool StudentLogin(ref int userId, ref int userType)
        {
            bool getDatas = true;
            while (getDatas)
            {
                Console.WriteLine("Ana səhifəyə qayıtmaq üçün 0 seçin");
                Console.WriteLine("Tələbə loginini daxil edin:");
                string login = Console.ReadLine();
                if (login == "0")
                {
                    getDatas = false;
                    break;
                }
                Console.WriteLine("Şifrənizi daxil edin:");
                string password = Console.ReadLine();
                if (password == "0")
                {
                    getDatas = false;
                    break;
                }

                var matchingStudents = students.Where(x => x.Login == login && x.Password == PasswordService.HashPassword(password)).ToList();

                if (matchingStudents.Count > 0)
                {
                    userId = matchingStudents[0].Id;
                    userType = matchingStudents[0].type;
                    return true;
                }
                else
                {
                    Console.WriteLine("Login və ya şifrə yanlışdır");
                }
            }
            return false;
        }
    }
}
