namespace SprintCrowd.BackEnd.Domain.Achievement
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System;
    using SprintCrowd.BackEnd.Application;
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;

    public class AchievementService : IAchievementService
    {
        public AchievementService(IAchievementRepo repo)
        {
            this.AchievementRepo = repo;
        }

        private IAchievementRepo AchievementRepo { get; }
        private const int tenKM = 10000;
        private const int twentyKM = 20000;
        private const int thirtyKM = 30000;
        private const int fotyKM = 40000;
        private const int fiftyKM = 50000;
        private const int tenEvents = 10;
        private const int twentyEvents = 20;
        private const int thirtyEvents = 30;
        private const int fortyEvents = 40;
        private const int fiftyEvents = 50;

        public async Task SignUp(int userId)
        {
            var alreadySignUp = await this.AchievementRepo.GetByUserId(userId);
            if (alreadySignUp == null)
            {
                UserAchievement firstSignUp = new UserAchievement()
                {
                Type = Infrastructure.Persistence.Entities.AchievementType.JoinedTheCrowd,
                AchivedOn = DateTime.UtcNow,
                UserId = userId,
                Percentage = 100,
                };
                await this.AchievementRepo.Add(firstSignUp);
                this.AchievementRepo.SaveChanges();
            }
        }

        public async Task RaceCompleted(int userId, SprintType sprintType)
        {
            var allAchievements = this.AchievementRepo.GetAllUserAchivements(userId);
            this.FirstEventHandler(userId, sprintType);
            this.DistanceCompletedHandler(userId, sprintType, allAchievements);
            if (sprintType == SprintType.PrivateSprint)
            {
                this.PrivateEventCountHandler(userId, sprintType, allAchievements);
            }
            else
            {
                this.PublicEventCountHandler(userId, sprintType, allAchievements);
            }
            this.AchievementRepo.SaveChanges();
        }

        private void FirstEventHandler(int userId, SprintType sprintType)
        {
            var eventCount = this.AchievementRepo.GetEventCount(userId, sprintType);
            if (eventCount == 1)
            {
                var type = sprintType == SprintType.PrivateSprint ?
                    Infrastructure.Persistence.Entities.AchievementType.ThreeIsCrowd :
                    Infrastructure.Persistence.Entities.AchievementType.CrowdFunded;
                var firstAchievement = new UserAchievement()
                {
                    Type = type,
                    AchivedOn = DateTime.UtcNow,
                    UserId = userId,
                    Percentage = 100,
                };
                this.AchievementRepo.Add(firstAchievement);
            }
        }

        private void DistanceCompletedHandler(int userId, SprintType sprintType, Dictionary<Infrastructure.Persistence.Entities.AchievementType, UserAchievement> allAchievements)
        {
            var totalDistance = this.AchievementRepo.GetTotalDistance(userId, sprintType);
            if (totalDistance >= tenKM && !allAchievements.ContainsKey(Infrastructure.Persistence.Entities.AchievementType.TenComplete))
            {
                this.AchievementRepo.Add(BuildAchievement(userId, Infrastructure.Persistence.Entities.AchievementType.TenComplete));
            }

            if (totalDistance >= twentyKM && !allAchievements.ContainsKey(Infrastructure.Persistence.Entities.AchievementType.TwentyComplete))
            {
                this.AchievementRepo.Add(BuildAchievement(userId, Infrastructure.Persistence.Entities.AchievementType.TwentyComplete));
            }

            if (totalDistance >= thirtyKM && !allAchievements.ContainsKey(Infrastructure.Persistence.Entities.AchievementType.ThirtyComplete))
            {
                this.AchievementRepo.Add(BuildAchievement(userId, Infrastructure.Persistence.Entities.AchievementType.ThirtyComplete));
            }

            if (totalDistance >= fotyKM && !allAchievements.ContainsKey(Infrastructure.Persistence.Entities.AchievementType.FortyComplete))
            {
                this.AchievementRepo.Add(BuildAchievement(userId, Infrastructure.Persistence.Entities.AchievementType.FortyComplete));
            }

            if (totalDistance >= fiftyKM && !allAchievements.ContainsKey(Infrastructure.Persistence.Entities.AchievementType.FiftyComplete))
            {
                this.AchievementRepo.Add(BuildAchievement(userId, Infrastructure.Persistence.Entities.AchievementType.FiftyComplete));
            }
        }

        private void PrivateEventCountHandler(int userId, SprintType sprintType, Dictionary<Infrastructure.Persistence.Entities.AchievementType, UserAchievement> allAchievements)
        {
            var totalPrivateEvents = this.AchievementRepo.GetEventCount(userId, sprintType);
            if (totalPrivateEvents >= tenEvents && !allAchievements.ContainsKey(Infrastructure.Persistence.Entities.AchievementType.TenPrivateEventsComplete))
            {
                this.AchievementRepo.Add(BuildAchievement(userId, Infrastructure.Persistence.Entities.AchievementType.TenPrivateEventsComplete));
            }

            if (totalPrivateEvents >= twentyEvents && !allAchievements.ContainsKey(Infrastructure.Persistence.Entities.AchievementType.TwentyPrivateEventsComplete))
            {
                this.AchievementRepo.Add(BuildAchievement(userId, Infrastructure.Persistence.Entities.AchievementType.TwentyPrivateEventsComplete));
            }

            if (totalPrivateEvents >= thirtyEvents && !allAchievements.ContainsKey(Infrastructure.Persistence.Entities.AchievementType.ThirtyPrivateEventsComplete))
            {
                this.AchievementRepo.Add(BuildAchievement(userId, Infrastructure.Persistence.Entities.AchievementType.ThirtyPrivateEventsComplete));
            }

            if (totalPrivateEvents >= fortyEvents && !allAchievements.ContainsKey(Infrastructure.Persistence.Entities.AchievementType.FotyPrivateEventsComplete))
            {
                this.AchievementRepo.Add(BuildAchievement(userId, Infrastructure.Persistence.Entities.AchievementType.FotyPrivateEventsComplete));
            }

            if (totalPrivateEvents >= fiftyEvents && !allAchievements.ContainsKey(Infrastructure.Persistence.Entities.AchievementType.FiftyPrivateEventsComplete))
            {
                this.AchievementRepo.Add(BuildAchievement(userId, Infrastructure.Persistence.Entities.AchievementType.FiftyPrivateEventsComplete));
            }
        }

        private void PublicEventCountHandler(int userId, SprintType sprintType, Dictionary<Infrastructure.Persistence.Entities.AchievementType, UserAchievement> allAchievements)
        {
            var totalPrivateEvents = this.AchievementRepo.GetEventCount(userId, sprintType);
            if (totalPrivateEvents >= tenEvents && !allAchievements.ContainsKey(Infrastructure.Persistence.Entities.AchievementType.TenPublicEventsComplete))
            {
                this.AchievementRepo.Add(BuildAchievement(userId, Infrastructure.Persistence.Entities.AchievementType.TenPublicEventsComplete));
            }

            if (totalPrivateEvents >= twentyEvents && !allAchievements.ContainsKey(Infrastructure.Persistence.Entities.AchievementType.TwentyPublicEventsComplete))
            {
                this.AchievementRepo.Add(BuildAchievement(userId, Infrastructure.Persistence.Entities.AchievementType.TwentyPublicEventsComplete));
            }

            if (totalPrivateEvents >= thirtyEvents && !allAchievements.ContainsKey(Infrastructure.Persistence.Entities.AchievementType.ThirtyPublicEventsComplete))
            {
                this.AchievementRepo.Add(BuildAchievement(userId, Infrastructure.Persistence.Entities.AchievementType.ThirtyPublicEventsComplete));
            }

            if (totalPrivateEvents >= fortyEvents && !allAchievements.ContainsKey(Infrastructure.Persistence.Entities.AchievementType.FotyPublicEventsComplete))
            {
                this.AchievementRepo.Add(BuildAchievement(userId, Infrastructure.Persistence.Entities.AchievementType.FotyPublicEventsComplete));
            }

            if (totalPrivateEvents >= fiftyEvents && !allAchievements.ContainsKey(Infrastructure.Persistence.Entities.AchievementType.FiftyPublicEventsComplete))
            {
                this.AchievementRepo.Add(BuildAchievement(userId, Infrastructure.Persistence.Entities.AchievementType.FiftyPublicEventsComplete));
            }
        }

        private static UserAchievement BuildAchievement(int userId, Infrastructure.Persistence.Entities.AchievementType type)
        {
            return new UserAchievement()
            {
                Type = type,
                    AchivedOn = DateTime.UtcNow,
                    UserId = userId,
                    Percentage = 100,
            };
        }

    }
}