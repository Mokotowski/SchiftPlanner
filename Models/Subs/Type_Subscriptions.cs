using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchiftPlanner.Models.Subs
{
    public class Type_Subscriptions
    {
        [Key]
        public string Id_Sub { get; set; }

        [Column(TypeName = "decimal(6,2)")]
        public decimal Price { get; set; }

        public int DateLenght { get; set; }
        
        public ushort MaxPlann { get; set; }
        public ushort MaxPersonforPlann { get; set;}

        public ushort TypeCompany { get; set; }

        public List<Subscriptions> Subscriptions { get; set; }
    }
}
