using System;
using System.Net.Http.Headers;
using System.Text;

namespace SocialNetwork.Web.Models
{
    public class BasicAuthenticationHeaderValue : AuthenticationHeaderValue
    {
        public BasicAuthenticationHeaderValue(string userName, string password)
          : base("Basic", BasicAuthenticationHeaderValue.EncodeCredential(userName, password))
        {
        }

        private static string EncodeCredential(string userName, string password)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("{0}:{1}", (object)userName, (object)password)));
        }
    }
}