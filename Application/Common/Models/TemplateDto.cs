namespace Application.Common.Models
{
    public class TemplateDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int CarriageTypeId { get; set; }
        public string CarriageTypeName { get; set; }

        public int NumberOf { get; set; }

        public string Status { get; set; }
    }
}