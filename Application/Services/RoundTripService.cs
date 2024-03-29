using Domain.Exceptions;

namespace Application.Services
{
    public class RoundTripService : IRoundTripService
    {
        private readonly IMapper _mapper;

        private readonly IRoundTripRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public RoundTripService(IRoundTripRepository repositoy, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _repository = repositoy;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task AddAsync(RoundTrip roundTrip)
        {
            if (Exists(_repository, roundTrip.TrainCompanyId))
            {
                throw new BadRequestException(400, "Train company discounts already exist");
            }

            await _repository.Add(roundTrip);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(RoundTrip roundTrip)
        {
            await _repository.Delete(roundTrip);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<PagedList<RoundTripDto>> GetAllDtoAsync(RoundTripParams queryParams)
        {
            var query = await _repository.GetQueryWithTrainCompanyAsync();

            if (queryParams.TrainCompanyId != 0)
            {
                query = query.Where(t => t.TrainCompanyId == queryParams.TrainCompanyId);
            }

            if (!string.IsNullOrEmpty(queryParams.SearchTerm))
                query = query.Where(t => t.TrainCompany.Name.Contains(queryParams.SearchTerm.Trim()));


            query = queryParams.Sort switch
            {
                "trainCompanyNameAsc" => query.OrderBy(t => t.TrainCompany.Name),
                "trainCompanyNameDesc" => query.OrderByDescending(t => t.TrainCompany.Name),
                "discountAsc" => query.OrderBy(t => t.Discount),
                "discountDesc" => query.OrderByDescending(t => t.Discount),
                "createdAtDesc" => query.OrderByDescending(t => t.CreatedAt),
                _ => query.OrderBy(t => t.CreatedAt)
            };


            var roundTripDtoQuery = query.Select(t => _mapper.Map<RoundTripDto>(t));

            return await PagedList<RoundTripDto>.CreateAsync(roundTripDtoQuery, queryParams.PageNumber,
                queryParams.PageSize);
        }

        public async Task<RoundTrip> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<RoundTripDto> GetDtoByIdAsync(int id)
        {
            var roundTrip = await _repository.GetByIdWithTrainCompanyAsync(id);
            return _mapper.Map<RoundTripDto>(roundTrip);
        }

        public async Task<RoundTripDto> GetDtoByTrainCompanyIdAsync(int trainCompanyId)
        {
            var roundTrip = await _repository.GetByTrainCompanyIdAsync(trainCompanyId);
            return _mapper.Map<RoundTripDto>(roundTrip);
        }

        public async Task SoftDeleteAsync(RoundTrip roundTrip)
        {
            await _repository.SoftDelete(roundTrip);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(RoundTrip roundTrip)
        {
            var roundTripInDb = await _repository.GetByIdAsync(roundTrip.Id);

            if (roundTripInDb == null) throw new NotFoundException(nameof(RoundTrip), roundTrip.Id);

            roundTripInDb.TrainCompanyId = roundTrip.TrainCompanyId;
            roundTripInDb.Discount = roundTrip.Discount;
            roundTripInDb.UpdatedAt = DateTime.Now;

            await _repository.Update(roundTripInDb);
            await _unitOfWork.SaveChangesAsync();
        }

        #region Private Helper Methods

        private static bool Exists(IRoundTripRepository repository, int trainCompanyId)
        {
            return repository.GetQueryAsync().Result.Any(t => t.TrainCompanyId == trainCompanyId);
        }


        #endregion
    }
}