namespace SprintCrowd.BackEnd.Domain.Achievement
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System;
    using SprintCrowd.BackEnd.Application;
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;
    using SprintCrowd.Domain.Achievement;

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

        public async Task<List<AchievementDto>> RaceCompleted(int userId, SprintType sprintType)
        {
            var achivements = new List<AchievementDto>();
            var allAchievements = this.AchievementRepo.GetAllUserAchivements(userId);
            var firstEvent = await this.FirstEventHandler(userId, sprintType, allAchievements);
            if (firstEvent != null)
            {
                achivements.Add(firstEvent);
            }
            var distanceCompleted = await this.DistanceCompletedHandler(userId, sprintType, allAchievements);
            achivements.AddRange(distanceCompleted);
            if (sprintType == SprintType.PrivateSprint)
            {
                var privateEventCompleted = await this.PrivateEventCountHandler(userId, sprintType, allAchievements);
                achivements.AddRange(privateEventCompleted);
            }
            else
            {
                var publicEventCompleted = await this.PublicEventCountHandler(userId, sprintType, allAchievements);
                achivements.AddRange(publicEventCompleted);
            }
            this.AchievementRepo.SaveChanges();
            return achivements;
        }

        private async Task<AchievementDto> FirstEventHandler(int userId, SprintType sprintType, Dictionary<Infrastructure.Persistence.Entities.AchievementType, UserAchievement> allAchievements)
        {
            var eventCount = this.AchievementRepo.GetEventCount(userId, sprintType);
            var isAlreadExist = sprintType == SprintType.PrivateSprint ?
                !allAchievements.ContainsKey(Infrastructure.Persistence.Entities.AchievementType.ThreeIsCrowd) :
                !allAchievements.ContainsKey(Infrastructure.Persistence.Entities.AchievementType.CrowdFunded);
            if (eventCount == 1 || isAlreadExist)
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
                var achievement = await this.AchievementRepo.Add(firstAchievement);
                return new AchievementDto(achievement);
            }
            return null;
        }

        private async Task<List<AchievementDto>> DistanceCompletedHandler(
            int userId,
            SprintType sprintType,
            Dictionary<Infrastructure.Persistence.Entities.AchievementType, UserAchievement> allAchievements)
        {
            var achivements = new List<AchievementDto>();
            var totalDistance = this.AchievementRepo.GetTotalDistance(userId, sprintType);
            if (totalDistance >= tenKM && !allAchievements.ContainsKey(Infrastructure.Persistence.Entities.AchievementType.TenComplete))
            {
                var entity = await this.AchievementRepo.Add(BuildAchievement(userId, Infrastructure.Persistence.Entities.AchievementType.TenComplete));
                achivements.Add(new AchievementDto(entity));
            }

            if (totalDistance >= twentyKM && !allAchievements.ContainsKey(Infrastructure.Persistence.Entities.AchievementType.TwentyComplete))
            {
                var entity = await this.AchievementRepo.Add(BuildAchievement(userId, Infrastructure.Persistence.Entities.AchievementType.TwentyComplete));
                achivements.Add(new AchievementDto(entity));
            }

            if (totalDistance >= thirtyKM && !allAchievements.ContainsKey(Infrastructure.Persistence.Entities.AchievementType.ThirtyComplete))
            {
                var entity = await this.AchievementRepo.Add(BuildAchievement(userId, Infrastructure.Persistence.Entities.AchievementType.ThirtyComplete));
                achivements.Add(new AchievementDto(entity));
            }

            if (totalDistance >= fotyKM && !allAchievements.ContainsKey(Infrastructure.Persistence.Entities.AchievementType.FortyComplete))
            {
                var entity = await this.AchievementRepo.Add(BuildAchievement(userId, Infrastructure.Persistence.Entities.AchievementType.FortyComplete));
                achivements.Add(new AchievementDto(entity));
            }

            if (totalDistance >= fiftyKM && !allAchievements.ContainsKey(Infrastructure.Persistence.Entities.AchievementType.FiftyComplete))
            {
                var entity = await this.AchievementRepo.Add(BuildAchievement(userId, Infrastructure.Persistence.Entities.AchievementType.FiftyComplete));
                achivements.Add(new AchievementDto(entity));
            }
            return achivements;
        }

        private async Task<List<AchievementDto>> PrivateEventCountHandler(
            int userId, SprintType sprintType, Dictionary<Infrastructure.Persistence.Entities.AchievementType, UserAchievement> allAchievements)
        {
            var achivements = new List<AchievementDto>();
            var totalPrivateEvents = this.AchievementRepo.GetEventCount(userId, sprintType);
            if (totalPrivateEvents >= tenEvents && !allAchievements.ContainsKey(Infrastructure.Persistence.Entities.AchievementType.TenPrivateEventsComplete))
            {
                var entity = await this.AchievementRepo.Add(BuildAchievement(userId, Infrastructure.Persistence.Entities.AchievementType.TenPrivateEventsComplete));
                achivements.Add(new AchievementDto(entity));
            }

            if (totalPrivateEvents >= twentyEvents && !allAchievements.ContainsKey(Infrastructure.Persistence.Entities.AchievementType.TwentyPrivateEventsComplete))
            {
                var entity = await this.AchievementRepo.Add(BuildAchievement(userId, Infrastructure.Persistence.Entities.AchievementType.TwentyPrivateEventsComplete));
                achivements.Add(new AchievementDto(entity));
            }

            if (totalPrivateEvents >= thirtyEvents && !allAchievements.ContainsKey(Infrastructure.Persistence.Entities.AchievementType.ThirtyPrivateEventsComplete))
            {
                var entity = await this.AchievementRepo.Add(BuildAchievement(userId, Infrastructure.Persistence.Entities.AchievementType.ThirtyPrivateEventsComplete));
                achivements.Add(new AchievementDto(entity));
            }

            if (totalPrivateEvents >= fortyEvents && !allAchievements.ContainsKey(Infrastructure.Persistence.Entities.AchievementType.FotyPrivateEventsComplete))
            {
                var entity = await this.AchievementRepo.Add(BuildAchievement(userId, Infrastructure.Persistence.Entities.AchievementType.FotyPrivateEventsComplete));
                achivements.Add(new AchievementDto(entity));
            }

            if (totalPrivateEvents >= fiftyEvents && !allAchievements.ContainsKey(Infrastructure.Persistence.Entities.AchievementType.FiftyPrivateEventsComplete))
            {
                var entity = await this.AchievementRepo.Add(BuildAchievement(userId, Infrastructure.Persistence.Entities.AchievementType.FiftyPrivateEventsComplete));
                achivements.Add(new AchievementDto(entity));
            }
            return achivements;
        }

        private async Task<List<AchievementDto>> PublicEventCountHandler(int userId, SprintType sprintType, Dictionary<Infrastructure.Persistence.Entities.AchievementType, UserAchievement> allAchievements)
        {
            var achivements = new List<AchievementDto>();
            var totalPrivateEvents = this.AchievementRepo.GetEventCount(userId, sprintType);
            if (totalPrivateEvents >= tenEvents && !allAchievements.ContainsKey(Infrastructure.Persistence.Entities.AchievementType.TenPublicEventsComplete))
            {
                var entity = await this.AchievementRepo.Add(BuildAchievement(userId, Infrastructure.Persistence.Entities.AchievementType.TenPublicEventsComplete));
                achivements.Add(new AchievementDto(entity));
            }

            if (totalPrivateEvents >= twentyEvents && !allAchievements.ContainsKey(Infrastructure.Persistence.Entities.AchievementType.TwentyPublicEventsComplete))
            {
                var entity = await this.AchievementRepo.Add(BuildAchievement(userId, Infrastructure.Persistence.Entities.AchievementType.TwentyPublicEventsComplete));
                achivements.Add(new AchievementDto(entity));
            }

            if (totalPrivateEvents >= thirtyEvents && !allAchievements.ContainsKey(Infrastructure.Persistence.Entities.AchievementType.ThirtyPublicEventsComplete))
            {
                var entity = await this.AchievementRepo.Add(BuildAchievement(userId, Infrastructure.Persistence.Entities.AchievementType.ThirtyPublicEventsComplete));
                achivements.Add(new AchievementDto(entity));
            }

            if (totalPrivateEvents >= fortyEvents && !allAchievements.ContainsKey(Infrastructure.Persistence.Entities.AchievementType.FotyPublicEventsComplete))
            {
                var entity = await this.AchievementRepo.Add(BuildAchievement(userId, Infrastructure.Persistence.Entities.AchievementType.FotyPublicEventsComplete));
                achivements.Add(new AchievementDto(entity));
            }

            if (totalPrivateEvents >= fiftyEvents && !allAchievements.ContainsKey(Infrastructure.Persistence.Entities.AchievementType.FiftyPublicEventsComplete))
            {
                var entity = await this.AchievementRepo.Add(BuildAchievement(userId, Infrastructure.Persistence.Entities.AchievementType.FiftyPublicEventsComplete));
                achivements.Add(new AchievementDto(entity));
            }
            return achivements;
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