using ClosedXML.Excel;
using ExamSys.Models;
using System.Collections.Generic;
using System.Linq;

namespace ExamSys.Services
{
    public class AnswerExcelService
    {
        private const string FilePath = "answers.xlsx";

        public static List<Answer> answers = new List<Answer>();

        public static List<Answer> LoadAnswersFromExcel()
        {
            answers.Clear();

            if (!System.IO.File.Exists(FilePath))
                return answers;

            using (var workbook = new XLWorkbook(FilePath))
            {
                var worksheet = workbook.Worksheet("Answers");
                var rows = worksheet.RowsUsed().Skip(1);

                foreach (var row in rows)
                {
                    int id = row.Cell(1).GetValue<int>();
                    string title = row.Cell(2).GetValue<string>();
                    int questionId = row.Cell(3).GetValue<int>();
                    bool isTrue = row.Cell(4).GetValue<bool>();

                    answers.Add(new Answer(id, title, questionId, isTrue));
                }
            }

            return answers;
        }

        public static void SaveAnswersToExcel(List<Answer> answers)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Answers");

                worksheet.Cell(1, 1).Value = "ID";
                worksheet.Cell(1, 2).Value = "Title";
                worksheet.Cell(1, 3).Value = "QuestionId";
                worksheet.Cell(1, 4).Value = "IsTrue";

                int row = 2;
                foreach (var answer in answers)
                {
                    worksheet.Cell(row, 1).Value = answer.Id;
                    worksheet.Cell(row, 2).Value = answer.Title;
                    worksheet.Cell(row, 3).Value = answer.QuestionId;
                    worksheet.Cell(row, 4).Value = answer.IsTrue;
                    row++;
                }

                workbook.SaveAs(FilePath);
            }
        }

        public static List<Answer> GetAnswersByQuestionId(int questionId)
        {
            return answers.Where(a => a.QuestionId == questionId).ToList();
        }

        public static Answer GetTrueAnswerByQuestionId(int questionId)
        {
            return answers.FirstOrDefault(a => a.QuestionId == questionId && a.IsTrue);
        }

        public static void RemoveAnswerFromExcel(int answerId)
        {
            var answers = LoadAnswersFromExcel(); 
            var answerToRemove = answers.FirstOrDefault(a => a.Id == answerId);
            if (answerToRemove != null)
            {
                answers.Remove(answerToRemove);  
            }
            SaveAnswersToExcel(answers);  
        }


    }
}
