using Application.Common.Exceptions;
using Application.Common.Interfaces.Persistence;
using Application.Common.Interfaces.Services;
using Application.Common.Models;
using Application.Common.Models.Pagination;
using Application.Common.Models.QueryParams;
using AutoMapper;
using Domain.Entities;


namespace Application.Services;

public class SeatTypeService : ISeatTypeService
{
    private readonly IMapper _mapper;
    private readonly ISeatTypeRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public SeatTypeService(ISeatTypeRepository repository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PagedList<SeatTypeDto>> GetAllSeatTypeDtoAsync(QueryParams queryParams)
    {
        var query = await _repository.GetQueryAsync();

        if (!string.IsNullOrEmpty(queryParams.SearchTerm))
            query = query.Where(st => st.Name.Contains(queryParams.SearchTerm));

        query = queryParams.Sort switch
        {
            "serviceChargeAsc" => query.OrderBy(st => st.ServiceCharge),
            "serviceChargeDesc" => query.OrderByDescending(st => st.ServiceCharge),
            _ => query.OrderBy(st => st.CreatedAt)
        };

        var seatTypeDtoQuery = query.Select(p => _mapper.Map<SeatTypeDto>(p));

        return await PagedList<SeatTypeDto>.CreateAsync(seatTypeDtoQuery, queryParams.PageNumber,
            queryParams.PageSize);
    }

    public async Task<SeatTypeDto> GetSeatTypeDtoByIdAsync(int id)
    {
        var seatType = await _repository.GetByIdAsync(id);
        return _mapper.Map<SeatTypeDto>(seatType);
    }

    public async Task<SeatType> GetSeatTypeByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task AddSeatTypeAsync(SeatType seatType)
    {
        _repository.Add(seatType);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateSeatTypeAsync(SeatType seatType)
    {
        var seatTypeInDb = await _repository.GetByIdAsync(seatType.Id);

        if (seatTypeInDb == null) throw new NotFoundException(nameof(Passenger), seatType.Id);

        seatTypeInDb.Name = seatType.Name;
        seatTypeInDb.ServiceCharge = seatType.ServiceCharge;
        seatTypeInDb.UpdatedAt = DateTime.Now;

        _repository.Update(seatTypeInDb);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteSeatTypeAsync(SeatType seatType)
    {
        _repository.Delete(seatType);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task SoftDeleteSeatTypeAsync(SeatType seatType)
    {
        _repository.SoftDelete(seatType);
        await _unitOfWork.SaveChangesAsync();
    }
}
