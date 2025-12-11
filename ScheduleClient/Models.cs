namespace ScheduleClient
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Lesson
    {
        public int Id { get; set; }
        public int GroupId { get; set; }
        public int DayOfWeek { get; set; }
        public bool IsNumerator { get; set; }
        public string Time { get; set; }
        public string Subject { get; set; }
        public string Teacher { get; set; }
        public string Room { get; set; }
    }
}