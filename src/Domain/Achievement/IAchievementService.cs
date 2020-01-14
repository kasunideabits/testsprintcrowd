namespace SprintCrowd.BackEnd.Domain.Achievement
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using SprintCrowd.BackEnd.Application;
    using SprintCrowd.Domain.Achievement;

    public interface IAchievementService
    {
        Task SignUp(int userId);
        Task<List<AchievementDto>> RaceCompleted(int userId, SprintType sprintType);
    }
}