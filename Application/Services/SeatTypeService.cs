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
    private readonly ISeatTypeRepository _repositoy;
    private readonly IUnitOfWork _unitOfWork;

    public SeatTypeService(ISeatTypeRepository repositoy, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _repositoy = repositoy;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task AddAsync(SeatType seatType)
    {
        _repositoy.Add(seatType);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteAsync(SeatType seatType)
    {
        _repositoy.Remove(seatType);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateAsync(SeatType seatType)
    {
        var seatTypeExis = await _repositoy.GetByIdAsync(seatType.Id);

        if (seatTypeExis == null) throw new NotFoundException(nameof(SeatType), seatType.Id);

        seatTypeExis.Name = seatType.Name;
        seatTypeExis.ServiceCharge = seatType.ServiceCharge;
        seatTypeExis.Status = seatType.Status;
        seatTypeExis.UpdatedAt = DateTime.Now;

        _repositoy.Update(seatTypeExis);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<SeatType> GetByIdAsync(int id)
    {
        return await _repositoy.GetByIdAsync(id);
    }

    public async Task<SeatTypeDto> GetByIdDtoAsync(int id)
    {
        return await _repositoy.GetByIdDtoAsync(id);
    }

    async Task<PagedList<SeatTypeDto>> ISeatTypeService.GetAllDtoAsync(QueryParams queryParams)
    {
        var query = await _repositoy.GetQueryAsync();

        if (!string.IsNullOrEmpty(queryParams.SearchTerm))
            query = query.Where(p => p.Name.Contains(queryParams.SearchTerm));

        query = queryParams.Sort switch
        {
            "serviceAsc" => query.OrderBy(p => p.ServiceCharge),
            "serviceDesc" => query.OrderByDescending(p => p.ServiceCharge),
            "nameAsc" => query.OrderBy(p => p.Name),
            "nameDesc" => query.OrderByDescending(p => p.Name),
            _ => query.OrderBy(p => p.CreatedAt)
        };

        var seatTypeDtos = query.Select(p => _mapper.Map<SeatTypeDto>(p));

        return await PagedList<SeatTypeDto>.CreateAsync(seatTypeDtos, queryParams.PageNumber, queryParams.PageSize);
    }
}
