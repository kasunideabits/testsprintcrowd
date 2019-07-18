namespace SprintCrowd.BackEnd.Infrastructure.Persistence.Entities
{
  /// <summary>
  /// Access tokens.
  /// </summary>
  public class Achievement
  {
    /// <summary>
    /// gets or sets value.
    /// </summary>
    /// <value>unique identifier.</value>
    public int Id { get; set; }
    /// <summary>
    /// gets or sets value.
    /// </summary>
    /// <value>user whom the achivement belongs to.</value>
    public User User { get; set; }
    /// <summary>
    /// Achivement Type
    /// </summary>
    /// <value>User reference</value>
    public int Type { get; set; }
  }
}