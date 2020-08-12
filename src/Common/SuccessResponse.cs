namespace SprintCrowd.BackEnd.Common
{
    public class SuccessResponse<T>
    {
        public SuccessResponse(T data)
        {
            this.Data = data;
        }
        public T Data { get; set; }
    }


    public class PrivateSprint
    {
        public static class PrivateSprintDefaultConfigration
        {
            public static string PrivateSprintCount { get; set; }
            public static string LapsTime { get; set; }

        }


    }
}