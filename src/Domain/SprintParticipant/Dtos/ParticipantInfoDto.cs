using SprintCrowd.BackEnd.Application;
using SprintCrowd.BackEnd.Common;

namespace SprintCrowd.BackEnd.Domain.SprintParticipant
{
    /// <summary>
    /// class for indicate sprint participant basic info
    /// </summary>
    public class ParticipantInfoDto : ParticipantBaseDto
    {
        /// <summary>
        /// Initialize <see cref="ParticipantInfoDto">ParticipantInfoDto class </see>
        /// </summary>
        /// <param name="userId">user id for participant</param>
        /// <param name="userName">name for participant</param>
        /// <param name="profilePicture">profile picture url for participant</param>
        /// <param name="userCode">user code</param>
        /// <param name="colorCode">user color code</param>
        /// <param name="city">user city</param>
        /// <param name="country">user country</param>
        /// <param name="countryCode">user country cde</param>
        /// <param name="stage">participant stage</param>
        /// <param name="creator">creator or not</param>
        public ParticipantInfoDto(
            int userId,
            string userName,
            string profilePicture,
            string userCode,
            string colorCode,
            string city,
            string country,
            string countryCode,
            bool isIinfluencerEventParticipant,
            ParticipantStage stage,
            bool creator = false) : base(userId, userName, profilePicture, city, country, countryCode)
        {
            this.Code = userCode;
            this.ColorCode = colorCode;
            this.Creator = creator;
            this.Stage = stage;
            this.IsIinfluencerEventParticipant = isIinfluencerEventParticipant;
        }

        /// <summary>
        /// User code
        /// </summary>
        public string Code { get; }

        /// <summary>
        /// Gets color code
        /// </summary>
        public string ColorCode { get; }

        /// <summary>
        /// Gets creator or not
        /// </summary>
        public bool Creator { get; }

        /// <summary>
        /// participant stage
        /// </summary>
        public ParticipantStage Stage { get; }

        /// <summary>
        /// Is Iinfluencer Event Participant
        /// </summary>
        public bool IsIinfluencerEventParticipant { get; }
    }
}