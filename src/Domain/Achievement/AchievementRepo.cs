using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SprintCrowd.BackEnd.Application;
using SprintCrowd.BackEnd.Infrastructure.Persistence;
using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;

namespace SprintCrowd.BackEnd.Domain.Achievement
{
    public class AchievementRepo : IAchievementRepo
    {
        public AchievementRepo(ScrowdDbContext context)
        {
            this.Context = context;
        }

        private ScrowdDbContext Context { get; }

        public async Task<UserAchievement> GetByUserId(int userId)
        {
            return await this.Context.UserAchivements.FirstOrDefaultAsync(u => u.UserId == userId);
        }

        public async Task<UserAchievement> Add(UserAchievement achievement)
        {
            var result = await this.Context.UserAchivements.AddAsync(achievement);
            return result.Entity;
        }

        public void SaveChanges()
        {
            this.Context.SaveChanges();
        }

        public Dictionary<Infrastructure.Persistence.Entities.AchievementType, UserAchievement> GetAllUserAchivements(int userId)
        {
            var result = this.Context.UserAchivements.Where(u => u.UserId == userId).ToDictionary(u => u.Type);
            return result;
        }

        public int GetEventCount(int userId, SprintType sprintType)
        {
            var result = this.Context.SprintParticipant.Count(s => s.Sprint.Type == (int)sprintType && s.UserId == userId && s.Stage == ParticipantStage.COMPLETED);
            return result;
        }

        public int GetTotalDistance(int userId, SprintType sprintType)
        {
            var result = this.Context.SprintParticipant
                .Where(s => s.Sprint.Type == (int)sprintType && s.UserId == userId && s.Stage == ParticipantStage.COMPLETED)
                .Sum(s => s.Sprint.Distance);
            return result;
        }
    }
}