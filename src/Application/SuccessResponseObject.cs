namespace SprintCrowd.BackEnd.Application
{
  /// <summary>
  /// Each and every rest api will return a object of this, it will make it easy for the mobile clients
  /// to do error handling.
  /// </summary>
  public class SuccessResponseObject
  {
    /// <summary>
    /// gets or set value.
    /// </summary>
    /// <value>success response data for the operation.</value>
    public dynamic data { get; set; }
  }
}