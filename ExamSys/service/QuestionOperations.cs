using ExamSys.Controllers;
using System;

namespace ExamSys.Services
{
    public class QuestionOperations
    {
        public static void QuestionOperationsMenu()
        {
            bool keepRunning = true;

            while (keepRunning)
            {
                Console.WriteLine("Suallar əməliyyatlar menyusuna xoş gəlmisiniz.");
                Console.WriteLine("1. Sual əlavə et");
                Console.WriteLine("2. Sualı yenilə");
                Console.WriteLine("3. Sualı sil");
                Console.WriteLine("4. Sualları siyahıla");
                Console.WriteLine("5. Əsas menyuya qayıt");
                Console.WriteLine("Seçiminizi edin:");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        QuestionController.AddQuestion();
                        break;
                    case "2":
                        QuestionController.UpdateQuestion();
                        break;
                    case "3":
                        QuestionController.DeleteQuestion();
                        break;
                    case "4":
                        QuestionController.DisplayQuestions();
                        break;
                    case "5":
                        keepRunning = false;
                        break;
                    default:
                        Console.WriteLine("Yanlış seçim, yenidən cəhd edin.");
                        break;
                }
            }
        }
    }
}
