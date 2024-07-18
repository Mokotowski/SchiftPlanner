using System.ComponentModel.DataAnnotations;

namespace SchiftPlanner.Models.Company.Type_1
{
    public class Workers
    {
        [Key]
        public string Work_Group_User { get; set; } // Id_Work_Group.Id_user
        public string Id_user { get; set; }
        public int Id_Work_Group { get; set; }
        public bool Accepted { get; set; }


        public int Company_Type1Id { get; set; }
        public Company_Type1 Company_Type1 { get; set;}
    }
}
