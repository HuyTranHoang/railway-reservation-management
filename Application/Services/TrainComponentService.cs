

namespace Application.Services
{
    public class TrainComponentService : ITrainComponentService
    {
        private readonly ICarriageRepository _carriageRepository;
        private readonly ICarriageTypeRepository _carriageTypeRepository;
        private readonly IMapper _mapper;
        private readonly ICompartmentRepository _compartmentRepository;
        private readonly ISeatRepository _seatRepository;
        private readonly IUnitOfWork _unitOfWork;
        public TrainComponentService(IUnitOfWork unitOfWork,
                                        IMapper mapper,
                                        ICarriageRepository carriageRepository,
                                        ICarriageTypeRepository carriageTypeRepository,
                                        ICompartmentRepository compartmentRepository,
                                        ISeatRepository seatRepository)
        {
            _carriageRepository = carriageRepository;
            _carriageTypeRepository = carriageTypeRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _compartmentRepository = compartmentRepository;
            _seatRepository = seatRepository;
        }
        public async Task AddTrainComponentsAsync(Carriage carriage, int trainId)
        {

            carriage.TrainId = trainId;
            await _carriageRepository.Add(carriage);
            await _unitOfWork.SaveChangesAsync();

            int carriageId = carriage.Id;
            
            var carriageType = await _carriageTypeRepository.GetByIdAsync(carriage.CarriageTypeId);
            

            for (int i = 0; i < carriageType.NumberOfCompartments; i++)
            {
                var compartment = new Compartment
                {
                    Name = $"Conpartment-{i+1}",
                    CarriageId = carriageId,
                    NumberOfSeats = 6, //Nghiệp vu mặc định NumberOfSeats dựa theo Compartment
                };

                await _compartmentRepository.Add(compartment);
                await _unitOfWork.SaveChangesAsync();


                //Loop thêm 1 lần nữa để add Seat dựa trên NumberOfSeats trong Compartment
                for (int j = 0; j < compartment.NumberOfSeats; j++)
                {
                    var seat = new Seat
                    {
                        Name = $"{j+1}",
                        SeatTypeId = 3, //Hiện tại set cứng, PHẢI LẬP LUẬN ĐỂ SET SỐ LƯỢNG GHẾ THEO SeatType
                        CompartmentId = compartment.Id,
                    };
                    await _seatRepository.Add(seat);
                }
            }
            
            await _unitOfWork.SaveChangesAsync();
        }

        
    }
}