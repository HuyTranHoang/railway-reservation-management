namespace Application.Common.Models;

public class TrainDetailDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public TrainCompanyDetailDto TrainCompany { get; set; }
    public List<CarriageDetailDto> Carriages { get; set; }
}

public class TrainCompanyDetailDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Logo { get; set; }
}

public class CarriageDetailDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public CarriageTypeDetailDto Type { get; set; }
    public List<CompartmentDetailDto> Compartments { get; set; }
}

public class CarriageTypeDetailDto
{
    public int Id { get; set; }
    public string Name { get; set; }
}

public class CompartmentDetailDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<SeatDtoDetail> Seats { get; set; }
}

public class SeatDtoDetail
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool Booked { get; set; }
}

