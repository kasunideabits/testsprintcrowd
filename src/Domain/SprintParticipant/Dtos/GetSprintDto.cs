using System.Collections.Generic;

namespace SprintCrowd.BackEnd.Domain.SprintParticipant.Dtos
{
    public class GetSprintDto
    {
        public SprintInfo Creator { get; set; }
        public List<SprintInfo> Other { get; set; }
    }
}