namespace LucidHR.Models
{
    public class CalendarViewModel
    {
        public int CurrentYear { get; set; }
        public int CurrentMonth { get; set; }
        public int MinYear { get; set; }
        public int MaxYear { get; set; }
        public List<CalendarDay> Days { get; set; }
        public List<LeaveDate> LeaveDates { get; set; }
        public List<LeaveDate> ApprovedLeaveDates { get; set; }
        public List<LeaveDate> PendingLeaveDates { get; set; }
        public List<LeaveDate> RejectedLeaveDates { get; set; }
    }

    public class CalendarDay
    {
        public int Day { get; set; }
        public bool IsWeekend { get; set; }
        public bool IsLeaveDay { get; set; }
        public bool IsApprovedLeaveDay { get; set; }
        public bool IsPendingLeaveDay { get; set; }
        public bool IsRejectedLeaveDay { get; set; }
    }

    public class LeaveDate
    {
        public DateTime LeaveFromDate { get; set; }
        public DateTime LeaveToDate { get; set; }
    }
}
