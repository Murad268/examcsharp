using ExamSys.Controllers;
using System;

namespace ExamSys.Services
{
    public class StudentOperations
    {
        public static void StudentOperationsMenu()
        {
            bool exit = false;

            while (!exit)
            {
                Console.WriteLine("Tələbə əlavə etmək üçün 1, tələbə yeniləmək üçün 2, tələbə silmək üçün 3 seçin, çıxış üçün 4.");
                var selectedOption = Console.ReadLine();

                switch (selectedOption)
                {
                    case "1":
                        StudentController.AddStudent();
                        break;
                    case "2":
                        StudentController.UpdateStudent();
                        break;
                    case "3":
                        StudentController.DeleteStudent();
                        break;
                    case "4":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Yanlış seçim.");
                        break;
                }
            }
        }
    }
}
