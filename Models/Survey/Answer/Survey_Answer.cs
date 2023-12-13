using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace SchiftPlanner.Models.Survey.Answer
{
    public class Survey_Answer
    {
        public string Id_Survey { get; set; }
        public string Id_User { get; set; }

        [Key]
        public string Survey_User { get; set; } // Id_Survey.Id_User dla anonimowych id_suer generowane


        public List<Question_Answer> Question_Answers { get; set; }



    }
}
