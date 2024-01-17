namespace Application.Common.Models
{
    public class CompartmentDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int CarriageId { get; set; }
        public string CarriageName { get; set; }
        public string TrainName { get; set; }

        public int NumberOfSeats { get; set; }

        public string Status { get; set; }

        public DateTime CreatedAt { get; set; }

    }
}