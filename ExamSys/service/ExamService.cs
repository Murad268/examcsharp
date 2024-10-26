using ExamSys.Models;
using System.Collections.Generic;
using System.Linq;

namespace ExamSys.Services
{
    public class ExamService
    {
        private static List<Exam> exams = ExamExcelService.LoadExamsFromExcel();
        private static List<Student> students = StudentService.LoadStudentsFromExcelX();
        private static List<Subject> subjects = SubjectService.LoadSubjectsFromExcel();
        private static List<Question> questions = QuestionExcelService.LoadQuestionsFromExcel();
        public static void AddExam(Exam exam)
        {
            exams.Add(exam);
            ExamExcelService.SaveExamsToExcel(exams);
        }

        public static Exam GetExamById(int id)
        {
            return exams.FirstOrDefault(e => e.Id == id);
        }

        public static void UpdateExam(Exam updatedExam)
        {
            var exam = GetExamById(updatedExam.Id);
            if (exam != null)
            {
                exam.StudentId = updatedExam.StudentId;
                exam.SubjectId = updatedExam.SubjectId;
                exam.PassingScore = updatedExam.PassingScore;
                ExamExcelService.SaveExamsToExcel(exams);
            }
        }

        public static bool DeleteExam(int id)
        {
            var exam = GetExamById(id);
            if (exam != null)
            {
                exams.Remove(exam);
                ExamExcelService.SaveExamsToExcel(exams);
                return true;
            }
            return false;
        }

        public static List<Exam> GetAllExams()
        {
            return exams;
        }

        public static void DisplayExams()
        {
         

            foreach (var exam in exams) {
                string str = "";
                str += $"Id: {exam.Id}";
                int QuestionCount = questions.Where(question => question.SubjectId == exam.SubjectId).Count();
                Student student = students.Find(student => student.Id == exam.StudentId);
                str += $" İmtahan verəcək tələbənin tam adı: {student.Name} {student.Surname}";
                Subject subject = subjects.Find(subject => subject.Id == exam.SubjectId);
                str += $" İmtahan veriləcək fənn: {subject.Title}";
                str += $" Sual sayı: = {QuestionCount}";
                str += $" Minimum keçid üçün doğru yazılmalı sual sayı: {exam.PassingScore}";
                Console.WriteLine(str);
            }

         
        }
    }
}
