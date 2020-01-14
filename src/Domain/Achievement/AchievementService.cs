namespace SprintCrowd.BackEnd.Domain.Achievement
{
    using System.Threading.Tasks;
    using System;
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;

    public class AchievementService : IAchievementService
    {
        public AchievementService(IAchievementRepo repo)
        {
            this.AchievementRepo = repo;
        }

        private IAchievementRepo AchievementRepo { get; }

        public async Task SignUp(int userId)
        {
            var alreadySignUp = await this.AchievementRepo.GetByUserId(userId);
            if (alreadySignUp != null)
            {
                UserAchievement firstSignUp = new UserAchievement()
                {
                Type = AchievementType.JoinedTheCrowd,
                AchivedOn = DateTime.UtcNow,
                UserId = userId,
                Percentage = 100,
                };
                await this.AchievementRepo.Add(firstSignUp);
            }
        }
    }
}