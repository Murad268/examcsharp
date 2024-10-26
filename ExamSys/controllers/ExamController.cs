using ExamSys.Models;
using ExamSys.Services;
using System;

namespace ExamSys.Controllers
{
    public class ExamController
    {
        public static void AddExam()
        {
            Console.WriteLine("Yeni imtahan əlavə et:");

            int studentId;
            do
            {
                Console.WriteLine("Mövcud Tələbələr:");
                StudentService.LoadStudentsFromExcel();
                StudentService.DisplayStudents();

                studentId = GetValidatedIntegerField("Tələbə ID-sini daxil edin:", "Yanlış tələbə ID-si.");

                if (StudentService.GetStudentById(studentId) == null)
                {
                    Console.WriteLine("Bu ID ilə tələbə tapılmadı. Yenidən daxil edin.");
                    studentId = -1;
                }
            } while (studentId == -1);

            int subjectId;
            int questionCount = 0;  
            do
            {
                Console.WriteLine("Mövcud Fənnlər:");
                SubjectService.LoadSubjectsFromExcel();
                SubjectService.DisplaySubjects();

                subjectId = GetValidatedIntegerField("Fənn ID-sini daxil edin:", "Yanlış fənn ID-si.");

                if (SubjectService.GetSubjectById(subjectId) == null)
                {
                    Console.WriteLine("Bu ID ilə fənn tapılmadı. Yenidən daxil edin.");
                    subjectId = -1;
                }
                else
                {
                    questionCount = QuestionExcelService.LoadQuestionsFromExcel().Count(q => q.SubjectId == subjectId);
                    Console.WriteLine($"Bu fənn üçün mövcud sual sayı: {questionCount}");
                }
            } while (subjectId == -1);

            int passingScore;
            do
            {
                passingScore = GetValidatedIntegerField("Keçid balını daxil edin:", "Yanlış keçid balı.");

                if (passingScore > questionCount)
                {
                    Console.WriteLine($"Keçid balı sual sayından çox ola bilməz. Ən çox {questionCount} daxil edə bilərsiniz.");
                }
            } while (passingScore > questionCount);

            int nextId = ExamService.GetAllExams().Count > 0 ? ExamService.GetAllExams().Max(e => e.Id) + 1 : 1;
            Exam newExam = new Exam(nextId, studentId, subjectId, passingScore);

            ExamService.AddExam(newExam);
            Console.WriteLine("İmtahan uğurla əlavə olundu.");
        }

        public static void UpdateExam()
        {
            Console.WriteLine("Yeniləmək istədiyiniz imtahanın ID-sini daxil edin:");
            ExamService.DisplayExams();
            int examId = GetValidatedIntegerField("İmtahan ID-sini daxil edin:", "Yanlış imtahan ID-si.");

            Exam examToUpdate = ExamService.GetExamById(examId);
            if (examToUpdate != null)
            {
                int newStudentId;
                do
                {
                    Console.WriteLine("Mövcud Tələbələr:");
                    StudentService.LoadStudentsFromExcel();
                    StudentService.DisplayStudents();

                    newStudentId = GetValidatedIntegerField("Yeni tələbə ID-sini daxil edin (keçmək üçün Enter basın):", null);

                    if (newStudentId > 0 && StudentService.GetStudentById(newStudentId) == null)
                    {
                        Console.WriteLine("Bu ID ilə tələbə tapılmadı. Yenidən daxil edin.");
                        newStudentId = -1;
                    }
                } while (newStudentId == -1);

                int newSubjectId;
                int questionCount = 0; 
                do
                {
                    Console.WriteLine("Mövcud Fənnlər:");
                    SubjectService.LoadSubjectsFromExcel();
                    SubjectService.DisplaySubjects();

                    newSubjectId = GetValidatedIntegerField("Yeni fənn ID-sini daxil edin (keçmək üçün Enter basın):", null);

                    if (newSubjectId > 0 && SubjectService.GetSubjectById(newSubjectId) == null)
                    {
                        Console.WriteLine("Bu ID ilə fənn tapılmadı. Yenidən daxil edin.");
                        newSubjectId = -1;
                    }
                    else if (newSubjectId > 0)
                    {
                        questionCount = QuestionExcelService.LoadQuestionsFromExcel().Count(q => q.SubjectId == newSubjectId);
                        Console.WriteLine($"Bu fənn üçün mövcud sual sayı: {questionCount}");
                    }
                } while (newSubjectId == -1);

                int newPassingScore;
                do
                {
                    newPassingScore = GetValidatedIntegerField("Yeni keçid balını daxil edin:", null);

                    if (newPassingScore > questionCount)
                    {
                        Console.WriteLine($"Keçid balı sual sayından çox ola bilməz. Ən çox {questionCount} daxil edə bilərsiniz.");
                    }
                } while (newPassingScore > questionCount);

                examToUpdate.StudentId = newStudentId > 0 ? newStudentId : examToUpdate.StudentId;
                examToUpdate.SubjectId = newSubjectId > 0 ? newSubjectId : examToUpdate.SubjectId;
                examToUpdate.PassingScore = newPassingScore > 0 ? newPassingScore : examToUpdate.PassingScore;

                ExamService.UpdateExam(examToUpdate);
                Console.WriteLine("İmtahan məlumatları yeniləndi.");
            }
            else
            {
                Console.WriteLine("Bu ID ilə imtahan tapılmadı.");
            }
        }

        public static void DeleteExam()
        {
            ExamService.DisplayExams();
            int examId = GetValidatedIntegerField("Silinməsini istədiyiniz imtahanın ID-sini daxil edin:", "Yanlış imtahan ID-si.");

            if (ExamService.DeleteExam(examId))
            {
                Console.WriteLine("İmtahan uğurla silindi.");
            }
            else
            {
                Console.WriteLine("Bu ID ilə imtahan tapılmadı.");
            }
        }

        private static int GetValidatedIntegerField(string prompt, string errorMessage)
        {
            int fieldValue;
            while (true)
            {
                Console.WriteLine(prompt);
                string input = Console.ReadLine();

                if (int.TryParse(input, out fieldValue) && fieldValue > 0)
                {
                    return fieldValue;
                }
                else if (!string.IsNullOrEmpty(errorMessage))
                {
                    Console.WriteLine(errorMessage);
                }
            }
        }
    }
}
