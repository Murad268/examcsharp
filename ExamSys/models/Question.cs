using System;

namespace ExamSys.Models
{
    public class Question
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int SubjectId { get; set; }

        public Question(int id, string title, int subjectId)
        {
            Id = id;
            Title = title;
            SubjectId = subjectId;
        }
    }
}
