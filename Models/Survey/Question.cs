using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace SchiftPlanner.Models.Survey
{
    public class Question
    {
        [Key]
        public string Question_Location { get; set; } // Id_Survey.numer_kórym jest pytaniem
        public int Id_Survey { get; set; }
        public bool IsOpen { get; set; }
        public string Question_Text { get; set; }
        public List<Option_Answer>? Options_Answer { get; set; }



        public SurveysProperties SurveysProperties { get; set; }


    }
}
