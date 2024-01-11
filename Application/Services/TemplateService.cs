
using AutoMapper;

namespace Application.Services
{
    public class TemplateService<T> : IService<T> where T : class
    {
        private readonly IMapper _mapper;
        private readonly IRepository<T> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public TemplateService(IRepository<T> repository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TemplateDto>> GetAllDtoAsync()
    {
        var entities = await _repository.GetQueryAsync();
        var dtos = _mapper.Map<IEnumerable<TemplateDto>>(entities);

        return dtos;
    }
        public async Task AddAsync(T t)
        {
            await _repository.AddAsync(t);
        }

        public async Task DeleteAsync(T t)
        {
            _repository.Delete(t);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task SoftDeleteAsync(T t)
        {
            _repository.SoftDelete(t);
            await _unitOfWork.SaveChangesAsync();
        }

        public Task UpdateAsync(T t)
        {
            throw new NotImplementedException();
        }
    }
}