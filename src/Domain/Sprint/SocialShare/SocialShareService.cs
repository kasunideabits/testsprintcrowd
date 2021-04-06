namespace SprintCrowd.BackEnd.Domain.SocialShare
{
    using System.Threading.Tasks;
    using SprintCrowd.BackEnd.Web.SocialShare;
    using System;
    using Newtonsoft.Json.Linq;
    using RestSharp;

    /// <summary>
    /// SocialShareService
    /// </summary>
    public class SocialShareService : ISocialShareService
    {
        private const string GetSocialApiKey = "2d76dac9a47cc768620bdbb79d0080b7";
        private const string AppId = "n7F0jZQh89j";

        /// <summary>
        /// GetSmartLink
        /// </summary>
        public async Task<string> GetSmartLink(SocialLink socialLink)
        {

            RestClient restClient = new RestClient("https://api.getsocial.im/v1/smart-links");
            var guid = Guid.NewGuid().ToString().Replace("-", string.Empty);
            var strToken = System.Text.RegularExpressions.Regex.Replace(guid, "[a-zA-Z]", string.Empty).Substring(0, 12);

            RestRequest request = new RestRequest(Method.POST);
            request.AddHeader("Accept", "application/json");
            request.AddHeader("X-GetSocial-API-Key", GetSocialApiKey);
            request.AddJsonBody(new { app_id = AppId, name = socialLink.Name, token = strToken, channel = "email", medium = "mail", campaign_name = "testCampaign", image = socialLink.ImageUrl });

            Console.WriteLine("Before GetDeepLink 1 post call ");
            try
            {
                var response = restClient.Execute(request, Method.POST);
                dynamic data = JObject.Parse(response.Content);
                Console.WriteLine("GetDeepLink 1 after POST call");
                return Convert.ToString(data["url"]);
            }
            catch (Exception ex)
            {
                return null;
            }

        }
    }
}