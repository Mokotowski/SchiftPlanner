using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace SchiftPlanner.Models.Survey
{
    public class Option_Answer
    {
        [Key]
        public string Question_Location { get; set; } // Id_Survey.numer_kórym jest pytaniem
        public string Question_Location_Id { get; set; } // Id_Survey.numer_kórym jest pytaniem.Którą jest odpowiedzią
        public string Text_Answer { get; set; }

        public Question Question { get; set; }

    }
}
