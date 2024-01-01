﻿using Application.Common.Interfaces.Persistence;
using Application.Common.Interfaces.Services;
using Application.Common.Models;
using AutoMapper;
using Domain.Entities;

namespace Application.Services;

public class PassengerService : IPassengerService
{
    private readonly IPassengerReponsitory _reponsitory;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public PassengerService(IPassengerReponsitory reponsitory, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _reponsitory = reponsitory;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<PassengerDto>> GetAllPassengerDtoAsync()
    {
        var passengers = await _reponsitory.GetAllAsync();
        return _mapper.Map<List<PassengerDto>>(passengers);
    }

    public async Task<PassengerDto> GetPassgenerDtoByIdAsync(int id)
    {
        var passenger = await _reponsitory.GetByIdAsync(id);
        return _mapper.Map<PassengerDto>(passenger);
    }

    public async Task<Passenger> GetPassgenerByIdAsync(int id)
    {
        return await _reponsitory.GetByIdAsync(id);
    }

    public async Task AddPassengerAsync(Passenger passenger)
    {
        _reponsitory.Add(passenger);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdatePassengerAsync(Passenger passenger)
    {
        passenger.CreatedAt = _reponsitory.GetOldCreatedDate(passenger.Id);
        passenger.UpdatedAt = DateTime.Now;
        _reponsitory.Update(passenger);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeletePassengerAsync(Passenger passenger)
    {
        _reponsitory.Delete(passenger);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task SoftDeletePassengerAsync(Passenger passenger)
    {
        _reponsitory.SoftDelete(passenger);
        await _unitOfWork.SaveChangesAsync();
    }
}