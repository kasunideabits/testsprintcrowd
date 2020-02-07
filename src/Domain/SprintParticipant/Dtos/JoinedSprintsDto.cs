namespace SprintCrowd.BackEnd.Domain.SprintParticipant.Dtos
{
    using System.Collections.Generic;
    using System;

    public class JoinedSprintsDto
    {
        public JoinedSprintsDto(List<JoinedSprintDto> joinedSprints, List<DateTime> dates)
        {
            this.JoinedSprints = joinedSprints;
            this.Dates = dates;
        }
        public List<JoinedSprintDto> JoinedSprints { get; set; }
        public List<DateTime> Dates { get; set; }
    }
}