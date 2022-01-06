using SprintCrowdBackEnd.Infrastructure.Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SprintCrowdBackEnd.Domain.Sprint.Dtos
{
    public class SprintProgramsPageDto
    {
        public List<SprintProgram> sPrograms { get; set; }
        public int totalItems { get; set; }
    }
}
