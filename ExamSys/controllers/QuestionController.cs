using ExamSys.Models;
using ExamSys.Services;
using ExamSys.service;
using System;
using System.Linq;

namespace ExamSys.Controllers
{
    internal class QuestionController
    {
        private static ValidationService validationService = new ValidationService();

        private static int GetNextId()
        {
            var questions = QuestionExcelService.LoadQuestionsFromExcel();
            if (questions.Count > 0)
            {
                return questions.Max(q => q.Id) + 1;
            }
            return 1;
        }

        public static void AddQuestion()
        {
            int questionIdCounter = GetNextId();

            string title = GetValidatedField("Sualın adını daxil edin:", "Sual adı boş ola bilməz.");

            int subjectId;
            do
            {
                SubjectService.LoadSubjectsFromExcel(); 
                SubjectService.DisplaySubjects();
                subjectId = GetValidatedIntegerField("Fənnin ID-sini daxil edin:", "Yanlış ID daxil edilib.");
                if (SubjectService.GetSubjectById(subjectId) == null)
                {
                    Console.WriteLine("Bu ID ilə fənn mövcud deyil. Yenidən daxil edin.");
                    subjectId = -1;
                }
            } while (subjectId == -1);

            Question question = new Question(questionIdCounter, title, subjectId);
            var questions = QuestionExcelService.LoadQuestionsFromExcel();
            questions.Add(question);

            QuestionExcelService.SaveQuestionsToExcel(questions);

            Console.WriteLine("Sual uğurla əlavə olundu.");
        }


        public static void UpdateQuestion()
        {
            var questions = QuestionExcelService.LoadQuestionsFromExcel();
            DisplayQuestions();

            int questionId;
            Question question;

            do
            {
                Console.WriteLine("Yeniləmək istədiyiniz sualın ID-sini daxil edin:");
                if (int.TryParse(Console.ReadLine(), out questionId))
                {
                    question = questions.FirstOrDefault(q => q.Id == questionId);
                    if (question == null)
                    {
                        Console.WriteLine("Bu ID ilə sual mövcud deyil. Yenidən daxil edin.");
                    }
                }
                else
                {
                    Console.WriteLine("Yanlış ID daxil edilib. Yenidən daxil edin.");
                    question = null;
                }

            } while (question == null);

            Console.WriteLine($"Sualın yeni adı (mövcud: {question.Title}):");
            string newTitle = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newTitle))
            {
                question.Title = newTitle;
            }
            else
            {
                Console.WriteLine("Yeni dəyər daxil edilmədiyi üçün köhnə dəyər saxlanıldı.");
            }

            Console.WriteLine($"Yeni fənnin ID-si (mövcud: {question.SubjectId}):");
            string newSubjectIdStr = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(newSubjectIdStr))
            {
                int newSubjectId;
                do
                {
                    if (int.TryParse(newSubjectIdStr, out newSubjectId) && SubjectService.GetSubjectById(newSubjectId) != null)
                    {
                        question.SubjectId = newSubjectId; 
                    }
                    else
                    {
                        Console.WriteLine("Bu ID ilə fənn mövcud deyil. Yenidən daxil edin və ya Enter-a basın köhnəni saxlamaq üçün.");
                        newSubjectIdStr = Console.ReadLine();
                        if (string.IsNullOrWhiteSpace(newSubjectIdStr))
                        {
                            break; 
                        }
                    }
                } while (!string.IsNullOrWhiteSpace(newSubjectIdStr));
            }
            else
            {
                Console.WriteLine("Yeni dəyər daxil edilmədiyi üçün köhnə dəyər saxlanıldı.");
            }

            QuestionExcelService.SaveQuestionsToExcel(questions);
            Console.WriteLine("Sual məlumatları yeniləndi.");
        }



        public static void DeleteQuestion()
        {
            var questions = QuestionExcelService.LoadQuestionsFromExcel();
            DisplayQuestions();

            Console.WriteLine("Silmək istədiyiniz sualın ID-sini daxil edin:");
            if (int.TryParse(Console.ReadLine(), out int questionId))
            {
                var question = questions.FirstOrDefault(q => q.Id == questionId);
                if (question != null)
                {
                    var answers = AnswerExcelService.GetAnswersByQuestionId(questionId);
                    if (answers.Count > 0)
                    {
                        foreach (var answer in answers)
                        {
                            AnswerExcelService.RemoveAnswerFromExcel(answer.Id);
                        }
                    }

                    questions.Remove(question);
                    QuestionExcelService.SaveQuestionsToExcel(questions);

                    Console.WriteLine("Sual silindi.");
                }
                else
                {
                    Console.WriteLine("Sual tapılmadı.");
                }
            }
            else
            {
                Console.WriteLine("Yanlış ID daxil edilib.");
            }
        }




        public static void DisplayQuestions()
        {
            var subjects = SubjectService.LoadSubjectsFromExcel();
            var questions = QuestionExcelService.LoadQuestionsFromExcel();

            Console.WriteLine("Mövcud suallar:");

            foreach (var subject in subjects)
            {
                Console.WriteLine($"Fənn: {subject.Title}");

                var subjectQuestions = questions.Where(q => q.SubjectId == subject.Id).ToList();
                if (subjectQuestions.Count > 0)
                {
                    foreach (var question in subjectQuestions)
                    {
                        Console.WriteLine($"\tSual ID: {question.Id}, Sual: {question.Title}");
                    }
                }
                else
                {
                    Console.WriteLine("\tBu fənnə aid sual yoxdur.");
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
    }
}
