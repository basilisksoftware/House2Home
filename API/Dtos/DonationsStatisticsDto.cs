namespace API.Dtos
{
    public class DonationsStatisticsDto
    {
        public int ReceivedAll { get; set; }
        public int RejectedAll { get; set; }
        public int ReceivedWeek { get; set; }
        public int RejectedWeek { get; set; }
        public int ReceivedMonth { get; set; }
        public int RejectedMonth { get; set; }
        public int ReceivedYear { get; set; }
        public int RejectedYear { get; set; }
        public int AwaitingResponse { get; set; }
        public int AwaitingCollection { get; set; }
        public int CollectionArranged { get; set; }
    }
}