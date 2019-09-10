namespace SprintCrowd.BackEnd.Web.Sprint
{
    using Newtonsoft.Json.Converters;
    using Newtonsoft.Json;
    using SprintCrowd.BackEnd.Application;

    /// <summary>
    /// Sprint crowd users pariticipating sprint query params
    /// </summary>
    public class SprintQuery
    {
        /// <summary>
        /// Sprint type public or private
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public SprintType SprintType { get; set; }

        /// <summary>
        /// Paticipant stage joined, mark attandance or exit
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public ParticipantStage ParticipantStage { get; set; }

        /// <summary>
        /// Distance in meters
        /// </summary>
        public int Distance { get; set; }

        /// <summary>
        /// sprint start form minutes
        /// </summary>
        public int StartFrom { get; set; }
    }
}