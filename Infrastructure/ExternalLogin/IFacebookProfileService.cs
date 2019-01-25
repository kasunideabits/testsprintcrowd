namespace SprintCrowd.Backend.Infrastructure.ExternalLogin
{
    using System.Threading.Tasks;

    public interface IFacebookProfileService
    {
        Task<FacebookProfile> GetProfile(string accessToken);
    }
}