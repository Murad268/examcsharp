using ExamSys.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExamSys.Services
{
    public class SubjectService
    {
        public static List<Subject> subjects = new List<Subject>();

        public static void DisplaySubjects()
        {
            LoadSubjectsFromExcel(); 
            if (subjects.Count == 0)
            {
                Console.WriteLine("Hal-hazırda mövcud fənn yoxdur.");
                return;
            }

            foreach (var subject in subjects)
            {
                Console.WriteLine($"ID: {subject.Id}, Title: {subject.Title}");
            }
        }

        public static Subject GetSubjectById(int id)
        {
            LoadSubjectsFromExcel(); 
            return subjects.FirstOrDefault(s => s.Id == id);
        }

        public static List<Subject> LoadSubjectsFromExcel()
        {
            subjects = SubjectExcelService.LoadSubjectsFromExcel();
            return subjects;
        }
    }
}
