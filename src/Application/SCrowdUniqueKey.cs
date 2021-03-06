namespace SprintCrowd.BackEnd.Application
{
  using System;

  /// <summary>
  /// Helper class for generate unique random values
  /// </summary>
  public static class SCrowdUniqueKey
  {
    /// <summary>
    /// Generate Unique user code
    /// </summary>
    /// <typeparam name="T">any argument unique to user,
    /// recommend to pass user email</typeparam>
    public static string UniqUserCode<T>(T user)
    {
      var first = Math.Abs(user.GetHashCode()).ToString();
      var second = Math.Abs(DateTime.UtcNow.GetHashCode()).ToString();
      var firstPart = first.Substring(first.Length - 4);
      var secondPart = second.Substring(second.Length - 4);
      return $"{firstPart}-{secondPart}";
    }

    /// <summary>
    /// Generate friend request code
    /// </summary>
    /// <param name="userCode">user unique code</param>
    /// <returns>unique friend request code</returns>
    public static string UniqFriendCode(string userCode)
    {
      var codeStr = Math.Abs(DateTime.UtcNow.GetHashCode()).ToString();
      var code = codeStr.Substring(codeStr.Length - 4);
      return $"{userCode}-{code}";
    }

    /// <summary>
    /// Generate unique code
    /// </summary>
    /// <returns>unique code for user</returns>
    public static string GetUniqueKey()
    {
      var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
      var stringChars = new char[8];
      var random = new System.Random();

      for (int i = 0; i < stringChars.Length; i++)
      {
        stringChars[i] = chars[random.Next(chars.Length)];
      }

      var finalString = new System.String(stringChars);

      return finalString;
    }

    /// <summary>
    /// Extract user code from friend code
    /// </summary>
    /// <param name="userCode">friend request code</param>
    /// <returns>user code</returns>
    public static string GetUserCode(string userCode) => userCode.Substring(0, 9);
  }
}