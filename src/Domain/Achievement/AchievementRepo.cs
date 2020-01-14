using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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

        public async Task<int> Add(UserAchievement achievement)
        {
            var result = await this.Context.UserAchivements.AddAsync(achievement);
            return result.Entity.Id;
        }

        public void SaveChanges()
        {
            this.Context.SaveChanges();
        }
    }
}