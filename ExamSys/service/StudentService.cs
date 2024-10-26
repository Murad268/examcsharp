using ExamSys.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExamSys.Services
{
    public class StudentService
    {
        public static List<Student> students = new List<Student>();

        public static void DisplayStudents()
        {
            if (students.Count == 0)
            {
                Console.WriteLine("Hal-hazırda siyahıda tələbə yoxdur.");
                return;
            }

            foreach (var student in students)
            {
                Console.WriteLine($"ID: {student.Id}, Ad: {student.Name}, Soyad: {student.Surname}, Login: {student.Login}");
            }
        }

        public static Student GetStudentById(int id)
        {
            return students.FirstOrDefault(s => s.Id == id);
        }

        public static void LoadStudentsFromExcel()
        {
            students = ExcelService.GetStudents();
        }
        public static List<Student> LoadStudentsFromExcelX()
        {
            return ExcelService.GetStudents();
        }
    }
}
