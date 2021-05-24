using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Otus.Signalr.Hubs;
using Otus.Signalr.Models;
using Otus.Signalr.Requests;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.Text;

namespace Otus.Signalr.Controllers
{
    public class AuthOptions
    {
        public const string ISSUER = "ISSUER"; // издатель токена
        public const string AUDIENCE = "AUDIENCE"; // потребитель токена
        const string KEY = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";   // ключ для шифрации
        public const int LIFETIME = 10; // время жизни токена - 10 минут
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
     => new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
    }


    [ApiController]
    [Route("/api/chat")]
    public class ChatController : ControllerBase
    {
        private readonly IHubContext<ChatHub, IChatClient> _chatHub;

        public ChatController(IHubContext<ChatHub, IChatClient> chatHub)
        {
            _chatHub = chatHub;
        }

        [HttpPost("login")]
        public string Login([FromBody] LoginRequest request)
        {
            var claims = new List<Claim> {
                new Claim(ClaimsIdentity.DefaultNameClaimType, request.Login),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, request.Login)
             };

            var claimsIdenty = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);


            var now = DateTime.UtcNow;
            // создаем JWT-токен
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: claimsIdenty.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return encodedJwt;

        }


    }
}