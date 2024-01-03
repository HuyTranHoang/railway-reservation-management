using Application.Common.Exceptions;
using Application.Common.Interfaces.Persistence;
using Application.Common.Interfaces.Services;
using Application.Common.Models;
using Application.Common.Models.Pagination;
using Application.Common.Models.QueryParams;
using AutoMapper;
using Domain.Entities;

namespace Application.Services;

public class TrainService : ITrainService
{
    private readonly IMapper _mapper;

    private readonly ITrainRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public TrainService(ITrainRepository repositoy, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _repository = repositoy;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task AddTrainAsync(Train train)
    {
        if (NameExists(_repository, train.Name))
        {
            throw new BadRequestException(400, "Name already exists");
        }

        _repository.Add(train);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteTrainAsync(Train train)
    {
        _repository.Delete(train);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<PagedList<TrainDto>> GetAllTrainDtoAsync(TrainQueryParams queryParams)
    {
        var query = await _repository.GetQueryWithTrainCompanyAsync();

        if (queryParams.TrainCompanyId != 0)
        {
            query = query.Where(t => t.TrainCompanyId == queryParams.TrainCompanyId);
        }

        if (!string.IsNullOrEmpty(queryParams.SearchTerm))
            query = query.Where(t => t.Name.Contains(queryParams.SearchTerm));

        query = queryParams.Sort switch
        {
            "nameAsc" => query.OrderBy(t => t.Name),
            "nameDesc" => query.OrderByDescending(t => t.Name),
            _ => query.OrderBy(t => t.CreatedAt)
        };


        var trainsDtoQuery = query.Select(t => _mapper.Map<TrainDto>(t));

        return await PagedList<TrainDto>.CreateAsync(trainsDtoQuery, queryParams.PageNumber,
            queryParams.PageSize);
    }

    public async Task<Train> GetTrainByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<TrainDto> GetTrainDtoByIdAsync(int id)
    {
        var trainDto = await _repository.GetByIdAsync(id);
        return _mapper.Map<TrainDto>(trainDto);
    }

    public async Task SoftDeleteTrainAsync(Train train)
    {
        _repository.SoftDelete(train);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateTrainAsync(Train train)
    {
        var trainInDb = await _repository.GetByIdAsync(train.Id);

        if (trainInDb == null) throw new NotFoundException(nameof(Train), train.Id);

        trainInDb.Name = train.Name;
        trainInDb.TrainCompanyId = train.TrainCompanyId;
        trainInDb.UpdatedAt = DateTime.Now;

        await _unitOfWork.SaveChangesAsync();
    }

    #region Private Helper Methods

    private static bool NameExists(ITrainRepository repository, string name)
    {
        return repository.GetQueryAsync().Result.Any(t => t.Name == name);
    }

    #endregion
}