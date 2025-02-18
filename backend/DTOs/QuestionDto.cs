namespace backend.DTOs
{
    public class QuestionDto
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int CreatedBy { get; set; }
    }

    public class CreateQuestionDto
    {
        public string Text { get; set; }
        public int CreatedBy { get; set; }
    }

    public class UpdateQuestionDto
    {
        public string Text { get; set; }
    }
}
