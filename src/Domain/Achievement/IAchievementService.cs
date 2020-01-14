namespace SprintCrowd.BackEnd.Domain.Achievement
{
    using System.Threading.Tasks;
    using SprintCrowd.BackEnd.Application;

    public interface IAchievementService
    {
        Task SignUp(int userId);
        Task RaceCompleted(int userId, SprintType sprintType);
    }
}