namespace SprintCrowd.BackEnd.Application
{
  /// <summary>
  /// User type used for various things, ex: sending registeration request to identity server.
  /// </summary>
  public enum UserType
  {
    /// <summary>
    /// User type facebook.
    /// </summary>
    Facebook = 1,
    /// <summary>
    /// User type google.
    /// </summary>
    Google = 2,
    /// <summary>
    /// normal user, without any third party provider.
    /// </summary>
    NormalUser = 3,
  }
}