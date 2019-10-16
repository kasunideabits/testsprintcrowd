namespace SprintCrowd.BackEnd.Domain.Friend
{
    public class RemoveFriendDto
    {
        public RemoveFriendDto(string message)
        {
            this.Message = message;
        }

        public string Message { get; }
    }

}