using ExamSys.Controllers;
using System;

namespace ExamSys.Services
{
    public class ExamOperations
    {
        public static void ExamOperationsMenu()
        {
            bool continueOperations = true;

            while (continueOperations)
            {
                Console.WriteLine("İmtahan əməliyyatları menyusu:");
                Console.WriteLine("1 - İmtahan əlavə et");
                Console.WriteLine("3 - İmtahanları sil");
                Console.WriteLine("4 - İmtahanları göstər");
                Console.WriteLine("5 - Menyudan çıx");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ExamController.AddExam();
                        break;
                    case "3":
                        ExamController.DeleteExam();
                        break;
                    case "4":
                        ExamService.DisplayExams();
                        break;
                    case "5":
                        continueOperations = false;
                        break;
                    default:
                        Console.WriteLine("Yanlış seçim. Yenidən cəhd edin.");
                        break;
                }
            }
        }
    }
}
