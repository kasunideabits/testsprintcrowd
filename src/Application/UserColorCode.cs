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

        private const string bfeda6 = "#bfeda6";
        private const string deeba6 = "#deeba6";
        private const string f0cc8c = "#f0cc8c";
        private const string f2b8a3 = "#f2b8a3";
        private const string a3d1f0 = "#a3d1f0";
        private const string a3aded = "#a3aded";
        private const string dba6e8 = "#dba6e8";
        private const string c2a3eb = "#c2a3eb";
        private const string e5a6cc = "#e5a6cc";

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
            this.Colors.AddRange(new string [] { c8e4f5, c8f1f4, c8f4dc, bfeda6, deeba6, f0cc8c, f2b8a3, a3d1f0, a3aded, dba6e8, c2a3eb, e5a6cc });
        }
    }
}