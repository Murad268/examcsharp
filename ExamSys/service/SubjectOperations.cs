using System;
using ExamSys.Controllers;

namespace ExamSys.Services
{
    public class SubjectOperations
    {
        public static void SubjectOperationsMenu()
        {
            bool exit = false;

            while (!exit)
            {
                Console.WriteLine("Fənn əlavə etmək üçün 1, fənni yeniləmək üçün 2, fənni silmək üçün 3 seçin, əsas menyuya qayıtmaq üçün 4 seçin");
                var subjectChoice = Console.ReadLine();

                switch (subjectChoice)
                {
                    case "1":
                        SubjectController.AddSubject();
                        break;
                    case "2":
                        SubjectController.UpdateSubject();
                        break;
                    case "3":
                        SubjectController.DeleteSubject();
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
