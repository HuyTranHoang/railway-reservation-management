using Application.Common.Exceptions;
using Application.Common.Interfaces.Persistence;
using Application.Common.Interfaces.Services;
using Application.Common.Models;
using Application.Common.Models.Pagination;
using Application.Common.Models.QueryParams;
using AutoMapper;
using Domain.Entities;

namespace Application.Services;

public class SeatService : ISeatService
{
    private readonly ISeatRepository _repository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public SeatService(ISeatRepository repository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task AddAsync(Seat seat)
    {
        if (NameExists(_repository, seat.Id))
        {
            throw new BadRequestException(400, "Name already exists");
        }

        _repository.Add(seat);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteAsync(Seat seat)
    {
        _repository.Delete(seat);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<PagedList<SeatDto>> GetAllDtoAsync(SeatQueryParams queryParams)
    {
        var query = await _repository.GetQueryWithSeatTypeAndCompartmentAsync();
        
        if (queryParams.SeatTypeId != 0)
        {
            query = query.Where(t => t.SeatTypeId == queryParams.SeatTypeId);
        }
        
        if (!string.IsNullOrEmpty(queryParams.SearchTerm))
            query = query.Where(p => p.SeatType.Name.Contains(queryParams.SearchTerm));

        query = queryParams.Sort switch
        {
            "nameSeatTypeAsc" => query.OrderBy(p => p.SeatType.Name),
            "nameSeatTypeDesc" => query.OrderByDescending(p => p.SeatType.Name),
            "nameCompartmentAsc" => query.OrderBy(p => p.Compartment.Name),
            "nameCompartmentDesc" => query.OrderByDescending(p => p.Compartment.Name),
            _ => query.OrderBy(p => p.CreatedAt)
        };

        var seatDtoQuery = query.Select(p => _mapper.Map<SeatDto>(p));

        return await PagedList<SeatDto>.CreateAsync(seatDtoQuery, queryParams.PageNumber,
            queryParams.PageSize);
    }

    public async Task<Seat> GetByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<SeatDto> GetDtoByIdAsync(int id)
    {
        var seatDto = await _repository.GetByIdAsync(id);
        return _mapper.Map<SeatDto>(seatDto);
    }

    public async Task SoftDeleteAsync(Seat seat)
    {
        _repository.SoftDelete(seat);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateAsync(Seat seat)
    {
        var seatInDb = await _repository.GetByIdAsync(seat.Id);

        if (seatInDb == null) throw new NotFoundException(nameof(Passenger), seat.Id);

        if (NameExists(_repository, seat.Id))
        {
            throw new BadRequestException(400, "Name already exists");
        }

        seatInDb.SeatTypeId = seat.SeatTypeId;
        seatInDb.CompartmentId = seat.CompartmentId;
        seatInDb.UpdatedAt = DateTime.Now;

        await _unitOfWork.SaveChangesAsync();
    }

    private static bool NameExists(ISeatRepository repository, int seatId)
    {
        return repository.GetQueryAsync().Result.Any(t => t.Id == seatId);
    }
}
