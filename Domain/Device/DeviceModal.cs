namespace SprintCrowd.BackEnd.Domain.Device
{
    public class DeviceModal
    {
        public DeviceModal(int all, int ios, int android)
        {
            this.All = all;
            this.IOS = ios;
            this.Android = android;
        }

        public int All { get; set; }
        public int IOS { get; set; }
        public int Android { get; set; }

    }
}