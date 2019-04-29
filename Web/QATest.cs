using System;

namespace SprintCrowd.Backend.Web
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using System;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using SprintCrowdBackEnd.Application;
    using SprintCrowdBackEnd.Infrastructure.Persistence.Entities;
    using SprintCrowdBackEnd.Infrastructure.Persistence;

    [Route("[controller]")]
    public class QATest : ControllerBase
    {
        public QATest(ScrowdDbContext context)
        {
            this.context = context;
        }

        private ScrowdDbContext context;

        [HttpGet("user")]
        public async Task<List<User>> GetUsers() => this.context.User.ToList();

        [HttpPost("user/generate/")]
        [AllowAnonymous]
        public async Task<User> GenerateUser()
        {
            string username = QATestHelper.RandomString();
            AccessToken accessToken = new AccessToken()
            {
                Token = QATestHelper.RandomString()
            };

            User user = new User()
            {
                UserType = (int)UserType.Facebook,
                FacebookUserId = QATestHelper.RandomString(),
                Email = username + "@example.com",
                Name = username,
                ProfilePicture = "https://www.hindustantimes.com/rf/image_size_960x540/HT/p2/2017/04/09/Pictures/sunny-leone_4d74badc-1cf6-11e7-aa2a-1591876ff7cf.jpg",
                AccessToken = accessToken,
            };
            this.context.User.Add(user);
            this.context.SaveChanges();
            return this.context.User.FirstOrDefault(d => d.Name == username);
        }

        [HttpPost("sprint/generate")]
        public IActionResult GenerateSprint()
        {
            string name = QATestHelper.RandomString();

            Sprint sprint = new Sprint()
            {
                Name = name,
                Distance = 300,
                StartDateTime = DateTime.UtcNow.AddMinutes(1),
            };
            this.context.Sprint.Add(sprint);
            this.context.SaveChanges();
            return this.Ok(this.context.Sprint.FirstOrDefault(d => d.Name == name));
        }

        [HttpGet("sprint/{sprintId:int}")]
        public IActionResult GetSprint(int sprintId) =>
            this.Ok(this.context.Sprint.FirstOrDefault(d => d.Id == sprintId));

        [HttpGet("sprint")]

        [HttpPut("sprint/update/{sprintId:int}")]
        public IActionResult UpdateSprint([FromBody] Sprint sprint, int sprintId)
        {
            Sprint exSprint = this.context.Sprint.FirstOrDefault(d => d.Id == sprintId);
            if (exSprint != null && !exSprint.Equals(sprint) && exSprint.Id == sprint.Id)
            {
                this.context.Entry(exSprint).CurrentValues.SetValues(sprint);
                this.context.Update(exSprint);
            }
            this.context.SaveChanges();
            return this.Ok();
        }

        [HttpPost("sprint/update/time/{sprintId:int}")]
        public IActionResult UpdateStartTime([FromBody] SprintStartTime startTime, int sprintId)
        {
            Sprint exSprint = this.context.Sprint.FirstOrDefault(d => d.Id == sprintId);
            exSprint.StartDateTime = DateTime.UtcNow.AddMinutes(startTime.StartFrom);
            this.context.Update(exSprint);
            this.context.SaveChanges();
            return this.Ok("success");
        }

        [HttpPost("sprint/pariticipant/{sprintId:int}/{userId:int}")]
        public IActionResult AddSprintParticipant(int sprintId, int userId)
        {
            User user = this.context.User.Where(d => d.Id == userId).FirstOrDefault();
            Sprint sprint = this.context.Sprint.FirstOrDefault(d => d.Id == sprintId);
            if (sprint.Participants == null)
            {
                sprint.Participants = new List<SprintParticipant>();
            }
            SprintParticipant sp = new SprintParticipant()
            {
                User = user
            };
            sprint.Participants.Add(sp);
            this.context.Sprint.Update(sprint);
            this.context.SaveChanges();
            return this.Ok();
        }
    }
}

public static class QATestHelper
{
    public static string RandomString()
    {
        Random rnd = new Random();
        int length = 20;
        var str = string.Empty;
        for (var i = 0; i < length; i++)
        {
            str += ((char)(rnd.Next(1, 26) + 64)).ToString();
        }
        return str.ToLower();
    }
}

public class SprintStartTime
{
    public int StartFrom { get; set; }
}