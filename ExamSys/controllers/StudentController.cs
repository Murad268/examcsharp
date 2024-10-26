using ExamSys.Models;
using ExamSys.service;
using ExamSys.Services;
using System;
using System.Linq;

namespace ExamSys.Controllers
{
    internal class StudentController
    {
        private static ValidationService validationService = new ValidationService();

        private static int GetNextId()
        {
            var users = ExcelService.LoadUsersFromExcel();
            if (users.Count > 0)
            {
                return users.Max(u => u.Id) + 1;
            }
            return 1;
        }
        public static void AddStudent()
        {
            int studentIdCounter = GetNextId();

            string name = GetValidatedField("Adı daxil edin:", "Ad boş ola bilməz.");
            string surname = GetValidatedField("Soyadı daxil edin:", "Soyad boş ola bilməz.");
            string login = GetValidatedField("Login-i daxil edin:", "Login boş ola bilməz.");

            string rawPassword = new Random().Next(1000, 9999).ToString();
            Console.WriteLine($"Tələbənin şifrəsi: {rawPassword}");

            string hashedPassword = PasswordService.HashPassword(rawPassword);

            Student student = new Student(studentIdCounter, name, surname, login, hashedPassword);
            StudentService.students.Add(student);

            Console.WriteLine("Tələbə uğurla əlavə olundu.");

            ExcelService.SaveUsersToExcel(StudentService.students.Cast<User>().ToList()); 
        }

        public static void UpdateStudent()
        {
            StudentService.LoadStudentsFromExcel();
            StudentService.DisplayStudents();
            Console.WriteLine("Yeniləmək istədiyiniz tələbənin ID-sini daxil edin:");
            if (int.TryParse(Console.ReadLine(), out int studentId))
            {
                var student = StudentService.GetStudentById(studentId);
                if (student == null)
                {
                    Console.WriteLine("Tələbə tapılmadı.");
                    return;
                }

                Console.WriteLine($"Ad (mövcud: {student.Name}):");
                string newName = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(newName)) student.Name = newName;

                Console.WriteLine($"Soyad (mövcud: {student.Surname}):");
                string newSurname = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(newSurname)) student.Surname = newSurname;

                Console.WriteLine($"Login (mövcud: {student.Login}):");
                string newLogin = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(newLogin)) student.Login = newLogin;

                Console.WriteLine("Tələbə məlumatları yeniləndi.");

                ExcelService.SaveUsersToExcel(StudentService.students.Cast<User>().ToList()); 
            }
            else
            {
                Console.WriteLine("Yanlış ID daxil edilib.");
            }
        }


        public static void DeleteStudent()
        {
            StudentService.LoadStudentsFromExcel();
            StudentService.DisplayStudents();
            Console.WriteLine("Silmək istədiyiniz tələbənin ID-sini daxil edin:");
            if (int.TryParse(Console.ReadLine(), out int studentId))
            {
                var student = StudentService.GetStudentById(studentId);
                if (student != null)
                {
                    StudentService.students.Remove(student);
                    ExcelService.RemoveUserFromExcel(studentId);
                    Console.WriteLine("Tələbə silindi.");
                }
                else
                {
                    Console.WriteLine("Tələbə tapılmadı.");
                }
            }
            else
            {
                Console.WriteLine("Yanlış ID daxil edilib.");
            }
        }

        private static string GetValidatedField(string promptMessage, string errorMessage)
        {
            string fieldValue;
            do
            {
                Console.WriteLine(promptMessage);
                fieldValue = Console.ReadLine();

                if (!validationService.ValidateField(fieldValue))
                {
                    Console.WriteLine(errorMessage);
                }

            } while (!validationService.ValidateField(fieldValue));

            return fieldValue;
        }
    }
}
