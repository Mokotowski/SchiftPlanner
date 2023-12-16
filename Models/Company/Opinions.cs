using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchiftPlanner.Models.Company
{
    public class Opinions
    {
        [Key]
        public int Id { get; set; }
        public int Id_Company { get; set; }
        public bool Anonymously { get; set; }
        public string Id_user { get; set; }
        [Required]
        public int Score { get; set; }  
        public string Text { get; set; }

        public DateTime DateAdd { get; set; }

        public CompanyInfo CompanyInfo { get; set; }


        public static Opinions operator /(Opinions opinion1, Opinions opinion2)
        {
            opinion1.Anonymously = opinion2.Anonymously;
            opinion1.Score = opinion2.Score;
            opinion1.Text = opinion2.Text;
            opinion1.DateAdd = opinion2.DateAdd;

            return opinion1;
        }
    }
}
