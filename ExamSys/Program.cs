using ExamSys.controllers;
using ExamSys.Controllers;
using ExamSys.service;
using ExamSys.Service;
using ExamSys.Services;
using System;
using System.Text;

namespace ExamSys
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool logged = false;
            int userId = 0;
            int userType = 0;
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;

        Start:
            if (!logged)
            {
                Console.WriteLine("Giriş edilməyib. Zəhmət olmasa daxil olun.");
                Console.WriteLine("Tələbə kimi daxil olmaq üçün 1, admin kimi daxil olmaq üçün 2, çıxış üçün 3 seçin");
            }
            else
            {
                if (userType == 1)
                {
                    Console.WriteLine("Tələbələrlə bağlı əməliyyatlara keçid etmək üçün 4 seçin");
                    Console.WriteLine("Fənnlərlə bağlı əməliyyatlara keçid etmək üçün 5 seçin");
                    Console.WriteLine("Suallarla bağlı əməliyyatlara keçid etmək üçün 6 seçin");
                    Console.WriteLine("Cavablarla bağlı əməliyyatlara keçid etmək üçün 7 seçin");
                    Console.WriteLine("İmtahanlarla bağlı əməliyyatlara keçid etmək üçün 8 seçin");
                    Console.WriteLine("Nəticələrə baxmaq üçün 9 seçin");
                }
                else
                {
                    Console.WriteLine("Xoş gəldiniz əziz tələbə:");
                    StudentMainMenu.Menu(userId);
                    goto Start;
                }
            }

            var selectedVariant = Console.ReadLine();

            switch (selectedVariant)
            {
                case "1":
                    logged = LoginController.StudentLogin(ref userId, ref userType);
                    if (!logged)
                    {
                        Console.WriteLine("Giriş uğursuz oldu, yenidən cəhd edin.");
                    }
                    goto Start;
                case "2":
                    logged = LoginController.AdminLogin(ref userId, ref userType);
                    if (!logged)
                    {
                        Console.WriteLine("Giriş uğursuz oldu, yenidən cəhd edin.");
                    }
                    goto Start;
                case "3":
                    Console.WriteLine("Çıxış edilir...");
                    break;
                case "4":
                    if (!logged) goto Start;
                    StudentOperations.StudentOperationsMenu();
                    goto Start;
                case "5":
                    if (!logged) goto Start;
                    SubjectOperations.SubjectOperationsMenu();
                    goto Start;
                case "6":
                    if (!logged) goto Start;
                    QuestionOperations.QuestionOperationsMenu();
                    goto Start;
                case "7":
                    if (!logged) goto Start;
                    AnswerOperations.AnswerOperationsMenu();
                    goto Start;
                case "8":
                    if (!logged) goto Start;
                    ExamOperations.ExamOperationsMenu();
                    goto Start;
                case "9":
                    if (!logged) goto Start;
                    ExamResultsController.ViewAllResults();
                    goto Start;
                default:
                    Console.WriteLine("Yanlış seçim. Yenidən cəhd edin.");
                    goto Start;
            }
        }
    }
}
