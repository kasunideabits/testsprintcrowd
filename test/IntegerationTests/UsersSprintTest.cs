namespace Tests
{
    using System;
    using System.Net;
    using System.Net.Http;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;
    using Tests.Helpers;
    using Xunit;

    [Collection("Sequential")]
    public class UsersSprintTest
    {
        private readonly HttpClient _httpClient;

        public UsersSprintTest()
        {
            this._httpClient = HttpServerClient.CreateServerClient();
        }


        /// <summary>
        /// should successfully return currently joined users of a given sprint
        /// </summary>
        /// <returns></returns>

        [Fact]
        public async void GetJoinedUsersTest()
        {
            User sam = new User();
            sam.Id = 2;
            sam.UserType = 1;
            sam.FacebookUserId = "1425376969546751";
            sam.Email = "sam@gmail.com";
            sam.Name = "Sam";

            User john = new User();
            john.Id = 4;
            john.UserType = 1;
            john.FacebookUserId = "1422346969546751";
            john.Email = "john@gmail.com";
            john.Name = "John";

            User kale = new User();
            kale.Id = 5;
            kale.UserType = 1;
            kale.FacebookUserId = "1422354369546751";
            kale.Email = "kale@gmail.com";
            kale.Name = "Kale";

            User max = new User();
            max.Id = 6;
            max.UserType = 1;
            max.FacebookUserId = "5622354369546751";
            max.Email = "max@gmail.com";
            max.Name = "max";

            await TestStartUp.DbContext.User.AddAsync(sam);
            await TestStartUp.DbContext.User.AddAsync(john);
            await TestStartUp.DbContext.User.AddAsync(kale);
            await TestStartUp.DbContext.User.AddAsync(max);

            TestStartUp.DbContext.SaveChanges();

            Sprint sprint1 = new Sprint();
            sprint1.Name = "sprint1";
            sprint1.Distance = 1000;
            sprint1.CreatedDate = DateTime.UtcNow;
            sprint1.Type = 0;
            sprint1.Status = 0;

            Sprint sprint2 = new Sprint();
            sprint2.Name = "sprint2";
            sprint2.Distance = 1500;
            sprint2.CreatedDate = DateTime.UtcNow;
            sprint2.Type = 0;
            sprint2.Status = 0;

            Sprint sprint3 = new Sprint();
            sprint3.Name = "sprint3";
            sprint3.Distance = 2000;
            sprint3.CreatedDate = DateTime.UtcNow;
            sprint3.Type = 0;
            sprint3.Status = 0;

            Sprint sprint4 = new Sprint();
            sprint4.Name = "sprint4";
            sprint4.Distance = 3000;
            sprint4.CreatedDate = DateTime.UtcNow;
            sprint4.Type = 0;
            sprint4.Status = 0;

            await TestStartUp.DbContext.Sprint.AddAsync(sprint1);
            await TestStartUp.DbContext.Sprint.AddAsync(sprint2);
            await TestStartUp.DbContext.Sprint.AddAsync(sprint3);
            await TestStartUp.DbContext.Sprint.AddAsync(sprint4);

            TestStartUp.DbContext.SaveChanges();

            SprintParticipant sprintParticipant = new SprintParticipant();
            sprintParticipant.User = sam;
            sprintParticipant.Stage = 1;
            sprintParticipant.Sprint = sprint1;

            SprintParticipant sprintParticipant2 = new SprintParticipant();
            sprintParticipant2.User = john;
            sprintParticipant2.Stage = 1;
            sprintParticipant2.Sprint = sprint1;

            SprintParticipant sprintParticipant3 = new SprintParticipant();
            sprintParticipant3.User = kale;
            sprintParticipant3.Stage = 1;
            sprintParticipant3.Sprint = sprint1;

            SprintParticipant sprintParticipant4 = new SprintParticipant();
            sprintParticipant4.User = kale;
            sprintParticipant4.Stage = 1;
            sprintParticipant4.Sprint = sprint1;

            SprintParticipant sprintParticipant5 = new SprintParticipant();
            sprintParticipant5.User = max;
            sprintParticipant5.Stage = 1;
            sprintParticipant5.Sprint = sprint2;

            SprintParticipant sprintParticipant6 = new SprintParticipant();
            sprintParticipant6.User = max;
            sprintParticipant6.Stage = 1;
            sprintParticipant6.Sprint = sprint3;

            await TestStartUp.DbContext.SprintParticipant.AddAsync(sprintParticipant);
            await TestStartUp.DbContext.SprintParticipant.AddAsync(sprintParticipant2);
            await TestStartUp.DbContext.SprintParticipant.AddAsync(sprintParticipant3);
            await TestStartUp.DbContext.SprintParticipant.AddAsync(sprintParticipant4);
            await TestStartUp.DbContext.SprintParticipant.AddAsync(sprintParticipant5);
            await TestStartUp.DbContext.SprintParticipant.AddAsync(sprintParticipant6);

            TestStartUp.DbContext.SaveChanges();

            int sprintId = sprint1.Id;
            int sprintType = 0;
            int offset = 0;
            int fetch = 3;
            System.Console.WriteLine("/userssprint/getusers?" + "sprint_id=" + sprintId + "&sprint_type=" + sprintType + "&offset=" + offset + "&fetch=" + fetch);
            HttpResponseMessage response = await _httpClient.GetAsync("/userssprint/getusers?" + "sprint_id=" + sprintId + "&sprint_type=" + sprintType + "&offset=" + offset + "&fetch=" + fetch);
            string strResponse = await response.Content.ReadAsStringAsync();
            dynamic resObject = JsonConvert.DeserializeObject(strResponse);

            System.Console.WriteLine(resObject);
            //Assert.True(1 == ((JArray)["Data"].Count));
            Assert.True(1 == (int)resObject.Data[0].SprintId);
            //Assert.True(3 == (int)resObject.Data.length);
            //Assert.True(1 == (int)resObject.Data.SprintId);
        }

    }
}
