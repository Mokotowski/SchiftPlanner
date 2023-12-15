using System.ComponentModel.DataAnnotations;

namespace SchiftPlanner.Models.Survey.Answer
{
    public class Question_Answer
    {
        public string Survey_User { get; set; }
        [Key]
        public string Survey_User_Question_Id { get; set; } // Survey_User.Question_Id
        public int Question_Id { get; set; }
        public bool IsOpen { get; set; }
        public string? Text_Answer { get; set; }
        public ushort? Number_Option_Answer { get; set; }

        public Survey_Answer Survey_Answer { get; set; }
    }
}
