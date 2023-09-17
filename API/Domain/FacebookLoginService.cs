using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using API.Dtos;
using API.Provider;

namespace API.Domain
{
    public class FacebookLoginService
    {
        private readonly  string ClientID = "629483562400919";
        private readonly string ClientSecret = "df041939538481f9207c1b35127597a2";
        private readonly  string LoginUrl = "https://www.facebook.com/v17.0/dialog/oauth?client_id={0}&redirect_uri={1}&state={2}&response_type={3}&scope={4}";

        private readonly string tokenUrl = "https://graph.facebook.com/v17.0/oauth/access_token";
        private readonly string ProfileUrl ="https://graph.facebook.com/me?fields=id,name,email,picture&access_token={0}";

        private static HttpClient client = new HttpClient();

        private readonly JsonProvider _jsonProvider = new JsonProvider();

        public string GetLoginUrl(string redirect_url)
        {
            var state = "ooxx";
            var scope = "public_profile";
            var uri = String.Format(LoginUrl,ClientID,redirect_url,state,"code",scope);
            return uri;
        }

            public async Task<FacebookTokenDto> GetTokenByAuthToken(string authToken, string callBackUrl)
            {
                var formContent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string,string>("code",authToken),
                    new KeyValuePair<string,string>("redirect_uri",callBackUrl),
                    new KeyValuePair<string,string>("client_id",ClientID),
                    new KeyValuePair<string,string>("client_secret",ClientSecret),

                });

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = await client.PostAsync(tokenUrl,formContent);
                var dto = _jsonProvider.Deserialize<FacebookTokenDto>(await response.Content.ReadAsStringAsync());

                return dto;
            }


        public async Task<UserProfileDto> GetUserProfileByAccessToken(string accessToken)
        {
            // client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",accessToken);

            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(String.Format(ProfileUrl,accessToken))
            };

            var response = await client.SendAsync(request);
            var profile = _jsonProvider.Deserialize<UserProfileDto>(await response.Content.ReadAsStringAsync());

            return profile;
        }

    }
}