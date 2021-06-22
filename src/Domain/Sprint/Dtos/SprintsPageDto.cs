namespace SprintCrowd.BackEnd.Domain.Sprint.Dtos
{
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;
    using SprintCrowd.BackEnd.Domain.ScrowdUser;
    using System.Collections.Generic;

    public class SprintsPageDto
    {
        public List<Sprint> sprints { get; set; }
        public int totalItems { get; set; }
    }
}