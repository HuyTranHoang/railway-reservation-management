

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
                    NumberOfSeats = 10,
                };

                await _compartmentRepository.Add(compartment);
            }
            

            await _unitOfWork.SaveChangesAsync();
        }

        
    }
}