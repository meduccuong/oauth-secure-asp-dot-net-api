using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace SocialNetwork.Web.Models
{
    public class OAuth2Client
    {
        private HttpClient _client;

        public OAuth2Client(Uri address)
        {
            this._client = new HttpClient()
            {
                BaseAddress = address
            };
        }

        public OAuth2Client(Uri address, string clientId, string clientSecret)
          : this(address)
        {
            this._client.DefaultRequestHeaders.Authorization = (AuthenticationHeaderValue)new BasicAuthenticationHeaderValue(clientId, clientSecret);
        }

        public static string CreateCodeFlowUrl(string endpoint, string clientId, string scope, string redirectUri, string state = null)
        {
            return OAuth2Client.CreateUrl(endpoint, clientId, scope, redirectUri, "code", state);
        }

        public static string CreateImplicitFlowUrl(string endpoint, string clientId, string scope, string redirectUri, string state = null)
        {
            return OAuth2Client.CreateUrl(endpoint, clientId, scope, redirectUri, "token", state);
        }

        private static string CreateUrl(string endpoint, string clientId, string scope, string redirectUri, string responseType, string state = null)
        {
            string str = string.Format("{0}?client_id={1}&scope={2}&redirect_uri={3}&response_type={4}", (object)endpoint, (object)clientId, (object)scope, (object)redirectUri, (object)responseType);
            if (!string.IsNullOrWhiteSpace(state))
                str = string.Format("{0}&state={1}", (object)str, (object)state);
            return str;
        }

        public AccessTokenResponse RequestAccessTokenUserName(string userName, string password, string scope, Dictionary<string, string> additionalProperties = null)
        {
            HttpResponseMessage result = this._client.PostAsync("", (HttpContent)this.CreateFormUserName(userName, password, scope, additionalProperties)).Result;
            result.EnsureSuccessStatusCode();
            return this.CreateResponseFromJson(JObject.Parse(result.Content.ReadAsStringAsync().Result));
        }

        public AccessTokenResponse RequestAccessTokenClientCredentials(string scope, Dictionary<string, string> additionalProperties = null)
        {
            HttpResponseMessage result = this._client.PostAsync("", (HttpContent)this.CreateFormClientCredentials(scope, additionalProperties)).Result;
            result.EnsureSuccessStatusCode();
            return this.CreateResponseFromJson(JObject.Parse(result.Content.ReadAsStringAsync().Result));
        }

        public AccessTokenResponse RequestAccessTokenRefreshToken(string refreshToken, Dictionary<string, string> additionalProperties = null)
        {
            HttpResponseMessage result = this._client.PostAsync("", (HttpContent)this.CreateFormRefreshToken(refreshToken, additionalProperties)).Result;
            result.EnsureSuccessStatusCode();
            return this.CreateResponseFromJson(JObject.Parse(result.Content.ReadAsStringAsync().Result));
        }

        public AccessTokenResponse RequestAccessTokenCode(string code, Dictionary<string, string> additionalProperties = null)
        {
            HttpResponseMessage result = this._client.PostAsync("", (HttpContent)this.CreateFormCode(code, additionalProperties)).Result;
            result.EnsureSuccessStatusCode();
            return this.CreateResponseFromJson(JObject.Parse(result.Content.ReadAsStringAsync().Result));
        }

        public AccessTokenResponse RequestAccessTokenCode(string code, Uri redirectUri, Dictionary<string, string> additionalProperties = null)
        {
            HttpResponseMessage result = this._client.PostAsync("", (HttpContent)this.CreateFormCode(code, redirectUri, additionalProperties)).Result;
            result.EnsureSuccessStatusCode();
            return this.CreateResponseFromJson(JObject.Parse(result.Content.ReadAsStringAsync().Result));
        }

        public AccessTokenResponse RequestAccessTokenAssertion(string assertion, string assertionType, string scope, Dictionary<string, string> additionalProperties = null)
        {
            HttpResponseMessage result = this._client.PostAsync("", (HttpContent)this.CreateFormAssertion(assertion, assertionType, scope, additionalProperties)).Result;
            result.EnsureSuccessStatusCode();
            return this.CreateResponseFromJson(JObject.Parse(result.Content.ReadAsStringAsync().Result));
        }

        protected virtual FormUrlEncodedContent CreateFormClientCredentials(string scope, Dictionary<string, string> additionalProperties = null)
        {
            return OAuth2Client.CreateForm(new Dictionary<string, string>()
      {
        {
          "grant_type",
          "client_credentials"
        },
        {
          "scope",
          scope
        }
      }, additionalProperties);
        }

        protected virtual FormUrlEncodedContent CreateFormUserName(string userName, string password, string scope, Dictionary<string, string> additionalProperties = null)
        {
            return OAuth2Client.CreateForm(new Dictionary<string, string>()
      {
        {
          "grant_type",
          "password"
        },
        {
          "username",
          userName
        },
        {
          "password",
          password
        },
        {
          "scope",
          scope
        }
      }, additionalProperties);
        }

        protected virtual FormUrlEncodedContent CreateFormRefreshToken(string refreshToken, Dictionary<string, string> additionalProperties = null)
        {
            return OAuth2Client.CreateForm(new Dictionary<string, string>()
      {
        {
          "grant_type",
          "refresh_token"
        },
        {
          "refresh_token",
          refreshToken
        }
      }, additionalProperties);
        }

        protected virtual FormUrlEncodedContent CreateFormCode(string code, Dictionary<string, string> additionalProperties = null)
        {
            return OAuth2Client.CreateForm(new Dictionary<string, string>()
      {
        {
          "grant_type",
          "authorization_code"
        },
        {
          "code",
          code
        }
      }, additionalProperties);
        }

        protected virtual FormUrlEncodedContent CreateFormCode(string code, Uri redirectUri, Dictionary<string, string> additionalProperties = null)
        {
            return OAuth2Client.CreateForm(new Dictionary<string, string>()
      {
        {
          "grant_type",
          "authorization_code"
        },
        {
          "redirect_uri",
          redirectUri.AbsoluteUri
        },
        {
          "code",
          code
        }
      }, additionalProperties);
        }

        protected virtual FormUrlEncodedContent CreateFormAssertion(string assertion, string assertionType, string scope, Dictionary<string, string> additionalProperties = null)
        {
            return OAuth2Client.CreateForm(new Dictionary<string, string>()
      {
        {
          "grant_type",
          assertionType
        },
        {
          "assertion",
          assertion
        },
        {
          "scope",
          scope
        }
      }, additionalProperties);
        }

        private AccessTokenResponse CreateResponseFromJson(JObject json)
        {
            AccessTokenResponse accessTokenResponse = new AccessTokenResponse()
            {
                AccessToken = json["access_token"].ToString(),
                TokenType = json["token_type"].ToString(),
                ExpiresIn = int.Parse(json["expires_in"].ToString())
            };
            if (json["refresh_token"] != null)
                accessTokenResponse.RefreshToken = json["refresh_token"].ToString();
            return accessTokenResponse;
        }

        /// <summary>
        /// FormUrlEncodes both Sets of Key Value Pairs into one form object
        /// </summary>
        /// <param name="explicitProperties"></param>
        /// <param name="additionalProperties"></param>
        /// <returns></returns>
        private static FormUrlEncodedContent CreateForm(Dictionary<string, string> explicitProperties, Dictionary<string, string> additionalProperties = null)
        {
            return new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)OAuth2Client.MergeAdditionKeyValuePairsIntoExplicitKeyValuePairs(explicitProperties, additionalProperties));
        }

        /// <summary>
        /// Merges additional into explicit properties keeping all explicit properties intact
        /// </summary>
        /// <param name="explicitProperties"></param>
        /// <param name="additionalProperties"></param>
        /// <returns></returns>
        private static Dictionary<string, string> MergeAdditionKeyValuePairsIntoExplicitKeyValuePairs(Dictionary<string, string> explicitProperties, Dictionary<string, string> additionalProperties = null)
        {
            Dictionary<string, string> dictionary = explicitProperties;
            if (additionalProperties != null)
                dictionary = explicitProperties.Concat<KeyValuePair<string, string>>(additionalProperties.Where<KeyValuePair<string, string>>((Func<KeyValuePair<string, string>, bool>)(add => !explicitProperties.ContainsKey(add.Key)))).ToDictionary<KeyValuePair<string, string>, string, string>((Func<KeyValuePair<string, string>, string>)(final => final.Key), (Func<KeyValuePair<string, string>, string>)(final => final.Value));
            return dictionary;
        }
    }
}