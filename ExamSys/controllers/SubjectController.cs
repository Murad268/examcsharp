using ExamSys.Models;
using ExamSys.Services;
using ExamSys.service;
using System;
using System.Linq;

namespace ExamSys.Controllers
{
    internal class SubjectController
    {
        private static ValidationService validationService = new ValidationService();

        private static int GetNextId()
        {
            var subjects = SubjectService.subjects;
            if (subjects.Count > 0)
            {
                return subjects.Max(s => s.Id) + 1;
            }
            return 1;
        }

        public static void AddSubject()
        {
            int subjectIdCounter = GetNextId();

            string title = GetValidatedField("Fənnin adını daxil edin:", "Fənn adı boş ola bilməz.");

            Subject subject = new Subject(subjectIdCounter, title);
            SubjectService.subjects.Add(subject);

            SubjectExcelService.SaveSubjectsToExcel(SubjectService.subjects);

            Console.WriteLine("Fənn uğurla əlavə olundu.");
        }

        public static void UpdateSubject()
        {
            SubjectService.LoadSubjectsFromExcel();
            SubjectService.DisplaySubjects();

            Console.WriteLine("Yeniləmək istədiyiniz fənnin ID-sini daxil edin:");
            if (int.TryParse(Console.ReadLine(), out int subjectId))
            {
                var subject = SubjectService.GetSubjectById(subjectId);
                if (subject == null)
                {
                    Console.WriteLine("Fənn tapılmadı.");
                    return;
                }

                Console.WriteLine($"Fənnin yeni adı (mövcud: {subject.Title}):");
                string newTitle = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(newTitle))
                {
                    subject.Title = newTitle;
                }
                else
                {
                    Console.WriteLine("Yeni dəyər daxil edilmədiyi üçün köhnə dəyər saxlanıldı.");
                }

                SubjectExcelService.SaveSubjectsToExcel(SubjectService.subjects);

                Console.WriteLine("Fənn məlumatları yeniləndi.");
            }
            else
            {
                Console.WriteLine("Yanlış ID daxil edilib.");
            }
        }

        public static void DeleteSubject()
        {
            SubjectService.LoadSubjectsFromExcel();
            SubjectService.DisplaySubjects();

            Console.WriteLine("Silmək istədiyiniz fənnin ID-sini daxil edin:");
            if (int.TryParse(Console.ReadLine(), out int subjectId))
            {
                var subject = SubjectService.GetSubjectById(subjectId);
                if (subject != null)
                {
                    QuestionService.LoadQuestionsFromExcel();
                    AnswerExcelService.LoadAnswersFromExcel();

                    var subjectQuestions = QuestionService.questions.Where(q => q.SubjectId == subjectId).ToList();

                    foreach (var question in subjectQuestions)
                    {
                        var relatedAnswers = AnswerExcelService.answers.Where(a => a.QuestionId == question.Id).ToList();

                        foreach (var answer in relatedAnswers)
                        {
                            AnswerExcelService.answers.Remove(answer);
                        }

                        QuestionService.questions.Remove(question);
                    }

                    SubjectService.subjects.Remove(subject);

                    AnswerExcelService.SaveAnswersToExcel(AnswerExcelService.answers);
                    QuestionExcelService.SaveQuestionsToExcel(QuestionService.questions);
                    SubjectExcelService.SaveSubjectsToExcel(SubjectService.subjects);

                    Console.WriteLine("Fənn və ona aid olan bütün suallar və cavablar silindi.");
                }
                else
                {
                    Console.WriteLine("Fənn tapılmadı.");
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
