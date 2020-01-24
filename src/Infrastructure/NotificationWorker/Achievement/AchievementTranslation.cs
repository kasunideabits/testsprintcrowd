using System.IO;
using Newtonsoft.Json.Linq;
using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;

namespace SprintCrowd.BackEnd.Infrastructure.NotificationWorker.Achievement
{
    internal class AchievementTranslation
    {
        public AchievementTranslation(string lang)
        {
            switch (lang)
            {
                case "en":
                    this.translation = JObject.Parse(File.ReadAllText(@"Translation/en.json"));
                    break;
                case "se":
                    this.translation = JObject.Parse(File.ReadAllText(@"Translation/se.json"));
                    break;
                default:
                    this.translation = JObject.Parse(File.ReadAllText(@"Translation/en.json"));
                    break;
            }
        }
        private JToken translation { get; }

        public SCFireBaseNotificationMessage Get(AchievementType type)
        {
            var section = this.GetSection(type);
            return new SCFireBaseNotificationMessage(section);
        }

        private JToken GetSection(AchievementType type)
        {
            switch (type)
            {
                case AchievementType.CrowdFunded:
                    return this.translation ["achievement"] ["crowdFunded"];
                case AchievementType.FiftyComplete:
                    return this.translation ["achievement"] ["fiftyComplete"];
                case AchievementType.FiftyPrivateEventsComplete:
                    return this.translation ["achievement"] ["fiftyPrivateEventsComplete"];
                case AchievementType.FiftyPublicEventsComplete:
                    return this.translation ["achievement"] ["fiftyPublicEventsComplete"];
                case AchievementType.FortyComplete:
                    return this.translation ["achievement"] ["fortyComplete"];
                case AchievementType.FotyPrivateEventsComplete:
                    return this.translation ["achievement"] ["fotyPrivateEventsComplete"];
                case AchievementType.FotyPublicEventsComplete:
                    return this.translation ["achievement"] ["fotyPublicEventsComplete"];
                case AchievementType.JoinedTheCrowd:
                    return this.translation ["achievement"] ["joinedTheCrowd"];
                case AchievementType.TenComplete:
                    return this.translation ["achievement"] ["tenComplete"];
                case AchievementType.TenPrivateEventsComplete:
                    return this.translation ["achievement"] ["tenPrivateEventsComplete"];
                case AchievementType.TenPublicEventsComplete:
                    return this.translation ["achievement"] ["tenPublicEventsComplete"];
                case AchievementType.ThirtyComplete:
                    return this.translation ["achievement"] ["thirtyComplete"];
                case AchievementType.ThirtyPrivateEventsComplete:
                    return this.translation ["achievement"] ["thirtyPrivateEventsComplete"];
                case AchievementType.ThirtyPublicEventsComplete:
                    return this.translation ["achievement"] ["thirtyPublicEventsComplete"];
                case AchievementType.ThreeIsCrowd:
                    return this.translation ["achievement"] ["threeIsCrowd"];
                case AchievementType.TwentyComplete:
                    return this.translation ["achievement"] ["twentyComplete"];
                case AchievementType.TwentyPrivateEventsComplete:
                    return this.translation ["achievement"] ["twentyPrivateEventsComplete"];
                case AchievementType.TwentyPublicEventsComplete:
                    return this.translation ["achievement"] ["twentyPublicEventsComplete"];
                default:
                    break;
            }
            return null;
        }
    }
}