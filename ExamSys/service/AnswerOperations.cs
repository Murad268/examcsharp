using ExamSys.Controllers;
using System;

namespace ExamSys.Services
{
    internal class AnswerOperations
    {
        public static void AnswerOperationsMenu()
        {
            bool exit = false;

            while (!exit)
            {
                Console.WriteLine("Cavab əlavə etmək üçün 1, cavabları siyahı şəklində görmək üçün 2, cavabı yeniləmək üçün 3, cavabı silmək üçün 4, çıxış üçün 5 seçin:");
                var option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        AnswerController.AddAnswer();
                        break;
                    case "2":
                        AnswerController.DisplayAnswers();
                        break;
                    case "3":
                        AnswerController.UpdateAnswer();
                        break;
                    case "4":
                        AnswerController.DeleteAnswer();
                        break;
                    case "5":
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
