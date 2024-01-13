namespace Application.Common.Interfaces.Services;

public interface ITrainCompanyService : IService<TrainCompany>
{
    Task<PagedList<TrainCompanyDto>> GetAllDtoAsync(QueryParams queryParams);
    Task<List<TrainCompanyDto>> GetAllDtoNoPagingAsync();
    Task<TrainCompanyDto> GetDtoByIdAsync(int id);
}