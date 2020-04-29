using System;
using System.Collections.Generic;


namespace SprintCrowd.BackEnd.Domain.SprintParticipant.Dtos
{
    public class GetSprintDto
    {
        public SprintInfoDTO Creator { get; set; }
        public List<JoinedSprintDTO> Other { get; set; }

    }

    public class JoinedSprintDTO
    {
        public SprintInfoDTO SprintInfo { get; set; }
        public List<ParticipantInfoDTO> ParticipantInfo { get; set; }
    }

    public class ParticipantInfoDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ColorCode { get; set; }

        public bool IsFriend { get; set; }
    }

    public class SprintInfoDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int Distance { get; set; }
        public DateTime StartTime { get; set; }

        public DateTime ExtendedTime { get; set; }

        public bool Creator { get; set; }

        public int Type { get; set; }

    }
}