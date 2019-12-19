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
        private const int _maxNight = 3;
        private UserPreference _userPreference { get; }

        public Expression<Func<SprintParticipant, bool>> Build(int offset)
        {
            Expression<Func<SprintParticipant, bool>> query1 = this.PublicSprintQuery();
            Expression<Func<SprintParticipant, bool>> query2 = this.ExtendtedTimeQuery(offset);
            Expression<Func<SprintParticipant, bool>> query3 = this.DayQyery(offset);
            Expression<Func<SprintParticipant, bool>> query4 = this.TimeQuery(offset);
            Expression<Func<SprintParticipant, bool>> query5 = query1.AndAlso(query2);
            Expression<Func<SprintParticipant, bool>> query6 = query5.AndAlso(query3);
            return query6.AndAlso(query4);
        }

        public Expression<Func<SprintParticipant, bool>> PublicSprintQuery()
        {
            Expression<Func<SprintParticipant, bool>> query = s => s.Sprint.Type == (int)SprintType.PublicSprint && s.User.UserState == UserState.Active;
            return query;
        }

        public Expression<Func<SprintParticipant, bool>> ExtendtedTimeQuery(int offset)
        {
            var now = DateTime.UtcNow.AddMinutes(offset + (-15));
            Expression<Func<SprintParticipant, bool>> query = s => s.Sprint.StartDateTime > now;
            return query;
        }

        public Expression<Func<SprintParticipant, bool>> DayQyery(int offset)
        {
            Expression<Func<SprintParticipant, bool>> query = s =>
                (this._userPreference.Mon && s.Sprint.StartDateTime.AddMinutes(offset).DayOfWeek == DayOfWeek.Monday) ||
                (this._userPreference.Tue && s.Sprint.StartDateTime.AddMinutes(offset).DayOfWeek == DayOfWeek.Tuesday) ||
                (this._userPreference.Wed && s.Sprint.StartDateTime.AddMinutes(offset).DayOfWeek == DayOfWeek.Wednesday) ||
                (this._userPreference.Thur && s.Sprint.StartDateTime.AddMinutes(offset).DayOfWeek == DayOfWeek.Thursday) ||
                (this._userPreference.Fri && s.Sprint.StartDateTime.AddMinutes(offset).DayOfWeek == DayOfWeek.Friday) ||
                (this._userPreference.Sat && s.Sprint.StartDateTime.AddMinutes(offset).DayOfWeek == DayOfWeek.Saturday) ||
                (this._userPreference.Sun && s.Sprint.StartDateTime.AddMinutes(offset).DayOfWeek == DayOfWeek.Sunday);
            return query;
        }

        public Expression<Func<SprintParticipant, bool>> TimeQuery(int offset)
        {
            Expression<Func<SprintParticipant, bool>> query = s =>
                (this._userPreference.Morning && s.Sprint.StartDateTime.AddMinutes(offset).Hour >= _minMorning && s.Sprint.StartDateTime.AddMinutes(offset).Hour <= _maxMorning) ||
                (this._userPreference.AfterNoon && s.Sprint.StartDateTime.AddMinutes(offset).Hour >= _minAfternoon && s.Sprint.StartDateTime.AddMinutes(offset).Hour <= _maxAfternoon) ||
                (this._userPreference.Evening && s.Sprint.StartDateTime.AddMinutes(offset).Hour >= _minEvening && s.Sprint.StartDateTime.AddMinutes(offset).Hour <= _maxEvening) ||
                (this._userPreference.Night && s.Sprint.StartDateTime.AddMinutes(offset).Hour >= _minNight && s.Sprint.StartDateTime.AddMinutes(offset).Hour < _maxNight);
            return query;
        }
    }
}