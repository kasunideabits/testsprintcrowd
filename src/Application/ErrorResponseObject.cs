namespace SprintCrowd.BackEnd.Application
{
  /// <summary>
  /// Each and every rest api will return a object of this, it will make it easy for the mobile clients
  /// to do error handling.
  /// </summary>
  public class ErrorResponseObject
  {
    /// <summary>
    /// gets or set value.
    /// </summary>
    /// <value>custom error code for the operation.</value>
    public int ErrorCode { get; set; }

    /// <summary>
    /// gets or set value.
    /// </summary>
    /// <value>error message.</value>
    public dynamic ErrorMessage { get; set; }
  }
}