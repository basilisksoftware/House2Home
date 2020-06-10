namespace API.Dtos
{
    public class ItemsStatisticsDto
    {
        public int ReceivedAll { get; set; }
        public int RejectedAll { get; set; }
        public int ReceivedWeek { get; set; }
        public int RejectedWeek { get; set; }
        public int ReceivedMonth { get; set; }
        public int RejectedMonth { get; set; }
        public int ReceivedYear { get; set; }
        public int RejectedYear { get; set; }
    }
}