namespace SprintCrowd.BackEnd.Utils

{
    using System.Text.RegularExpressions;

    /// <summary>
    /// For common use utils for String utils
    /// </summary>
    public class StringUtils
    {


        /// <summary>
        /// check given string is base 64 or not
        /// </summary>
        public static bool IsBase64String(string baseString)
        {
            string trimmed = baseString.Trim();
            return (trimmed.Length % 4 == 0) && Regex.IsMatch(trimmed, @"^[a-zA-Z0-9\+/]*={0,3}$", RegexOptions.None);
        }
    }
}