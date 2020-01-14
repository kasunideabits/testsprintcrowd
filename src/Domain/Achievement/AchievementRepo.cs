using SprintCrowd.BackEnd.Infrastructure.Persistence;

namespace SprintCrowd.BackEnd.Domain.Achievement
{
    public class AchievementRepo : IAchievementRepo
    {
        public AchievementRepo(ScrowdDbContext context)
        {
            this.Context = context;
        }

        private ScrowdDbContext Context { get; }

        public void SaveChanges()
        {
            this.Context.SaveChanges();
        }
    }
}