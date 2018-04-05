﻿using System.Web.Mvc;
using SpotifyAPI.Web.Auth;
using SpotifyAPI.Web.Models;
using web.Services;
using static web.Models.CustomToken;

namespace web.Controllers
{
    public class CallbackController : Controller
    {
        private readonly ITokenCookieService _tokenService;
        private readonly IUserService _userService;
        private readonly ISessionService _sessionService;

        public CallbackController(ITokenCookieService tokenService, IUserService userService, ISessionService sessionService)
        {
            _tokenService = tokenService;
            _userService = userService;
            _sessionService = sessionService;
        }

        public ActionResult Index(string code = null)
        {
            AutorizationCodeAuth auth = new AutorizationCodeAuth
            {
                ClientId = Constants.ClientId,
                RedirectUri = Constants.RedirectUri,
                State = Constants.StateKey
            };

            Token token = auth.ExchangeAuthCode(code, Constants.ClientSecret);

            _tokenService.SetToken(token.AccessToken, token.RefreshToken, token.ExpiresIn, TokenCredentialType.Auth);
            _sessionService.SetToken(token.ToCustomToken(TokenCredentialType.Auth));

            var returnUrl = _sessionService.GetReturnUrl();

            if (returnUrl != null && !string.IsNullOrEmpty(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return Redirect("~/");
        }
    }
}