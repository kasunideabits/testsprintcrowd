namespace SprintCrowdBackEnd.Application
{
  /// <summary>
  /// common enum for device type, ios or android
  /// </summary>
  public enum SprintStatus
  {
    /// <summary>
    /// device type is android
    /// </summary>
    NOTSTARTEDYET = 0,

    /// <summary>
    /// sprint is in progress
    /// </summary>
    INPROGRESS = 1,

    /// <summary>
    /// sprint has ended
    /// </summary>
    ENDED = 2
  }
}