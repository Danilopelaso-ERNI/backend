using backend.Entities;

namespace YourProjectName.Models
{
    public class Question
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int CreatedBy { get; set; } // UserId of the user who created the question

        // Navigation property
        public ICollection<Answer> Answers { get; set; }
    }
}