using ExamSys.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExamSys.Services
{
    public class QuestionService
    {
        public static List<Question> questions = new List<Question>();

        public static void DisplayQuestions()
        {
            if (questions.Count == 0)
            {
                Console.WriteLine("Hal-hazırda siyahıda sual yoxdur.");
                return;
            }

            foreach (var question in questions)
            {
                Console.WriteLine($"ID: {question.Id}, Sual: {question.Title}, Fənn ID: {question.SubjectId}");
            }
        }

        public static Question GetQuestionById(int id)
        {
            return questions.FirstOrDefault(q => q.Id == id);
        }

        public static void LoadQuestionsFromExcel()
        {
            questions = QuestionExcelService.LoadQuestionsFromExcel();
        }
    }
}
