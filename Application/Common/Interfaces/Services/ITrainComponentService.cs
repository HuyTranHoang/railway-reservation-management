namespace Application.Common.Interfaces.Services
{
    public interface ITrainComponentService
    {
        Task AddTrainComponentsAsync(Carriage carriage);
    }
}