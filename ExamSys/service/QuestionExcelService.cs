using ClosedXML.Excel;
using ExamSys.Models;
using System.Collections.Generic;
using System.Linq;

namespace ExamSys.Services
{
    public class QuestionExcelService
    {
        private const string FilePath = "questions.xlsx";

        public static List<Question> LoadQuestionsFromExcel()
        {
            var questions = new List<Question>();

            if (!System.IO.File.Exists(FilePath))
                return questions;

            using (var workbook = new XLWorkbook(FilePath))
            {
                var worksheet = workbook.Worksheet("Questions");
                var rows = worksheet.RowsUsed().Skip(1);

                foreach (var row in rows)
                {
                    int id = row.Cell(1).GetValue<int>();
                    string title = row.Cell(2).GetValue<string>();
                    int subjectId = row.Cell(3).GetValue<int>();

                    questions.Add(new Question(id, title, subjectId));
                }
            }

            return questions;
        }

        public static void SaveQuestionsToExcel(List<Question> questions)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Questions");

                worksheet.Cell(1, 1).Value = "ID";
                worksheet.Cell(1, 2).Value = "Title";
                worksheet.Cell(1, 3).Value = "SubjectId";

                int row = 2;
                foreach (var question in questions)
                {
                    worksheet.Cell(row, 1).Value = question.Id;
                    worksheet.Cell(row, 2).Value = question.Title;
                    worksheet.Cell(row, 3).Value = question.SubjectId;
                    row++;
                }

                workbook.SaveAs(FilePath);
            }
        }
    }
}
