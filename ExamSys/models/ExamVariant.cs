using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamSys.models
{
    public class ExamVariant
    {
        public int StudentID { get; set; }
        public int ExamID { get; set; }
        public int QuestionID { get; set; }
        public string SelectedAnswer { get; set; }
    }
}
