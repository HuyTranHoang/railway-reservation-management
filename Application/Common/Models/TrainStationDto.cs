namespace Application.Common.Models
{
    public class TrainStationDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public int CoordinateValue { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}