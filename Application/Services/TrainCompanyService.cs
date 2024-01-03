using Application.Common.Exceptions;
using Application.Common.Interfaces.Persistence;
using Application.Common.Interfaces.Services;
using Application.Common.Models;
using Application.Common.Models.Pagination;
using Application.Common.Models.QueryParams;
using AutoMapper;
using Domain.Entities;

namespace Application.Services;

public class TrainCompanyService : ITrainCompanyService
{
    private readonly IMapper _mapper;
    private readonly ITrainCompanyRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public TrainCompanyService(ITrainCompanyRepository repositoy, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _repository = repositoy;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PagedList<TrainCompanyDto>> GetAllCompanyDtoAsync(QueryParams queryParams)
    {
        var query = await _repository.GetQueryAsync();

        if (!string.IsNullOrEmpty(queryParams.SearchTerm))
        {
            query = query.Where(t => t.Name.Contains(queryParams.SearchTerm));
        }

        query = queryParams.Sort switch
        {
            "nameAsc" => query.OrderBy(p => p.Name),
            "nameDesc" => query.OrderByDescending(p => p.Name),
            _ => query.OrderBy(p => p.CreatedAt)
        };

        var trainCompaniesDtoQuery = query.Select(p => _mapper.Map<TrainCompanyDto>(p));

        return await PagedList<TrainCompanyDto>
            .CreateAsync(trainCompaniesDtoQuery, queryParams.PageNumber, queryParams.PageSize);
    }

    public async Task<TrainCompany> GetCompanyByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<TrainCompanyDto> GetCompanyDtoByIdAsync(int id)
    {
        var trainCompany = await _repository.GetByIdAsync(id);
        return _mapper.Map<TrainCompanyDto>(trainCompany);
    }

    public async Task AddCompanyAsync(TrainCompany trainCompany)
    {

        if (NameExists(_repository, trainCompany.Name))
        {
            throw new BadRequestException(400, "Name already exists");
        }

        _repository.Add(trainCompany);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateCompanyAsync(TrainCompany trainCompany)
    {
        var trainCompanyInDb = await _repository.GetByIdAsync(trainCompany.Id);

        if (trainCompanyInDb == null) throw new NotFoundException(nameof(Passenger), trainCompany.Id);

        trainCompanyInDb.Name = trainCompany.Name;
        trainCompanyInDb.UpdatedAt = DateTime.Now;

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteCompanyAsync(TrainCompany trainCompany)
    {
        _repository.Delete(trainCompany);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task SoftDeleteCompanyAsync(TrainCompany trainCompany)
    {
        _repository.SoftDelete(trainCompany);
        await _unitOfWork.SaveChangesAsync();
    }

    #region Private Helper Methods

    private static bool NameExists(ITrainCompanyRepository repository, string name)
    {
        return repository.GetQueryAsync().Result.Any(t => t.Name == name);
    }

    #endregion
}