namespace SprintCrowd.BackEnd.Domain.Achievement
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using SprintCrowd.BackEnd.Application;
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;

    public interface IAchievementRepo
    {
        Task<UserAchievement> GetByUserId(int userId);
        Dictionary<Infrastructure.Persistence.Entities.AchievementType, UserAchievement> GetAllUserAchivements(int userId);
        Task<UserAchievement> Add(UserAchievement achievement);
        int GetEventCount(int userId, SprintType sprintType);
        int GetTotalDistance(int userId, SprintType sprintType);
        void SaveChanges();
    }
}