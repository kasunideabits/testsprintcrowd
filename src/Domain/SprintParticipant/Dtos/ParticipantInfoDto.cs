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
        public ParticipantInfoDto(
            int userId,
            string userName,
            string profilePicture,
            string userCode,
            string colorCode,
            string city,
            string country,
            string countryCode,
            ParticipantStage stage,
            bool creator = false) : base(userId, userName, profilePicture, city, country, countryCode)
        {
            this.Code = userCode;
            this.ColorCode = colorCode;
            this.Creator = creator;
            this.Stage = stage;
        }

        /// <summary>
        /// User code
        /// </summary>
        public string Code { get; }

        /// <summary>
        /// Gets color code
        /// </summary>
        public string ColorCode { get; }
        public bool Creator { get; }
        public ParticipantStage Stage { get; }

    }
}