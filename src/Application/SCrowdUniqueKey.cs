namespace SprintCrowd.BackEnd.Application
{
    using System;

    /// <summary>
    /// Helper class for generate unique random values
    /// </summary>
    public class SCrowdUniqueKey
    {
        /// <summary>
        /// Generate Unique user code
        /// </summary>
        /// <typeparam name="T">any argument unique to user,
        /// recommend to pass user email</typeparam>
        public string UniqUserCode<T>(ref T user)where T : class
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
        public string UniqFriendCode(string userCode)
        {
            var codeStr = Math.Abs(DateTime.UtcNow.GetHashCode()).ToString();
            var code = codeStr.Substring(codeStr.Length - 4);
            return $"{userCode}-{code}";
        }

        /// <summary>
        /// Extract user code from friend code
        /// </summary>
        /// <param name="userCode">friend request code</param>
        /// <returns>user code</returns>
        public string GetUserCode(string userCode) => userCode.Substring(0, 9);
    }
}