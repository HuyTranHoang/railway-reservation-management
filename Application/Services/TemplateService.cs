
namespace Application.Services
{
    public class TemplateService<T> : IService<T> where T : class
    {
        private readonly IMapper _mapper;
        private readonly IRepository<T> _repository;

        public TemplateService(IRepository<T> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<CarriageTemplateDto>> GetAllCarriageTemplateDtoAsync()
        {
            var entities = await _repository.GetQueryAsync();
            var dtos = _mapper.Map<List<CarriageTemplateDto>>(entities);

            return dtos;
        }
        public async Task<List<CompartmentTemplateDto>> GetAllCompartmentTemplateDtoAsync()
        {
            var entities = await _repository.GetQueryAsync();
            var dtos = _mapper.Map<List<CompartmentTemplateDto>>(entities);

            return dtos;
        }

        public Task AddAsync(T t)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(T t)
        {
            throw new NotImplementedException();
        }

        public Task<T> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task SoftDeleteAsync(T t)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(T t)
        {
            throw new NotImplementedException();
        }
    }
}