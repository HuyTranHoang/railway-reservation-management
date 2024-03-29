using Domain.Exceptions;

namespace Application.Services
{
    public class TrainComponentService : ITrainComponentService
    {
        private readonly ICarriageRepository _carriageRepository;
        private readonly ICarriageTypeRepository _carriageTypeRepository;
        private readonly IMapper _mapper;
        private readonly ICompartmentRepository _compartmentRepository;
        private readonly ISeatRepository _seatRepository;
        private readonly ISeatTypeRepository _seatTypeRepository;
        private readonly IUnitOfWork _unitOfWork;

        public TrainComponentService(IUnitOfWork unitOfWork,
            IMapper mapper,
            ICarriageRepository carriageRepository,
            ICarriageTypeRepository carriageTypeRepository,
            ICompartmentRepository compartmentRepository,
            ISeatRepository seatRepository,
            ISeatTypeRepository seatTypeRepository)
        {
            _carriageRepository = carriageRepository;
            _carriageTypeRepository = carriageTypeRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _compartmentRepository = compartmentRepository;
            _seatRepository = seatRepository;
            _seatTypeRepository = seatTypeRepository;
        }

        public async Task AddTrainComponentsAsync(Carriage carriage)
        {
            await _carriageRepository.Add(carriage);
            await _unitOfWork.SaveChangesAsync();

            int carriageId = carriage.Id;

            var carriageType = await _carriageTypeRepository.GetByIdAsync(carriage.CarriageTypeId);

            var seatCounter = 1; // Đặt seatCounter ở đây để giữ giá trị khi chuyển compartment

            for (int i = 0; i < carriageType.NumberOfCompartments; i++)
            {
                var compartment = new Compartment
                {
                    Name = $"Conpartment-{i + 1}",
                    CarriageId = carriageId
                };

                await _compartmentRepository.Add(compartment);
                await _unitOfWork.SaveChangesAsync();

                var numberOfSeats = carriageType.NumberOfSeats / carriageType.NumberOfCompartments;
                var numberOfSeatTypes = carriageType.NumberOfSeatTypes;

                if (numberOfSeatTypes == 1)
                {
                    var seatType = await _seatTypeRepository.GetSeatTypeByNameAsync("C");
                    var seatTypeId = seatType.Id;
                    for (int j = 0; j < numberOfSeats; j++)
                    {
                        var seat = new Seat
                        {
                            Name = $"{seatCounter}",
                            SeatTypeId = seatTypeId,
                            CompartmentId = compartment.Id,
                        };
                        await _seatRepository.Add(seat);
                        seatCounter++;
                    }
                }
                else if (numberOfSeatTypes == 2)
                {
                    var seatTypeB = await _seatTypeRepository.GetSeatTypeByNameAsync("B");
                    var seatTypeC = await _seatTypeRepository.GetSeatTypeByNameAsync("C");
                    var seatTypeBId = seatTypeB.Id;
                    var seatTypeCId = seatTypeC.Id;
                    for (int j = 0; j < numberOfSeats; j++)
                    {
                        var seat = new Seat
                        {
                            Name = $"{seatCounter}",
                            SeatTypeId = (j % 2 == 0) ? seatTypeBId : seatTypeCId,
                            CompartmentId = compartment.Id,
                        };
                        await _seatRepository.Add(seat);
                        seatCounter++;
                    }
                }
                else if (numberOfSeatTypes == 3)
                {
                    var seatTypeA = await _seatTypeRepository.GetSeatTypeByNameAsync("A");
                    var seatTypeB = await _seatTypeRepository.GetSeatTypeByNameAsync("B");
                    var seatTypeC = await _seatTypeRepository.GetSeatTypeByNameAsync("C");
                    var seatTypeAId = seatTypeA.Id;
                    var seatTypeBId = seatTypeB.Id;
                    var seatTypeCId = seatTypeC.Id;

                    for (int j = 0; j < numberOfSeats; j++)
                    {
                        var seat = new Seat
                        {
                            Name = $"{seatCounter}",
                            SeatTypeId = j % 3 == 0 ? seatTypeAId : (j % 3 == 1 ? seatTypeBId : seatTypeCId),
                            CompartmentId = compartment.Id,
                        };
                        await _seatRepository.Add(seat);
                        seatCounter++;
                    }
                }
            }

            await _unitOfWork.SaveChangesAsync();
        }

