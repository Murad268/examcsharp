using ExamSys.Models;
using ExamSys.Services;
using ExamSys.service;
using System;
using System.Linq;

namespace ExamSys.Controllers
{
    internal class AnswerController
    {
        private static ValidationService validationService = new ValidationService();

        private static int GetNextId()
        {
            var answers = AnswerExcelService.LoadAnswersFromExcel();
            if (answers.Count > 0)
            {
                return answers.Max(a => a.Id) + 1;
            }
            return 1;
        }

        public static void AddAnswer()
        {
            int answerIdCounter = GetNextId();

            int questionId;
            do
            {
                QuestionService.LoadQuestionsFromExcel();
                DisplayQuestionsBySubject();
                questionId = GetValidatedIntegerField("Sualın ID-sini daxil edin:", "Yanlış ID daxil edilib.");
                if (QuestionService.GetQuestionById(questionId) == null)
                {
                    Console.WriteLine("Bu ID ilə sual tapılmadı. Yenidən daxil edin.");
                    questionId = -1;
                }
            } while (questionId == -1);

            if (AnswerExcelService.GetAnswersByQuestionId(questionId).Count >= 4)
            {
                Console.WriteLine("Bu sual üçün artıq 4 cavab əlavə olunub.");
                return;
            }

            string title = GetValidatedField("Cavabın adını daxil edin:", "Cavab adı boş ola bilməz.");
            bool isTrue = GetValidatedBooleanField("Bu cavab doğrudurmu? (h/y):", "Yanlış seçim.");

            if (isTrue && AnswerExcelService.GetTrueAnswerByQuestionId(questionId) != null)
            {
                Console.WriteLine("Bu sual üçün artıq doğru cavab təyin olunub.");
                return;
            }

            Answer answer = new Answer(answerIdCounter, title, questionId, isTrue);
            var answers = AnswerExcelService.LoadAnswersFromExcel();
            answers.Add(answer);

            AnswerExcelService.SaveAnswersToExcel(answers);
            Console.WriteLine("Cavab uğurla əlavə olundu.");
        }

        public static void UpdateAnswer()
        {
            var answers = AnswerExcelService.LoadAnswersFromExcel();
            DisplayAnswers();

            int answerId;
            do
            {
                Console.WriteLine("Yeniləmək istədiyiniz cavabın ID-sini daxil edin:");
                answerId = GetValidatedIntegerField("Cavabın ID-sini daxil edin:", "Yanlış ID daxil edilib.");
                if (answers.FirstOrDefault(a => a.Id == answerId) == null)
                {
                    Console.WriteLine("Bu ID ilə cavab tapılmadı. Yenidən daxil edin.");
                    answerId = -1;
                }
            } while (answerId == -1);

            var answer = answers.FirstOrDefault(a => a.Id == answerId);

            Console.WriteLine($"Cavabın yeni adı (mövcud: {answer.Title}):");
            string newTitle = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newTitle))
            {
                answer.Title = newTitle;
            }

            Console.WriteLine($"Doğruluq statusunu dəyişmək istəyirsinizmi? (Mövcud: {(answer.IsTrue ? "h" : "y")}) (h/y/n):");
            string input = Console.ReadLine()?.Trim().ToLower();

            if (input == "h" || input == "y")
            {
                bool newIsTrue = (input == "h");

                if (newIsTrue && AnswerExcelService.GetTrueAnswerByQuestionId(answer.QuestionId) != null && !answer.IsTrue)
                {
                    Console.WriteLine("Bu sual üçün artıq doğru cavab təyin olunub.");
                }
                else
                {
                    answer.IsTrue = newIsTrue;
                }
            }

            AnswerExcelService.SaveAnswersToExcel(answers);
            Console.WriteLine("Cavab məlumatları yeniləndi.");
        }



        public static void DeleteAnswer()
        {
            var answers = AnswerExcelService.LoadAnswersFromExcel();
            DisplayAnswers();

            int answerId;
            do
            {
                Console.WriteLine("Silmək istədiyiniz cavabın ID-sini daxil edin:");
                answerId = GetValidatedIntegerField("Cavabın ID-sini daxil edin:", "Yanlış ID daxil edilib.");
                if (answers.FirstOrDefault(a => a.Id == answerId) == null)
                {
                    Console.WriteLine("Bu ID ilə cavab tapılmadı. Yenidən daxil edin.");
                    answerId = -1;
                }
            } while (answerId == -1);

            var answer = answers.FirstOrDefault(a => a.Id == answerId);
            answers.Remove(answer);

            AnswerExcelService.SaveAnswersToExcel(answers);

            Console.WriteLine("Cavab silindi.");
        }

        public static void DisplayAnswers()
        {
            var subjects = SubjectService.LoadSubjectsFromExcel();
            var questions = QuestionExcelService.LoadQuestionsFromExcel();
            var answers = AnswerExcelService.LoadAnswersFromExcel();

            Console.WriteLine("Mövcud fənnlər, suallar və cavablar:");

            foreach (var subject in subjects)
            {
                Console.WriteLine($"Fənn: {subject.Title}");

                var subjectQuestions = questions.Where(q => q.SubjectId == subject.Id).ToList();
                if (!subjectQuestions.Any())
                {
                    Console.WriteLine($"\tBu fənnə aid sual yoxdur.");
                }
                else
                {
                    foreach (var question in subjectQuestions)
                    {
                        Console.WriteLine($"\tSual: {question.Title}");

                        var questionAnswers = answers.Where(a => a.QuestionId == question.Id).ToList();
                        if (!questionAnswers.Any())
                        {
                            Console.WriteLine($"\t\tBu suala aid cavab yoxdur.");
                        }
                        else
                        {
                            foreach (var answer in questionAnswers)
                            {
                                string correctness = answer.IsTrue ? "h" : "y";
                                Console.WriteLine($"\t\tCavab ID: {answer.Id}, Cavab: {answer.Title}, Doğrudurmu: {correctness}");
                            }
                        }
                    }
                }
            }
        }


        private static void DisplayQuestionsBySubject()
        {
            var subjects = SubjectService.LoadSubjectsFromExcel();
            var questions = QuestionExcelService.LoadQuestionsFromExcel();

            foreach (var subject in subjects)
            {
                Console.WriteLine($"Fənn: {subject.Title}");
                var subjectQuestions = questions.Where(q => q.SubjectId == subject.Id).ToList();

                if (!subjectQuestions.Any())
                {
                    Console.WriteLine($"\tBu fənnə aid sual yoxdur.");
                }
                else
                {
                    foreach (var question in subjectQuestions)
                    {
                        Console.WriteLine($"\tSual ID: {question.Id}, Sual: {question.Title}");
                    }
                }
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

        private static int GetValidatedIntegerField(string promptMessage, string errorMessage)
        {
            int fieldValue;
            do
            {
                Console.WriteLine(promptMessage);
                string input = Console.ReadLine();

                if (!int.TryParse(input, out fieldValue) || fieldValue <= 0)
                {
                    Console.WriteLine(errorMessage);
                    fieldValue = -1;
                }

            } while (fieldValue == -1);

            return fieldValue;
        }

        private static bool GetValidatedBooleanField(string promptMessage, string errorMessage)
        {
            bool fieldValue = false;
            do
            {
                Console.WriteLine(promptMessage);
                string input = Console.ReadLine()?.Trim().ToLower();

                if (input == "h" )
                {
                    fieldValue = true;
                    break;
                }
                else if (input == "y")
                {
                    fieldValue = false;
                    break;
                }
                else
                {
                    Console.WriteLine(errorMessage);
                }

            } while (true);

            return fieldValue;
        }
    }
}
