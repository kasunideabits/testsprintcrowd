namespace SprintCrowd.BackEnd.Infrastructure.Persistence.Entities
{
    using System.Collections.Generic;

    /// <summary>
    /// User model.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        public User()
        {
            this.AccessToken = new AccessToken();
        }

        /// <summary>
        /// gets or set value.
        /// </summary>
        /// <value>unique id for the user.</value>
        public int Id { get; set; }

        /// <summary>
        /// gets or set value.
        /// </summary>
        /// <value>type of the user.facebook or other..</value>
        public int UserType { get; set; }

        /// <summary>
        /// gets or set value.
        /// </summary>
        /// <value>facebook user id.</value>
        public string FacebookUserId { get; set; }

        /// <summary>
        /// gets or set value.
        /// </summary>
        /// <value>email of the user.</value>
        public string Email { get; set; }

        /// <summary>
        /// gets or set value.
        /// </summary>
        /// <value>name of the user.</value>
        public string Name { get; set; }

        /// <summary>
        /// gets or set value.
        /// </summary>
        /// <value>profile picture url of the user.</value>
        public string ProfilePicture { get; set; }

        /// <summary>
        /// Unique code for user
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// gets or set value.
        /// </summary>
        /// <value>list of sptints the user has participated and participating on.</value>
        public List<Sprint> Sprint { get; set; }

        /// <summary>
        /// gets or set value.
        /// </summary>
        /// <value>access token.</value>
        public AccessToken AccessToken { get; set; }

        /// <summary>
        /// Language Preference for user <see cref="LanguageCode"> Lanugage code </see>
        /// </summary>
        /// <value> Lanugage code </value>
        public string LanguagePreference { get; set; } = LanguageCode.English;

        /// <summary>
        /// Gets or set reference for participates
        /// </summary>
        public List<SprintParticipant> Participates { get; set; }

        /// <summary>
        /// Gets or set Notification reference for sender
        /// </summary>
        public virtual List<Notification> SenderNotification { get; set; }

        /// <summary>
        /// Gets or set Notification reference for receiver
        /// </summary>
        public virtual List<Notification> ReceiverNotification { get; set; }

        /// <summary>
        /// Gets or set reference for achievements
        /// </summary>
        public virtual List<Achievement> Achievements { get; set; }

        /// <summary>
        /// Gets or set inviter rederence
        /// </summary>
        public virtual List<SprintInvite> Inviter { get; set; }

        /// <summary>
        /// Gets or set invitee reference
        /// </summary>
        public virtual List<SprintInvite> Invitee { get; set; }

        /// <summary>
        /// Gets or set friend requester reference
        /// </summary>
        public virtual List<Friend> FriendRequester { get; set; }

        /// <summary>
        /// Gets or set frineds reference
        /// </summary>
        public virtual List<Friend> Friends { get; set; }

    }
}