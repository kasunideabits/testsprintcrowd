using System;

namespace SprintCrowd.BackEnd.Infrastructure.Persistence.Entities
{
  /// <summary>
  /// model for holding download device info
  /// </summary>
  /// <value></value>
  public class UserActivity : BaseEntity
  {
    /// <summary>
    /// UserActivity id
    /// </summary>
    /// <value>Unique Id</value>
    public int Id { get; set; }

    /// <summary>
    /// activity user id
    /// </summary>
    /// <value>user id</value>
    public int UserId { get; set; }

    /// <summary>
    /// activity date
    /// </summary>
    /// <value>Date and time</value>
    public DateTime DateTime { get; set; }

    /// <summary>
    /// number of times per day
    /// </summary>
    /// <value>count</value>
    public int ActivityCount { get; set; }

    /// <summary>
    /// activity user
    /// </summary>
    /// <value>user</value>
    public virtual User User { get; set; }
  }
}