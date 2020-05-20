namespace SprintCrowd.BackEnd.Domain.Sprint
{
    /// <summary>
    /// Live sprint count model
    /// </summary>
    public class LiveSprintCount
    {
        /// <summary>
        /// Initialize LiveSprintCount class
        /// </summary>
        /// <param name="all">all sprint count</param>
        /// <param name="twoToTen">2-10km sprint count</param>
        /// <param name="tenToTwenty">10-20km sprint count</param>
        /// <param name="twentyOneToThirty">21-30 sprint count</param>
        /// <param name="thirtyOneToFortyOne">31-41 sprint count</param>
        public LiveSprintCount(int all, int twoToTen, int tenToTwenty, int twentyOneToThirty, int thirtyOneToFortyOne)
        {
            this.All = all;
            this.TwoToTen = twoToTen;
            this.TenToTwenty = tenToTwenty;
            this.TwentyOneToThirty = twentyOneToThirty;
            this.ThirtyOneToFortyOne = thirtyOneToFortyOne;
        }

        /// <summary>
        /// Total count of sprints
        /// </summary>
        /// <value>Toatal ongoing sprints</value>
        public int All { get; }

        /// <summary>
        /// Ongoing sprints distance between 2-10KM
        /// </summary>
        /// <value>Ongoing sprints with distance 2-10KM</value>
        public int TwoToTen { get; }

        /// <summary>
        /// Ongoing sprints distance between 10-20KM
        /// </summary>
        /// <value>Ongoing sprints with distance 10-20KM</value>
        public int TenToTwenty { get; }

        /// <summary>
        /// OnGoing sprints distance between 21-30KM
        /// </summary>
        /// <value>Ongoing sprint with distance 21-30KM</value>
        public int TwentyOneToThirty { get; }

        /// <summary>
        /// OnGoing sprints distance between 31-41KM
        /// </summary>
        /// <value>Ongoing sprint with distance 31-41KM</value>
        public int ThirtyOneToFortyOne { get; }
    }
}