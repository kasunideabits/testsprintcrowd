namespace SprintCrowd.BackEnd.Domain.Achievement
{
    using System.Threading.Tasks;
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;

    public interface IAchievementRepo
    {
        Task<UserAchievement> GetByUserId(int userId);
        Task<int> Add(UserAchievement achievement);
        void SaveChanges();
    }
}