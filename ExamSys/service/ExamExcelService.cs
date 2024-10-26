using ClosedXML.Excel;
using ExamSys.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ExamSys.Services
{
    public class ExamExcelService
    {
        private const string FilePath = "exams.xlsx";

        public static List<Exam> LoadExamsFromExcel()
        {
            var exams = new List<Exam>();

            if (!File.Exists(FilePath))
                return exams;

            using (var workbook = new XLWorkbook(FilePath))
            {
                var worksheet = workbook.Worksheet("Exams");
                var rows = worksheet.RowsUsed().Skip(1);

                foreach (var row in rows)
                {
                    int id = row.Cell(1).GetValue<int>();
                    int studentId = row.Cell(2).GetValue<int>();
                    int subjectId = row.Cell(3).GetValue<int>();
                    int passingScore = row.Cell(4).GetValue<int>();

                    exams.Add(new Exam(id, studentId, subjectId, passingScore));
                }
            }

            return exams;
        }

        public static void SaveExamsToExcel(List<Exam> exams)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Exams");

                worksheet.Cell(1, 1).Value = "ID";
                worksheet.Cell(1, 2).Value = "StudentId";
                worksheet.Cell(1, 3).Value = "SubjectId";
                worksheet.Cell(1, 4).Value = "PassingScore";

                int row = 2;
                foreach (var exam in exams)
                {
                    worksheet.Cell(row, 1).Value = exam.Id;
                    worksheet.Cell(row, 2).Value = exam.StudentId;
                    worksheet.Cell(row, 3).Value = exam.SubjectId;
                    worksheet.Cell(row, 4).Value = exam.PassingScore;
                    row++;
                }

                workbook.SaveAs(FilePath);
            }
        }
    }
}
