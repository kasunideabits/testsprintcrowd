namespace SprintCrowd.BackEnd.Domain.Device
{
    /// <summary>
    /// Modal of device os platform
    /// </summary>
    public class DeviceModal
    {
        /// <summary>
        /// Initialize device info
        /// </summary>
        public DeviceModal(int all, int ios, int android)
        {
            this.All = all;
            this.IOS = ios;
            this.Android = android;

        }
        /// <summary>
        /// get all devices count
        /// </summary>
        public int All { get; }
        /// <summary>
        /// get android devices count
        /// </summary>
        public int IOS { get; }
        /// <summary>
        /// get apple devices count
        /// </summary>
        public int Android { get; }
    }
}