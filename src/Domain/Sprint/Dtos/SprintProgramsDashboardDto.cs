using SprintCrowdBackEnd.Infrastructure.Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SprintCrowdBackEnd.Domain.Sprint.Dtos
{
    public class SprintProgramsDashboardDto
    {
        public List<SprintProgramDto> dbPrograms { get; set; }
        public int totalItems { get; set; }

        public int totalParticipants { get; set; }
    }
}
