using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Web.Models;

//using Think

namespace SocialNetwork.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        [Authorize]
        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44325/connect/token");
            var content = new FormUrlEncodedContent(new []
            {
                new KeyValuePair<string, string>("grant_type", "client_credentials"),
                new KeyValuePair<string, string>("grant_type", "client_credentials"),
            });
            
            //var client = new OAuth2Client(new Uri("https://localhost:44325/connect/token"),
            //    "resourceOwner", "superSecretPassword");

            //var requestResponse = client.RequestAccessTokenUserName(username, password,
            //    "openid profile offline_access");

            //var claims = new[]
            //{
            //    new Claim("access_token", requestResponse.AccessToken),
            //    new Claim("refresh_token", requestResponse.RefreshToken)
            //};

            //var claimsIdentity = new ClaimsIdentity(claims, "ApplicationCookie");

            ////HttpContext.GetOwinContext().Authentication.SignIn(claimsIdentity);

            //return Redirect("/private");
        }
    }
}
