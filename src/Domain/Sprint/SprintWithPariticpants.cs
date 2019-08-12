using System;
using System.Collections.Generic;
using SprintCrowd.BackEnd.Application;

namespace SprintCrowd.BackEnd.Domain.Sprint
{
    /// <summary>
    /// Class which give informantion for sprint and participant
    /// details
    /// </summary>
    public class SprintWithPariticpants
    {
        /// <summary>
        /// Gets or set sprint id
        /// </summary>
        public int SprintId { get; set; }

        /// <summary>
        /// Gets or set sprint name
        /// </summary>
        public string SprintName { get; set; }

        /// <summary>
        /// Gets or set sprint distance
        /// </summary>
        public int Distance { get; set; }

        /// <summary>
        /// Gets or set sprint location
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        ///  Gets or set sprint start date time
        /// </summary>
        public DateTime StartDateTime { get; set; }

        /// <summary>
        /// Gets or set sprint type <see cref="SprintType">pubic or private </see>
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// Gets or set number of participants
        /// </summary>
        public int NumberOfParticipants { get; set; }

        /// <summary>
        ///  Gets or set all participants for given event
        /// </summary>
        /// <typeparam name="JoindedRunner"><see cref="JoindedRunner"> Jonied runners</typeparam>
        public List<JoindedRunner> Participants { get; set; } = new List<JoindedRunner>();

        /// <summary>
        /// Add pariticipants who as join to sprint
        /// </summary>
        /// <param name="userId">user id for the participant</param>
        /// <param name="name">name for the participant</param>
        /// <param name="profilePicture">profile picture url for the participant</param>
        /// <param name="code">user code</param>
        public void AddParticipant(int userId, string name, string profilePicture, string code)
        {
            this.Participants.Add(new JoindedRunner(userId, name, profilePicture, code));
        }
    }

    /// <summary>
    /// Join runners details
    /// </summary>
    public class JoindedRunner
    {
        /// <summary>
        /// Initialize <see cref="JoindedRunner">class </see>
        /// </summary>
        /// <param name="userId">user id for the participant</param>
        /// <param name="name">name for the participant</param>
        /// <param name="profilePicture">profile picture url for the participant</param>
        /// <param name="userCode">user code</param>
        public JoindedRunner(int userId, string name, string profilePicture, string userCode)
        {
            this.UserId = userId;
            this.Name = name;
            this.ProfilePicutre = profilePicture;
            this.Code = userCode;
        }

        /// <summary>
        ///  Gets or set user id
        /// </summary>
        public int UserId { get; }

        /// <summary>
        /// Gets or set user name
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets or set profile picture url
        /// </summary>
        public string ProfilePicutre { get; }

        /// <summary>
        /// Gets user code
        /// </summary>
        public string Code { get; }
    }
}