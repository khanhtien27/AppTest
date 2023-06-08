namespace AppTest.Models.ModelDTO
{
    public class TimeSheetMonth
    {
        public int Month { get; set; }
        public List<TimeSheet>? timeSheets { get; set; }
    }
}
