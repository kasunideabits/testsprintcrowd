namespace SprintCrowd.BackEnd.Domain.Notification
{
    /// <summary>
    /// User info for who send and receive notificaiton
    /// </summary>
    public class UserInfo
    {
        /// <summary>
        /// Initialize <see cref="UserInfo"> class</see>
        /// </summary>
        /// <param name="userId">user id of sender or receiver</param>
        /// <param name="name">name of sender or receiver</param>
        /// <param name="profilePicture">profile picture url for sender or receiver</param>
        /// <param name="code">notification code</param>
        public UserInfo(int userId, string name, string profilePicture, string code)
        {
            this.UserId = userId;
            this.Name = name;
            this.ProfilePicture = profilePicture;
            this.Code = code;
        }
        /// <summary>
        /// Gets or set user id of sender or receiver
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or setname of sender or receiver
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or set profile picture url for sender or receiver
        /// </summary>
        public string ProfilePicture { get; set; }

        /// <summary>
        /// Gets or set notification code
        /// </summary>
        public string Code { get; set; }

    }
}