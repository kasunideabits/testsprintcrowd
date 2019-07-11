namespace SprintCrowd.BackEnd.Domain.Sprint
{
    /// <summary>
    /// Class for available sprint count
    /// </summary>
    public class CreatedSprintCount
    {
        /// <summary>
        /// Initialize CreatedSprintCount class
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="privateCount"></param>
        /// <param name="publicCount"></param>
        public CreatedSprintCount(int totalCount, int privateCount, int publicCount)
        {
            this.Total = totalCount;
            this.Private = privateCount;
            this.Public = publicCount;
        }

        /// <summary>
        /// Total sprint count
        /// </summary>
        /// <value>total sprint count</value>
        public int Total { get; set; }

        /// <summary>
        /// Public sprint count
        /// </summary>
        /// <value>public sprint cout</value>
        public int Public { get; set; }

        /// <summary>
        /// Private sprint count
        /// </summary>
        /// <value>private sprint count</value>
        public int Private { get; set; }
    }
}