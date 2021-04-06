namespace SprintCrowd.BackEnd.Domain.SocialShare
{
    using System.Threading.Tasks;
    using SprintCrowd.BackEnd.Web.SocialShare;
    public interface ISocialShareService
    {
        Task<string> GetSmartLink(SocialLink socialLink);
        Task<string> GetInvitionLink(string token, object customdata);
        Task<string> GetToken();
        Task<string> updateTokenAndGetInvite(object json);
    }
}