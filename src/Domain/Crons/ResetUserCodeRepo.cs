namespace SprintCrowd.BackEnd.Domain.Crons
{
  using SprintCrowd.BackEnd.Application;
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading.Tasks;
  using System;
  using Microsoft.EntityFrameworkCore;
  using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;
  using SprintCrowd.BackEnd.Infrastructure.Persistence;

  /// <summary>
  ///  Implement <see cref="IResetUserCodeRepo" > interface </see>
  /// </summary>
  public class ResetUserCodeRepo : IResetUserCodeRepo
  {
    private ScrowdDbContext dbContext;

    /// <summary>
    /// Initialize <see cref="ResetUserCodeRepo"> class </see>
    /// </summary>
    /// <param name="dbContext">database context</param>
    public ResetUserCodeRepo(ScrowdDbContext dbContext)
    {
      this.dbContext = dbContext;
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

    /// <summary>
    /// Get all users
    /// </summary>
    public async Task<List<User>> GetAllUsers()
    {
      List<User> users = await this.dbContext.User.ToListAsync();
      return users;
    }

    /// <summary>
    /// Reset code of all users
    /// </summary>
    public async Task<User> ResetUserCode(User user)
    {
      var code = SCrowdUniqueKey.GetUniqueKey();
      var codeExist = await this.dbContext.User.FirstOrDefaultAsync(u => u.Code.Equals(code));

      while (codeExist != null)
      {
        code = SCrowdUniqueKey.GetUniqueKey();
        codeExist = await this.dbContext.User.FirstOrDefaultAsync(u => u.Code.Equals(code));
      }
      user.Code = code;
      return user;
    }

    /// <summary>
    /// Update single user
    /// </summary>
    public async Task<User> UpdateUserContext(User user)
    {
      var result = this.dbContext.User.Update(user);
            this.dbContext.SaveChanges();
      return result.Entity;
    }
  }
}