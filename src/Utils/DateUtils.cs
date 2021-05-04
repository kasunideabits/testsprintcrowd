namespace SprintCrowd.BackEnd.Utils
{
    using System;
    using System.Linq;

    /// <summary>
    /// For common use utils for Dates/Time
    /// </summary>
    public class DateUtils
    {

        private static Random random = new Random();

        /// <summary>
        /// Get Random Number of chars
        /// </summary>
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        /// <summary>
        /// Get Now time stamp (10 digit long)
        /// </summary>
        public static string getNowShortTimeStamp()
        {
            return DateTime.Now.ToString("yyMMddHHmmss");
        }

    }
}