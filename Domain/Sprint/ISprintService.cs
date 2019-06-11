namespace SprintCrowdBackEnd.Domain.Sprint
{

  using System.Threading.Tasks;
  using SprintCrowdBackEnd.Infrastructure.Persistence.Entities;

  public interface ISprintService
  {

    Task<Sprint> UpdateSprint(SprintModel sprintData);

  }
}