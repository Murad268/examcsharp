using ClosedXML.Excel;
using ExamSys.Models;
using System.Collections.Generic;
using System.Linq;

namespace ExamSys.Services
{
    public class SubjectExcelService
    {
        private const string FilePath = "subjects.xlsx";

        public static List<Subject> LoadSubjectsFromExcel()
        {
            var subjects = new List<Subject>();

            if (!System.IO.File.Exists(FilePath))
                return subjects;

            using (var workbook = new XLWorkbook(FilePath))
            {
                var worksheet = workbook.Worksheet("Subjects");
                var rows = worksheet.RowsUsed().Skip(1);

                foreach (var row in rows)
                {
                    int id = row.Cell(1).GetValue<int>();
                    string title = row.Cell(2).GetValue<string>();
                    subjects.Add(new Subject(id, title));
                }
            }

            return subjects;
        }

        public static void SaveSubjectsToExcel(List<Subject> subjects)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Subjects");

                worksheet.Cell(1, 1).Value = "ID";
                worksheet.Cell(1, 2).Value = "Title";

                int row = 2;
                foreach (var subject in subjects)
                {
                    worksheet.Cell(row, 1).Value = subject.Id;
                    worksheet.Cell(row, 2).Value = subject.Title;
                    row++;
                }

                workbook.SaveAs(FilePath);
            }
        }

        public static void RemoveSubjectFromExcel(int subjectId)
        {
            var subjects = LoadSubjectsFromExcel();
            var subjectToRemove = subjects.FirstOrDefault(s => s.Id == subjectId);
            if (subjectToRemove != null)
            {
                subjects.Remove(subjectToRemove);
            }
            SaveSubjectsToExcel(subjects);
        }
    }
}
