namespace SprintCrowd.BackEnd.Domain.Sprint
{
    using System.Linq.Expressions;
    using System;
    using SprintCrowd.BackEnd.Common;
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;

    internal class PublicSprintQueryBuilder
    {
        public PublicSprintQueryBuilder(UserPreference userPreference)
        {
            this._userPreference = userPreference;
        }

        private const int _minMorning = 4;
        private const int _maxMorning = 11;
        private const int _minAfternoon = 12;
        private const int _maxAfternoon = 16;
        private const int _minEvening = 17;
        private const int _maxEvening = 19;
        private const int _minNight = 20;
        private const int _maxNight = 23;
        private UserPreference _userPreference { get; }

        public Expression<Func<SprintParticipant, bool>> Build(int offset)
        {
            Expression<Func<SprintParticipant, bool>> query1 = s => this.DayQyery(s.Sprint.StartDateTime, offset);
            //  Expression<Func<SprintParticipant, bool>> query2 = s => this.TimeQuery(s.Sprint.StartDateTime, offset);
            // return query1.AndAlso(query2);
            return query1;
        }

        public bool DayQyery(DateTime startTime, int offset)
        {
            var utcDay = startTime.AddMinutes(offset).DayOfWeek;
            return (
                (this._userPreference.Mon && utcDay == DayOfWeek.Monday) ||
                (this._userPreference.Tue && utcDay == DayOfWeek.Tuesday) ||
                (this._userPreference.Wed && utcDay == DayOfWeek.Wednesday) ||
                (this._userPreference.Thur && utcDay == DayOfWeek.Thursday) ||
                (this._userPreference.Fri && utcDay == DayOfWeek.Friday) ||
                (this._userPreference.Sat && utcDay == DayOfWeek.Saturday) ||
                (this._userPreference.Sun && utcDay == DayOfWeek.Sunday)
            );
        }

        public bool TimeQuery(DateTime startTime, int offset)
        {
            var clientDay = DateTime.UtcNow.AddMinutes(offset).DayOfWeek;
            var utcHour = startTime.AddMinutes(offset).Hour;
            return (
                (this._userPreference.Morning && utcHour >= _minMorning && utcHour < _maxMorning) ||
                (this._userPreference.AfterNoon && utcHour >= _minAfternoon && utcHour < _maxAfternoon) ||
                (this._userPreference.Evening && utcHour >= _minEvening && utcHour < _maxEvening) ||
                (this._userPreference.Morning && utcHour >= _minNight && utcHour < _maxNight));
        }
    }
}