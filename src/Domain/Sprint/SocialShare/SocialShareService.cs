namespace SprintCrowd.BackEnd.Domain.SocialShare
{
    using System.Threading.Tasks;
    using SprintCrowd.BackEnd.Web.SocialShare;
    using System;
    using Newtonsoft.Json.Linq;
    using RestSharp;
    using Serilog;

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
            request.AddJsonBody(new
            {
                app_id = AppId,
                name = socialLink.Name,
                token = strToken,
                channel = "email",
                medium = "mail",
                title = new { en = socialLink.Name },
                description = String.IsNullOrEmpty(socialLink.Description) ? null : new { en = socialLink.Description },
                campaign_name = "testCampaign",
                image = socialLink.ImageUrl,
                custom_data = socialLink.CustomData
            });

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
                Log.Logger.Error($" getSmartLink - {ex}");
                return null;
            }

        }

        /// <summary>
        /// GetSmartLink
        /// </summary>
        public async Task<string> GetInvitionLink(string token, object customdata)
        {

            RestClient restClient = new RestClient("https://api.getsocial.im/v1/smart-invites");
            var guid = Guid.NewGuid().ToString().Replace("-", string.Empty);
            var strToken = System.Text.RegularExpressions.Regex.Replace(guid, "[a-zA-Z]", string.Empty).Substring(0, 12);

            RestRequest request = new RestRequest(Method.POST);
            request.AddHeader("Accept", "application/json");
            request.AddHeader("X-GetSocial-Access-Token", token);
            request.AddJsonBody(new { app_id = AppId, channel = "facebook", custom_data = customdata });
            Console.WriteLine("Before getInvitation 1 post call ");
            try
            {
                var response = restClient.Execute(request, Method.POST);
                dynamic data = JObject.Parse(response.Content);
                Console.WriteLine("Get invite link 1 after POST call");
                return Convert.ToString(data["url"]);
            }
            catch (Exception ex)
            {
                Log.Logger.Error($" getInvitationLink - {ex}");
                return null;
            }

        }


        /// <summary>
        /// GetSmartLink
        /// </summary>
        public async Task<string> GetToken()
        {

            RestClient restClient = new RestClient("https://api.getsocial.im/v1/authenticate/user");

            RestRequest request = new RestRequest(Method.POST);
            request.AddQueryParameter("app_id", AppId);
            request.AddQueryParameter("identity_type", "email");
            request.AddQueryParameter("value", "scrowd@ideabits.se");
            request.AddQueryParameter("token", GetSocialApiKey);
            Console.WriteLine("Before getToken");
            try
            {
                var response = restClient.Execute(request, Method.POST);
                dynamic data = JObject.Parse(response.Content);
                Console.WriteLine("GetDeepLink 1 after POST call");
                return Convert.ToString(data["access_token"]);
            }
            catch (Exception ex)
            {
                Log.Logger.Error($" get Invitation Link Token - {ex}");
                return null;
            }
            // return "test";

        }

        /// <summary>
        /// GetSmartLink
        /// </summary>
        public async Task<string> updateTokenAndGetInvite(object customdata)
        {

            var newtoken = await this.GetToken();
            return await this.GetInvitionLink(newtoken, customdata);

        }

    }
}