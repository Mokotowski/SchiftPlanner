using SchiftPlanner.Models.Company;
using SchiftPlanner.Models.Company.Type_1;
using SchiftPlanner.Models.Company.Type_2;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Worker_Timetable
{
    [Key]
    public int Id_Timetable { get; set; }

    public int Id_Company { get; set; }

    public ushort Simultant { get; set; }
    public ushort Column { get; set; }



    public List<Day_Worker_Timetable> Worker_Days { get; set; }
    public Company_Type1 Company_Type1 { get; set; }

}