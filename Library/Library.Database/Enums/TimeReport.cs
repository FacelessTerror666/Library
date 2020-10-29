using System.ComponentModel.DataAnnotations;

namespace Library.Database.Enums
{
    public enum TimeReport
    {
        [Display(Name = "За день")]
        Day = 0,

        [Display(Name = "За неделю")]
        Week = 10,

        [Display(Name = "За месяц")]
        Mounth = 20,

        [Display(Name = "За все время")]
        AllTime = 30
    }
}
