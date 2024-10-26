using System;

namespace ExamSys.Models
{
    public class Answer
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int QuestionId { get; set; }
        public bool IsTrue { get; set; }  

        public Answer(int id, string title, int questionId, bool isTrue)
        {
            Id = id;
            Title = title;
            QuestionId = questionId;
            IsTrue = isTrue;
        }
    }
}
