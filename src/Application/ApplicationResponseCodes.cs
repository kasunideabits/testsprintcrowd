namespace SprintCrowd.BackEnd.Application
{
  /// <summary>
  /// response codes to use when responding to rest requests.
  /// </summary>
  public enum ApplicationResponseCode
  {
    /// <summary>
    /// All went well.
    /// </summary>
    Success = 200,

    /// <summary>
    /// already exists.
    /// </summary>
    Exists = 409,

    /// <summary>
    /// expired.
    /// </summary>
    Expired = 401,

    /// <summary>
    /// Bad request.
    /// </summary>
    BadRequest = 400,

   /// <summary>
   ///NotExist.
   /// </summary>
   NotExist = 1001
  }
}