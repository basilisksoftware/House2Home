

namespace API.Dtos
{
    public class StatisticsDto
    {
        public DonationsStatisticsDto Donations { get; set; }
        public ItemsStatisticsDto Items { get; set; }
    }
}