using Application.Common.Exceptions;
using Application.Common.Interfaces.Persistence;
using Application.Common.Interfaces.Services;
using Application.Common.Models;
using Application.Common.Models.Pagination;
using Application.Common.Models.QueryParams;
using AutoMapper;
using Domain.Entities;

namespace Application.Services;

public class CarriageService : ICarriageService
{
    private readonly ICarriageReponsitory _repository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public CarriageService(ICarriageReponsitory repository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task AddCarriageAsync(Carriage carriage)
    {
        _repository.Add(carriage);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteCarriageAsync(Carriage carriage)
    {
        _repository.Delete(carriage);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<PagedList<CarriageDto>> GetAllCarriageDtoAsync(CarriageQueryParams queryParams)
    {
        var query = await _repository.GetQueryWithTrainAsync();

        if (queryParams.TrainId != 0)
        {
            query = query.Where(t => t.TrainId == queryParams.TrainId);
        }

        if (!string.IsNullOrEmpty(queryParams.SearchTerm))
            query = query.Where(p => p.Name.Contains(queryParams.SearchTerm));

        query = queryParams.Sort switch
        {
            "nameAsc" => query.OrderBy(p => p.Name),
            "nameDesc" => query.OrderByDescending(p => p.Name),
            "numberOfCompartmentAsc" => query.OrderBy(p => p.NumberOfCompartment),
            "numberOfCompartmentDesc" => query.OrderByDescending(p => p.NumberOfCompartment),
            _ => query.OrderBy(p => p.CreatedAt)
        };

        var carriageDtoQuery = query.Select(p => _mapper.Map<CarriageDto>(p));

        return await PagedList<CarriageDto>.CreateAsync(carriageDtoQuery, queryParams.PageNumber,
            queryParams.PageSize);
    }

    public async Task<Carriage> GetCarriageByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<CarriageDto> GetCarriageDtoByIdAsync(int id)
    {
        var carriageDto = await _repository.GetByIdAsync(id);
        return _mapper.Map<CarriageDto>(carriageDto);
    }

    public async Task SoftDeleteCarriageAsync(Carriage carriage)
    {
        _repository.SoftDelete(carriage);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateCarriageAsync(Carriage carriage)
    {
        var carriageInDb = await _repository.GetByIdAsync(carriage.Id);

        if (carriageInDb == null) throw new NotFoundException(nameof(Passenger), carriage.Id);

        carriageInDb.Name = carriage.Name;
        carriageInDb.TrainId = carriage.TrainId;
        carriageInDb.NumberOfCompartment = carriage.NumberOfCompartment;
        // carriageInDb.ServiceCharge = carriage.ServiceCharge;
        carriageInDb.Status = carriage.Status;
        carriageInDb.UpdatedAt = DateTime.Now;

        await _unitOfWork.SaveChangesAsync();
    }

}
