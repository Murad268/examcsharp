using System;
using System.Collections.Generic;
using ClosedXML.Excel;
using ExamSys.models;
using ExamSys.Models;
using ExamSys.Services;

namespace ExamSys.Controllers
{
    public class ExamResultsController
    {
        private static List<Exam> exams = ExamExcelService.LoadExamsFromExcel();
        private static List<Student> students = StudentService.LoadStudentsFromExcelX();
        private static List<Subject> subjects = SubjectService.LoadSubjectsFromExcel();
        private static List<Question> questions = QuestionExcelService.LoadQuestionsFromExcel();
        public static List<ExamResult> LoadExamResultsFromExcel(string filePath)
        {
            List<ExamResult> results = new List<ExamResult>();

            using (var workbook = new XLWorkbook(filePath))
            {
                var worksheet = workbook.Worksheet(1);
                var rowCount = worksheet.LastRowUsed().RowNumber();

                for (int row = 2; row <= rowCount; row++)
                {
                    results.Add(new ExamResult
                    {
                        StudentID = int.Parse(worksheet.Cell(row, 1).GetString()),
                        ExamID = int.Parse(worksheet.Cell(row, 2).GetString()),
                        CorrectAnswers = int.Parse(worksheet.Cell(row, 3).GetString()),
                        SkippedQuestions = int.Parse(worksheet.Cell(row, 4).GetString()),
                        TotalQuestions = int.Parse(worksheet.Cell(row, 5).GetString()),
                        PassingScore = int.Parse(worksheet.Cell(row, 6).GetString()),
                        Result = worksheet.Cell(row, 7).GetString(),
                        SubjectId = int.Parse(worksheet.Cell(row, 8).GetString()),
                    });
                }
            }

            return results;
        }

        public static List<ExamVariant> LoadExamVariantsFromExcel(string filePath)
        {
            List<ExamVariant> variants = new List<ExamVariant>();

            using (var workbook = new XLWorkbook(filePath))
            {
                var worksheet = workbook.Worksheet(1);
                var rowCount = worksheet.LastRowUsed().RowNumber();

                for (int row = 2; row <= rowCount; row++)
                {
                    variants.Add(new ExamVariant
                    {
                        StudentID = int.Parse(worksheet.Cell(row, 1).GetString()),
                        ExamID = int.Parse(worksheet.Cell(row, 2).GetString()),
                        QuestionID = int.Parse(worksheet.Cell(row, 3).GetString()),
                        SelectedAnswer = worksheet.Cell(row, 4).GetString()
                    });
                }
            }

            return variants;
        }

        public static void ViewAllResults()
        {
            string examResultsFilePath = "ExamResult.xlsx";
            string examVariantsFilePath = "ExamVariants.xlsx";

            List<ExamResult> examResults = LoadExamResultsFromExcel(examResultsFilePath);
            List<ExamVariant> examVariants = LoadExamVariantsFromExcel(examVariantsFilePath);

            foreach (var result in examResults)
            {
                Student student = students.FirstOrDefault(x => x.Id == result.StudentID );
                Subject subject = subjects.FirstOrDefault(x => x.Id == result.SubjectId);

                if (student != null && subject != null)
                {
                    Console.WriteLine($"Tələbənin tam adı: {student.Name} {student.Surname}, Fən: {subject.Title}, " +
                                      $"ExamID: {result.ExamID}, Düzgün Cavablar: {result.CorrectAnswers}, " +
                                      $"Atlanmış Suallar: {result.SkippedQuestions}, Ümumi Suallar: {result.TotalQuestions}, " +
                                      $"Keçid Balı: {result.PassingScore}, Nəticə: {result.Result}");
                }
                else
                {
                    Console.WriteLine($"Məlumat tapılmadı. Student ID: {result.StudentID}, Subject ID: {result.SubjectId}");
                }
            }
        }
    }
}
