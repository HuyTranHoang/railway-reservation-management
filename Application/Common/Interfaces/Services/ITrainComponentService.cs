namespace Application.Common.Interfaces.Services
{
    public interface ITrainComponentService
    {
        Task AddTrainComponentsAsync(Carriage carriage);
        Task UpdateTrainComponentsAsync(Carriage carriage, int currentCarriageId);
    }
}