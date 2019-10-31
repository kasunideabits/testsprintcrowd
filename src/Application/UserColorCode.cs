namespace SprintCrowd.BackEnd.Application
{
    using System.Collections.Generic;
    using System;

    /// <summary>
    /// Generate random user color code
    /// </summary>
    public class UserColorCode
    {
        /// <summary>
        /// Initialize class
        /// </summary>
        public UserColorCode()
        {
            this.InitializeColors();
        }

        private const string c8e4f5 = "#c8e4f5";
        private const string c8f1f4 = "#c8f1f4";
        private const string c8f4dc = "#c8f4dc";
        private List<string> Colors { get; set; }

        /// <summary>
        /// Pick random color
        /// </summary>
        /// <returns>color hexa code</returns>
        public string PickColor()
        {
            var randomIndex = new Random().Next(0, this.Colors.Count);
            return this.Colors [randomIndex];
        }

        private void InitializeColors()
        {
            this.Colors = new List<string>();
            this.Colors.AddRange(new string [] { c8e4f5, c8f1f4, c8f4dc });
        }
    }
}