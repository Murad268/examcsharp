using ExamSys.Controllers;
using ExamSys.Models;
using ExamSys.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExamSys.Service
{
    public class StudentMainMenu
    {
        private static List<Exam> exams = ExamExcelService.LoadExamsFromExcel();
        private static List<Student> students = StudentService.LoadStudentsFromExcelX();
        private static List<Subject> subjects = SubjectService.LoadSubjectsFromExcel();
        private static List<Answer> answers = AnswerExcelService.LoadAnswersFromExcel();
        private static List<Question> questions = QuestionExcelService.LoadQuestionsFromExcel();
        public static void Menu(int studentId)
        {
            Student student = students.Find(student => student.Id == studentId);
            Console.WriteLine($"{student.Name} {student.Surname}");
            Console.WriteLine("İmtahan verə biləcəyiniz fənlər aşağıdakılardır:");

            exams = exams.Where(exam => exam.StudentId == studentId).ToList();
            foreach (var exam in exams)
            {
                string str = $"ID: {exam.Id} - Fənn: {subjects.Find(s => s.Id == exam.SubjectId).Title}, Keçid Balı: {exam.PassingScore}";
                Console.WriteLine(str);
            }
            if (exams.Count < 1)
            {
                Console.WriteLine("Sizin üçün heç bir imtahan nəzərdə tutulmayıb.");
            }
            else
            {
                
                Console.WriteLine("İmtahan ID-sini daxil edin və başlamaq üçün Enter basın.");
                int examId = int.Parse(Console.ReadLine());
              
                StartExam(studentId, examId);
            }
        }

        private static void StartExam(int studentId, int examId)
        {
            Exam selectedExam = exams.FirstOrDefault(e => e.Id == examId);
            if (selectedExam == null)
            {
                Console.WriteLine("Yanlış imtahan ID-si. Yenidən cəhd edin.");
                return;
            }

            List<Question> questions = QuestionExcelService.LoadQuestionsFromExcel()
                .Where(q => q.SubjectId == selectedExam.SubjectId)
                .OrderBy(x => Guid.NewGuid()).ToList();

            Dictionary<int, string> answers = new Dictionary<int, string>();
            Dictionary<int, bool> correctness = new Dictionary<int, bool>();
            int skippedQuestions = 0;
            int currentQuestionIndex = 0;

            while (currentQuestionIndex < questions.Count)
            {
                var question = questions[currentQuestionIndex];
                Console.WriteLine($"Sual {currentQuestionIndex + 1}: {question.Title}");
                List<Answer> answerOptions = AnswerExcelService.GetAnswersByQuestionId(question.Id);

                for (int i = 0; i < answerOptions.Count; i++)
                {
                    Console.WriteLine($"{i + 1}) {answerOptions[i].Title}");
                }

                Console.WriteLine("Cavabınızı daxil edin:");
                if (currentQuestionIndex > 0)
                {
                    Console.WriteLine("'geri' - əvvəlki suala qayıtmaq");
                }
                if (currentQuestionIndex < questions.Count - 1)
                {
                    Console.WriteLine("Cavab seçmək üçün nömrə daxil edin və Enter basın.");
                }
                

                string userAnswer = Console.ReadLine()?.ToLower();

                if (userAnswer == "geri" && currentQuestionIndex > 0)
                {
                    currentQuestionIndex--;
                    continue;
                }
                else if (userAnswer == "tesdiq" && currentQuestionIndex == questions.Count - 1)
                {
                    break;
                }
                else if (int.TryParse(userAnswer, out int answerIndex) && answerIndex > 0 && answerIndex <= answerOptions.Count)
                {
                    answers[question.Id] = answerOptions[answerIndex - 1].Title;
                    correctness[question.Id] = answerOptions[answerIndex - 1].IsTrue;
                }
                else
                {
                    Console.WriteLine("Yanlış cavab seçimi. Yenidən cəhd edin.");
                    continue;
                }

                currentQuestionIndex++;
            }

            int correctAnswers = correctness.Values.Count(c => c);
            SaveExamResults(studentId, examId, correctAnswers, skippedQuestions, questions.Count, answers, selectedExam.PassingScore, selectedExam.SubjectId);
            FinalizeExam(examId, correctAnswers, selectedExam.PassingScore, studentId);
        }

        private static void SaveExamResults(int studentId, int examId, int correctAnswers, int skippedQuestions, int totalQuestions, Dictionary<int, string> answers, int passingScore, int subjectId)
        {
            ExcelService.SaveExamResults(studentId, examId, correctAnswers, skippedQuestions, totalQuestions, passingScore, subjectId);
            ExcelService.SaveExamVariants(studentId, examId, answers);
        }

        private static void FinalizeExam(int examId, int correctAnswers, int passingScore, int studentId)
        {
            Console.WriteLine(correctAnswers >= passingScore ? "Təbriklər, keçdiniz!" : "Təəssüf ki, keçə bilmədiniz.");
            ExamService.DeleteExam(examId);
            Console.WriteLine("İmtahan bitdi. Əsas menyuya qayıtmaq üçün Enter basın.");
            Console.ReadLine();
            Menu(studentId);
        }
    }
}