        /// //////////////
        public async Task UpdateTrainComponentsAsync(Carriage carriage, int currentCarriageId)
        {
            var currentCarriage = await _carriageRepository.GetByIdAsync(currentCarriageId);

            if (currentCarriage is null)
            {
                throw new NotFoundException("Carriage not found");
            }

            // await _carriageRepository.SoftDelete(currentCarriage);

            if (currentCarriage.CarriageTypeId != carriage.CarriageTypeId)
            {
                //SoftDelete thông tin carriage cũ
                var currentCompartments = await _compartmentRepository
                    .GetCompartmentsByCarriageIdAsync(currentCarriageId);

                foreach (var compartment in currentCompartments)
                {
                    await _compartmentRepository.SoftDelete(compartment);

                    var currentSeats = await _seatRepository.GetSeatsByCompartmentIdAsync(compartment.Id);
                    foreach (var seat in currentSeats)
                    {
                        await _seatRepository.SoftDelete(seat);
                    }
                }

                // Thêm thông tin carriage mới
                var carriageType = await _carriageTypeRepository.GetByIdAsync(carriage.CarriageTypeId);

                var seatCounter = 1; // Đặt seatCounter ở đây để giữ giá trị khi chuyển compartment

                for (int i = 0; i < carriageType.NumberOfCompartments; i++)
                {
                    var compartment = new Compartment
                    {
                        Name = $"Conpartment-{i + 1}",
                        CarriageId = currentCarriage.Id
                    };

                    await _compartmentRepository.Add(compartment);
                    await _unitOfWork.SaveChangesAsync();

                    var numberOfSeats = carriageType.NumberOfSeats / carriageType.NumberOfCompartments;
                    var numberOfSeatTypes = carriageType.NumberOfSeatTypes;

                    if (numberOfSeatTypes == 1)
                    {
                        var seatType = await _seatTypeRepository.GetSeatTypeByNameAsync("C");
                        var seatTypeId = seatType.Id;
                        for (int j = 0; j < numberOfSeats; j++)
                        {
                            var seat = new Seat
                            {
                                Name = $"{seatCounter}",
                                SeatTypeId = seatTypeId,
                                CompartmentId = compartment.Id,
                            };
                            await _seatRepository.Add(seat);
                            seatCounter++;
                        }
                    }
                    else if (numberOfSeatTypes == 2)
                    {
                        var seatTypeB = await _seatTypeRepository.GetSeatTypeByNameAsync("B");
                        var seatTypeC = await _seatTypeRepository.GetSeatTypeByNameAsync("C");
                        var seatTypeBId = seatTypeB.Id;
                        var seatTypeCId = seatTypeC.Id;
                        for (int j = 0; j < numberOfSeats; j++)
                        {
                            var seat = new Seat
                            {
                                Name = $"{seatCounter}",
                                SeatTypeId = (j % 2 == 0) ? seatTypeBId : seatTypeCId,
                                CompartmentId = compartment.Id,
                            };
                            await _seatRepository.Add(seat);
                            seatCounter++;
                        }
                    }
                    else if (numberOfSeatTypes == 3)
                    {
                        var seatTypeA = await _seatTypeRepository.GetSeatTypeByNameAsync("A");
                        var seatTypeB = await _seatTypeRepository.GetSeatTypeByNameAsync("B");
                        var seatTypeC = await _seatTypeRepository.GetSeatTypeByNameAsync("C");
                        var seatTypeAId = seatTypeA.Id;
                        var seatTypeBId = seatTypeB.Id;
                        var seatTypeCId = seatTypeC.Id;

                        for (int j = 0; j < numberOfSeats; j++)
                        {
                            var seat = new Seat
                            {
                                Name = $"{seatCounter}",
                                SeatTypeId = j % 3 == 0 ? seatTypeAId : (j % 3 == 1 ? seatTypeBId : seatTypeCId),
                                CompartmentId = compartment.Id,
                            };
                            await _seatRepository.Add(seat);
                            seatCounter++;
                        }
                    }
                }
            }

            //Cập nhật thông tin carriage mới
            currentCarriage.CarriageTypeId = carriage.CarriageTypeId;
            currentCarriage.Name = carriage.Name;
            currentCarriage.Status = carriage.Status;
            currentCarriage.TrainId = carriage.TrainId;
            currentCarriage.UpdatedAt = DateTime.Now;
            await _carriageRepository.Update(currentCarriage);

            await _unitOfWork.SaveChangesAsync();
        }
    }
}