using System;

namespace ExamSys.Models
{
    public class Subject
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public Subject(int id, string title)
        {
            Id = id;
            Title = title;
        }
    }
}
