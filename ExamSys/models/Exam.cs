namespace ExamSys.Models
{
    public class Exam
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int SubjectId { get; set; }
        public int PassingScore { get; set; }

        public Exam(int id, int studentId, int subjectId, int passingScore)
        {
            Id = id;
            StudentId = studentId;
            SubjectId = subjectId;
            PassingScore = passingScore;
        }
    }
}
