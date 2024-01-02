using Application.Common.Interfaces.Persistence;
using Application.Common.Interfaces.Services;
using Application.Common.Models;
using AutoMapper;
using Domain.Entities;

namespace Application.Services;

public class TrainCompanyService : ITrainCompanyService
{
    private readonly IMapper _mapper;
    private readonly ITrainCompanyRepository _reponsitory;
    private readonly IUnitOfWork _unitOfWork;

    public TrainCompanyService(ITrainCompanyRepository repositoy, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _reponsitory = repositoy;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task AddCompanyAsync(TrainCompany trainCompany)
    {
        _reponsitory.Add(trainCompany);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteCompanyAsync(TrainCompany trainCompany)
    {
        _reponsitory.Delete(trainCompany);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<List<TrainCompanyDto>> GetAllCompanyDtoAsync()
    {
        var trainCompanies = await _reponsitory.GetAllAsync();
        return _mapper.Map<List<TrainCompanyDto>>(trainCompanies);
    }

    public async Task<TrainCompany> GetCompanyByIdAsync(int id)
    {
        return await _reponsitory.GetByIdAsync(id);
    }

    public async Task UpdateCompanyAsync(TrainCompany trainCompany)
    {
        _reponsitory.Update(trainCompany);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task SoftDeleteCompanyAsync(TrainCompany trainCompany)
    {
        _reponsitory.SoftDelete(trainCompany);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<TrainCompanyDto> GetCompanyDtoByIdAsync(int id)
    {
        var trainCompany = await _reponsitory.GetByIdAsync(id);
        return _mapper.Map<TrainCompanyDto>(trainCompany);
    }
}