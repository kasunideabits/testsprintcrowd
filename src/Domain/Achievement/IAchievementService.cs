namespace SprintCrowd.BackEnd.Domain.Achievement
{
    using System.Threading.Tasks;

    public interface IAchievementService
    {
        Task SignUp(int userId);
    }
}