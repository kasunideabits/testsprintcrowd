using System.ComponentModel.DataAnnotations;

namespace SprintCrowd.BackEnd.Infrastructure.Persistence.Entities
{
    /// <summary>
    /// Firebase token
    /// </summary>
    public class FirebaseMessagingToken
    {
        /// <summary>
        /// unique id for the record
        /// </summary>
        /// <value>unique id</value>
        public int Id { get; set; }
        /// <summary>
        /// id of the user
        /// </summary>
        /// <value>user id</value>
        public User User { get; set; }
        /// <summary>
        /// firebase token
        /// </summary>
        /// <value>fcm token</value>
        [MaxLength(500)]
        public string Token { get; set; }
    }
}