﻿namespace SprintCrowd.BackEnd.Domain.Sprint
{
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Linq;
    using System.Threading.Tasks;
    using System;
    using Microsoft.EntityFrameworkCore;
    using SprintCrowd.BackEnd.Application;
    using SprintCrowd.BackEnd.Domain.Sprint.Dlos;
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;
    using SprintCrowd.BackEnd.Infrastructure.Persistence;

    /// <summary>
    /// Event repositiory
    /// </summary>
    public class SprintRepo : ISprintRepo
    {
        private ScrowdDbContext dbContext;
        /// <summary>
        /// intializes an instace of EventRepo
        /// </summary>
        /// <param name="dbContext">db context</param>
        public SprintRepo(ScrowdDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        /// <summary>
        /// Get sprint by given predicate
        /// </summary>
        /// <param name="predicate"> predicate</param>
        public async Task<Sprint> GetSprint(Expression<Func<Sprint, bool>> predicate)
        {
            return await this.dbContext.Set<Sprint>().FirstOrDefaultAsync(predicate);
        }

        /// <summary>
        /// Get all sprints with given predicate
        /// </summary>
        /// <param name="predicate">query </param>
        /// <returns>all sprints match to predicate</returns>
        public async Task<IQueryable<Sprint>> GetSprints(Expression<Func<Sprint, bool>> predicate)
        {
            var result = this.dbContext.Sprint.Include(s => s.Participants).ThenInclude(s => s.User).Where(predicate);
            return result;
        }

        public async Task<List<Sprint>> GetAllEvents()
        {
            return await this.dbContext.Sprint.ToListAsync();
        }

        /// <summary>
        /// Get all events
        /// </summary>
        /// <returns>Available events</returns>
        public async Task<List<Sprint>> GetAllEvents(int eventType)
        {
            return await this.dbContext.Sprint.Where(s => s.Type == eventType).ToListAsync();
        }

        /// <summary>
        /// Get created sprint count for given date range
        /// </summary>
        /// <param name="from">Start date for filter</param>
        /// <param name="to">End date for filter</param>
        /// <returns>Created All, Public, Private sprints</returns>
        public async Task<List<Sprint>> GetAllEvents(DateTime from, DateTime to)
        {
            return await this.dbContext.Sprint
                .Where(s => s.CreatedDate >= from && s.CreatedDate <= to)
                .ToListAsync();
        }

        /// <summary>
        /// Get all ongoing sprint sprints
        /// </summary>
        /// <returns>All ongoing sprints which not completed 24H</returns>
        public async Task<List<Sprint>> GetLiveSprints()
        {
            return await this.dbContext.Sprint
                .Where(s => s.Status == (int)SprintStatus.INPROGRESS)
                .ToListAsync();
        }

        /// <summary>
        /// creates event
        /// </summary>
        /// <param name="sprintToAdd">event model</param>
        /// <returns>added sprint result</returns>
        public async Task<Sprint> AddSprint(Sprint sprintToAdd)
        {
            var result = await this.dbContext.Sprint.AddAsync(sprintToAdd);
            return result.Entity;
        }

        /// <summary>
        /// creates event
        /// </summary>
        /// <param name="sprintToAdd">event model</param>
        /// <returns>added sprint result</returns>
        public async Task<Sprint> DraftSprint(Sprint sprintToAdd)
        {
            var result = await this.dbContext.Sprint.AddAsync(sprintToAdd);
            return result.Entity;
        }

        /// <summary>
        /// Update event details instance of SprintService
        /// </summary>
        /// <param name="sprintData">sprint repository</param>
        public async Task<Sprint> UpdateSprint(Sprint sprintData)
        {
            var result = this.dbContext.Sprint.Update(sprintData);
            return result.Entity;
        }

        /// <summary>
        /// Get the participants with given predicate
        /// sprint id
        /// </summary>
        /// <param name="predicate">predicate for lookup</param>
        /// <returns><see cref="SprintParticipant">sprint pariticipants</see></returns>
        public IEnumerable<SprintParticipant> GetParticipants(Expression<Func<SprintParticipant, bool>> predicate)
        {
            return this.dbContext.SprintParticipant
                .Include(s => s.Sprint)
                .ThenInclude(s => s.CreatedBy)
                .Include(s => s.User)
                .Where(predicate);
        }

        /// <summary>
        /// Add paritipant to sprint
        /// </summary>
        /// <param name="userId">user id for pariticipant</param>
        /// <param name="sprintId">sprint id which going to join</param>
        /// <param name="participantStage">sprint participant stage</param>
        public async Task AddParticipant(int userId, int sprintId, ParticipantStage participantStage)
        {
            SprintParticipant pariticipant = new SprintParticipant()
            {
                UserId = userId,
                SprintId = sprintId,
                Stage = participantStage,
            };
            await this.dbContext.AddAsync(pariticipant);
        }

        /// <summary>
        /// Remove sprint with given id
        /// </summary>
        /// <param name="sprint">sprint entity</param>
        public void RemoveSprint(Sprint sprint)
        {
            this.dbContext.Set<Sprint>().Remove(sprint);
        }

        public async Task<UserPreference> GetUserPreference(int userId)
        {
            return await this.dbContext.UserPreferences.FirstOrDefaultAsync(u => u.UserId == userId);
        }

        /// <summary>
        /// Get friend list for given user id
        /// </summary>
        /// <param name="userId">user id to fetch</param>
        /// <returns>Friends</returns>
        public IEnumerable<Friend> GetFriends(int userId)
        {
            return this.dbContext.Frineds.Where(f => f.SharedUserId == userId || f.AcceptedUserId == userId);
        }

        /// <summary>
        /// Get open events
        /// </summary>
        /// <param name="sprintPredicate">sprint filter with user preference</param>
        /// <returns></returns>
        public IEnumerable<OpenEventDlo> GetOpenEvents(Expression<Func<Sprint, bool>> sprintPredicate)
        {
            var afterSevenDays = DateTime.UtcNow.AddDays(7);
            var result = this.dbContext.Sprint
                .Where(sprintPredicate)
                .Join(this.dbContext.SprintParticipant,
                    s => s.Id,
                    sp => sp.SprintId,
                    (s, sp) => new { Id = s.Id, SprintInfo = s, ParticipantInfo = sp.User, ParticipantStage = sp.Stage })
                .Where(s =>
                    (s.ParticipantStage != ParticipantStage.DECLINE || s.ParticipantStage != ParticipantStage.QUIT) &&
                    s.SprintInfo.StartDateTime > DateTime.UtcNow && s.SprintInfo.StartDateTime < afterSevenDays)
                .GroupBy(s => s.SprintInfo.Id, (key, group) => new OpenEventDlo(group.Select(x => x.SprintInfo).ElementAt(0), group.Select(x => x.ParticipantInfo)))
                .OrderBy(s => s.Sprint.StartDateTime);
            return result;
        }

        /// <summary>
        /// commit and save changes to the db
        /// only call this from the service, DO NOT CALL FROM REPO ITSELF
        /// Unit of work methology.
        /// </summary>
        public void SaveChanges()
        {
            this.dbContext.SaveChanges();
        }

    }
}