namespace SprintCrowd.BackEnd.Domain.Sprint
{
    using System.Linq.Expressions;
    using System;
    using SprintCrowd.BackEnd.Application;
    using SprintCrowd.BackEnd.Common;
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;

    internal class PublicSprintQueryBuilder
    {
        public PublicSprintQueryBuilder(UserPreference userPreference)
        {
            this._userPreference = userPreference;
        }

        private const int _minMorning = 5;
        private const int _maxMorning = 11;
        private const int _minAfternoon = 12;
        private const int _maxAfternoon = 16;
        private const int _minEvening = 17;
        private const int _maxEvening = 20;
        private const int _minNight = 21;
        private const int _midNightMin = 23;
        private const int _midNightMax = 0;
        private const int _maxNight = 4;
        private UserPreference _userPreference { get; }

        public Expression<Func<Sprint, bool>> Build(int offset)
        {
            Expression<Func<Sprint, bool>> query1 = this.PublicSprintQuery();
            Expression<Func<Sprint, bool>> query2 = this.ExtendtedTimeQuery(offset);
            Expression<Func<Sprint, bool>> query3 = this.DayQyery(offset);
            Expression<Func<Sprint, bool>> query4 = this.TimeQuery(offset);
            Expression<Func<Sprint, bool>> query5 = this.DistanceQuery();

            Expression<Func<Sprint, bool>> query6 = query1.AndAlso(query2);
            Expression<Func<Sprint, bool>> query7 = query6.AndAlso(query3);
            Expression<Func<Sprint, bool>> query8 = query7.AndAlso(query4);
            return query8.AndAlso(query5);
        }

        public Expression<Func<Sprint, bool>> BuildOpenEvents(int offset)
        {
            var afterSevenDays = DateTime.UtcNow.AddDays(7);
            Expression<Func<Sprint, bool>> query1 = s => s.Type == (int)SprintType.PublicSprint &&
                s.StartDateTime > DateTime.UtcNow && s.StartDateTime < afterSevenDays &&
                s.Status != (int)SprintStatus.ARCHIVED;
            Expression<Func<Sprint, bool>> query2 = this.DayQyery(offset);
            Expression<Func<Sprint, bool>> query3 = this.TimeQuery(offset);
            Expression<Func<Sprint, bool>> query4 = this.DistanceQuery();

            Expression<Func<Sprint, bool>> query5 = query1.AndAlso(query2);
            Expression<Func<Sprint, bool>> query6 = query5.AndAlso(query3);

            return query6.AndAlso(query4);
        }

        public Expression<Func<Sprint, bool>> PublicSprintQuery()
        {
            Expression<Func<Sprint, bool>> query = s => s.Type == (int)SprintType.PublicSprint && s.Status != (int)SprintStatus.ARCHIVED;
            return query;
        }

        public Expression<Func<Sprint, bool>> ExtendtedTimeQuery(int offset)
        {
            var now = DateTime.UtcNow.AddMinutes(offset);
            Expression<Func<Sprint, bool>> query = s => s.StartDateTime <= now && now.AddMinutes(-15) < s.StartDateTime;
            return query;
        }

        public Expression<Func<Sprint, bool>> DayQyery(int offset)
        {
            Expression<Func<Sprint, bool>> query = s =>
                (this._userPreference.Mon && s.StartDateTime.AddMinutes(offset).DayOfWeek == DayOfWeek.Monday) ||
                (this._userPreference.Tue && s.StartDateTime.AddMinutes(offset).DayOfWeek == DayOfWeek.Tuesday) ||
                (this._userPreference.Wed && s.StartDateTime.AddMinutes(offset).DayOfWeek == DayOfWeek.Wednesday) ||
                (this._userPreference.Thur && s.StartDateTime.AddMinutes(offset).DayOfWeek == DayOfWeek.Thursday) ||
                (this._userPreference.Fri && s.StartDateTime.AddMinutes(offset).DayOfWeek == DayOfWeek.Friday) ||
                (this._userPreference.Sat && s.StartDateTime.AddMinutes(offset).DayOfWeek == DayOfWeek.Saturday) ||
                (this._userPreference.Sun && s.StartDateTime.AddMinutes(offset).DayOfWeek == DayOfWeek.Sunday);
            return query;
        }

        public Expression<Func<Sprint, bool>> TimeQuery(int offset)
        {
            Expression<Func<Sprint, bool>> query = s =>
                (this._userPreference.Morning && s.StartDateTime.AddMinutes(offset).Hour >= _minMorning && s.StartDateTime.AddMinutes(offset).Hour <= _maxMorning) ||
                (this._userPreference.AfterNoon && s.StartDateTime.AddMinutes(offset).Hour >= _minAfternoon && s.StartDateTime.AddMinutes(offset).Hour <= _maxAfternoon) ||
                (this._userPreference.Evening && s.StartDateTime.AddMinutes(offset).Hour >= _minEvening && s.StartDateTime.AddMinutes(offset).Hour <= _maxEvening) ||
                (
                    this._userPreference.Night &&
                    (
                        (s.StartDateTime.AddMinutes(offset).Hour >= _minNight && s.StartDateTime.AddMinutes(offset).Hour <= _midNightMin) ||
                        (s.StartDateTime.AddMinutes(offset).Hour >= _midNightMax && s.StartDateTime.AddMinutes(offset).Hour <= _maxNight)));
            return query;
        }

        private Expression<Func<Sprint, bool>> DistanceQuery()
        {
            Expression<Func<Sprint, bool>> query = s =>
                (this._userPreference.TwoToTen && s.Distance >= 2000 && s.Distance <= 10000) ||
                (this._userPreference.EleToTwenty && s.Distance > 10000 && s.Distance <= 20000) ||
                (this._userPreference.TOneToThirty && s.Distance > 20000 && s.Distance <= 30000);
            return query;
        }
    }
}