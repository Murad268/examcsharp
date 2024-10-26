using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamSys.Models
{
    public class ExamResult
    {
        public int StudentID { get; set; }
        public int ExamID { get; set; }
        public int CorrectAnswers { get; set; }
        public int SkippedQuestions { get; set; }
        public int TotalQuestions { get; set; }
        public int PassingScore { get; set; }
        public string Result { get; set; }
        public int SubjectId { get; set; }
    }

   
}
