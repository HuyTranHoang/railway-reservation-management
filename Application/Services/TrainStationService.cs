
using Domain.Exceptions;

namespace Application.Services
{
    public class TrainStationService : ITrainStationService
    {
        private readonly IMapper _mapper;

        private readonly ITrainStationRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public TrainStationService(ITrainStationRepository repository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task AddAsync(TrainStation trainStation)
        {
            await _repository.Add(trainStation);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(TrainStation trainStation)
        {
            await _repository.Delete(trainStation);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<PagedList<TrainStationDto>> GetAllDtoAsync(QueryParams queryParams)
        {
            var query = await _repository.GetQueryAsync();

            if (!string.IsNullOrEmpty(queryParams.SearchTerm))
                query = query.Where(p => p.Name.Contains(queryParams.SearchTerm.Trim()) || p.Address.Contains(queryParams.SearchTerm.Trim()));

            query = queryParams.Sort switch
            {
                "nameAsc" => query.OrderBy(p => p.Name),
                "nameDesc" => query.OrderByDescending(p => p.Name),
                "addressAsc" => query.OrderBy(p => p.Address),
                "addressDesc" => query.OrderByDescending(p => p.Address),
                "coordinateValueAsc" => query.OrderBy(p => p.CoordinateValue),
                "coordinateValueDesc" => query.OrderByDescending(p => p.CoordinateValue),
                "statusAsc" => query.OrderBy(p => p.Status),
                "statusDesc" => query.OrderByDescending(p => p.Status),
                "createdAtDesc" => query.OrderByDescending(p => p.CreatedAt),
                _ => query.OrderBy(p => p.CreatedAt)
            };

            var trainStationDtoQuery = query.Select(p => _mapper.Map<TrainStationDto>(p));

            return await PagedList<TrainStationDto>.CreateAsync(trainStationDtoQuery, queryParams.PageNumber,
                queryParams.PageSize);
        }

        public async Task<TrainStation> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<TrainStationDto> GetDtoByIdAsync(int id)
        {
            var trainStation = await _repository.GetByIdAsync(id);
            return _mapper.Map<TrainStationDto>(trainStation);
        }

        public async Task SoftDeleteAsync(TrainStation trainStation)
        {
            await _repository.SoftDelete(trainStation);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(TrainStation trainStation)
        {
            var strainStationInDb = await _repository.GetByIdAsync(trainStation.Id);

            if (strainStationInDb == null) throw new NotFoundException(nameof(TrainStation), trainStation.Id);

            strainStationInDb.Name = trainStation.Name;
            strainStationInDb.Address = trainStation.Address;
            strainStationInDb.CoordinateValue = trainStation.CoordinateValue;
            strainStationInDb.Status = trainStation.Status;
            strainStationInDb.UpdatedAt = DateTime.Now;

            await _repository.Update(strainStationInDb);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}