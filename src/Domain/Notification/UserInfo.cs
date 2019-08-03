namespace SprintCrowd.BackEnd.Domain.Notification
{
    public class UserInfo
    {
        public UserInfo(int userId, string name, string profilePicture, string code)
        {
            this.UserId = userId;
            this.Name = name;
            this.ProfilePicture = profilePicture;
            this.Code = code;
        }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string ProfilePicture { get; set; }
        public string Code { get; set; }

    }
}