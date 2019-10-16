namespace SprintCrowd.BackEnd.Domain.Crons
{
  using System.Collections.Generic;
  using System.Threading.Tasks;
  using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;

  /// <summary>
  ///  Interface for reset user code service
  /// </summary>
  public interface IResetUserCodeService
  {
    /// <summary>
    /// Reset codes of all users
    /// </summary>
    Task ResetUserCode();
  }
}