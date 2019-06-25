namespace SprintCrowd.BackEnd.Domain.Sprint
{
    public class CreatedSprintCount
    {
        public CreatedSprintCount(int totalCount, int privateCount, int publicCount)
        {
            this.Total = totalCount;
            this.Private = privateCount;
            this.Public = publicCount;
        }
        public int Total { get; set; }

        public int Public { get; set; }

        public int Private { get; set; }
    }
}