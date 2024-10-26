using ClosedXML.Excel;
using System.Collections.Generic;
using ExamSys.Models;
using System.Linq;
using System;
using ExamSys.models;

namespace ExamSys.Services
{
    public class ExcelService
    {
        private const string FilePath = "users.xlsx";
        private const string ResultsFilePath = "ExamResult.xlsx";
        private const string VariantsFilePath = "ExamVariants.xlsx";
        public static List<User> LoadUsersFromExcel()
        {
            var users = new List<User>();

            if (!System.IO.File.Exists(FilePath))
                return users;

            using (var workbook = new XLWorkbook(FilePath))
            {
                var worksheet = workbook.Worksheet("Users");
                var rows = worksheet.RowsUsed().Skip(1);

                foreach (var row in rows)
                {
                    int id = row.Cell(1).GetValue<int>();
                    string name = row.Cell(2).GetValue<string>();
                    string surname = row.Cell(3).GetValue<string>();
                    string login = row.Cell(4).GetValue<string>();
                    string password = row.Cell(5).GetValue<string>();
                    int type = row.Cell(6).GetValue<int>();

                    if (type == 1)
                    {
                        users.Add(new Admin(id, name, surname, login, password));
                    }
                    else if (type == 2)
                    {
                        users.Add(new Student(id, name, surname, login, password));
                    }
                }
            }

            return users;
        }

        public static List<Admin> GetAdmins()
        {
            var users = LoadUsersFromExcel();
            return users.OfType<Admin>().ToList();
        }

        public static List<Student> GetStudents()
        {
            var users = LoadUsersFromExcel();
            return users.OfType<Student>().ToList();
        }

        public static void SaveUsersToExcel(List<User> users)
        {
            var loadedUsers = LoadUsersFromExcel();

            foreach (var user in users)
            {
                var existingUser = loadedUsers.FirstOrDefault(u => u.Id == user.Id);
                if (existingUser != null)
                {
                    existingUser.Name = user.Name;
                    existingUser.Surname = user.Surname;
                    existingUser.Login = user.Login;
                    existingUser.Password = user.Password;
                }
                else
                {
                    loadedUsers.Add(user);
                }
            }


            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Users");

                worksheet.Cell(1, 1).Value = "ID";
                worksheet.Cell(1, 2).Value = "Name";
                worksheet.Cell(1, 3).Value = "Surname";
                worksheet.Cell(1, 4).Value = "Login";
                worksheet.Cell(1, 5).Value = "Password";
                worksheet.Cell(1, 6).Value = "Type";

                int row = 2;
                foreach (var user in loadedUsers)
                {
                    worksheet.Cell(row, 1).Value = user.Id;
                    worksheet.Cell(row, 2).Value = user.Name;
                    worksheet.Cell(row, 3).Value = user.Surname;
                    worksheet.Cell(row, 4).Value = user.Login;
                    worksheet.Cell(row, 5).Value = user.Password;
                    worksheet.Cell(row, 6).Value = user is Admin ? 1 : 2;
                    row++;
                }

                workbook.SaveAs(FilePath);
            }
        }


        public static void UpdateUserInExcel(User updatedUser)
        {
            var users = LoadUsersFromExcel(); 

            var existingUser = users.FirstOrDefault(u => u.Id == updatedUser.Id);
            if (existingUser != null)
            {
                users.Remove(existingUser); 
                users.Add(updatedUser); 
            }

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Users");

                worksheet.Cell(1, 1).Value = "ID";
                worksheet.Cell(1, 2).Value = "Name";
                worksheet.Cell(1, 3).Value = "Surname";
                worksheet.Cell(1, 4).Value = "Login";
                worksheet.Cell(1, 5).Value = "Password";
                worksheet.Cell(1, 6).Value = "Type";

                int row = 2;
                foreach (var user in users)
                {
                    worksheet.Cell(row, 1).Value = user.Id;
                    worksheet.Cell(row, 2).Value = user.Name;
                    worksheet.Cell(row, 3).Value = user.Surname;
                    worksheet.Cell(row, 4).Value = user.Login;
                    worksheet.Cell(row, 5).Value = user.Password;
                    worksheet.Cell(row, 6).Value = user is Admin ? 1 : 2;
                    row++;
                }

                workbook.SaveAs(FilePath);
            }
        }


        public static void RemoveUserFromExcel(int userId)
        {
            var users = LoadUsersFromExcel();
            var userToRemove = users.FirstOrDefault(u => u.Id == userId);
        

            if (userToRemove != null)
            {
                users.Remove(userToRemove);
            }


            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Users");

                worksheet.Cell(1, 1).Value = "ID";
                worksheet.Cell(1, 2).Value = "Name";
                worksheet.Cell(1, 3).Value = "Surname";
                worksheet.Cell(1, 4).Value = "Login";
                worksheet.Cell(1, 5).Value = "Password";
                worksheet.Cell(1, 6).Value = "Type";

                int row = 2;
                foreach (var user in users)
                {
                    worksheet.Cell(row, 1).Value = user.Id;
                    worksheet.Cell(row, 2).Value = user.Name;
                    worksheet.Cell(row, 3).Value = user.Surname;
                    worksheet.Cell(row, 4).Value = user.Login;
                    worksheet.Cell(row, 5).Value = user.Password;
                    worksheet.Cell(row, 6).Value = user is Admin ? 1 : 2;
                    row++;
                }

                workbook.SaveAs(FilePath);
            }
        }
        public static void SaveExamResults(int studentId, int examId, int correctAnswers, int skippedQuestions, int totalQuestions, int passingScore, int subjectId)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("ExamResults");

                worksheet.Cell(1, 1).Value = "Student ID";
                worksheet.Cell(1, 2).Value = "Exam ID";
                worksheet.Cell(1, 3).Value = "Correct Answers";
                worksheet.Cell(1, 4).Value = "Skipped Questions";
                worksheet.Cell(1, 5).Value = "Total Questions";
                worksheet.Cell(1, 6).Value = "Passing Score";
                worksheet.Cell(1, 7).Value = "Result";
                worksheet.Cell(1, 8).Value = "Subject ID";

                worksheet.Cell(2, 1).Value = studentId;
                worksheet.Cell(2, 2).Value = examId;
                worksheet.Cell(2, 3).Value = correctAnswers;
                worksheet.Cell(2, 4).Value = skippedQuestions;
                worksheet.Cell(2, 5).Value = totalQuestions;
                worksheet.Cell(2, 6).Value = passingScore;
                worksheet.Cell(2, 7).Value = correctAnswers >= passingScore ? "Passed" : "Failed";
                worksheet.Cell(2, 8).Value = subjectId;
                workbook.SaveAs(ResultsFilePath);
            }
        }

        public static void SaveExamVariants(int studentId, int examId, Dictionary<int, string> answers)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("ExamVariants");

                worksheet.Cell(1, 1).Value = "Student ID";
                worksheet.Cell(1, 2).Value = "Exam ID";
                worksheet.Cell(1, 3).Value = "Question ID";
                worksheet.Cell(1, 4).Value = "Selected Answer";

                int row = 2;
                foreach (var answer in answers)
                {
                    worksheet.Cell(row, 1).Value = studentId;
                    worksheet.Cell(row, 2).Value = examId;
                    worksheet.Cell(row, 3).Value = answer.Key;
                    worksheet.Cell(row, 4).Value = answer.Value;
                    row++;
                }

                workbook.SaveAs(VariantsFilePath);
            }
        }
      

    }
}
