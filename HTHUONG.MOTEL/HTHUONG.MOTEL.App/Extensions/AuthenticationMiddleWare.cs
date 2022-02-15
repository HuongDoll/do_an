using HTHUONG.MOTEL.Core.Authentication;
using HTHUONG.MOTEL.Core.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using Nancy.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HTHUONG.MOTEL.App.Extensions
{
    public class AuthenticationMiddleWare
    {
        private readonly RequestDelegate _next;
        private readonly AuthenticationConfig _authenticationConfig;

        public AuthenticationMiddleWare(AuthenticationConfig authenticationConfig, RequestDelegate next)
        {
            _next = next;
            _authenticationConfig = authenticationConfig;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var headers = context.Request.Headers;
            StringValues bearerToken = new StringValues();
            headers.TryGetValue("Authorization", out bearerToken);

            try
            {
                if (bearerToken.Count > 0)
                {
                    var accessTokens = bearerToken.ToString().Split(' ').ToList();
                    if (accessTokens.Count > 1)
                    {
                        var accessToken = accessTokens[1];
                        var decodedToken = _authenticationConfig.DecodeAccessToken(accessToken);
                        if (decodedToken != null)
                        {
                            var json_serializer = new JavaScriptSerializer();
                            var claims = (IDictionary<string, object>)json_serializer.DeserializeObject(decodedToken);
                            var expiredTimeStamp = Convert.ToInt64(claims["exp"].ToString());
                            var now = ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds();
                            if (expiredTimeStamp < now)
                            {
                                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                                return;
                            }
                            context.Request.Headers.Add(HeaderKey.FULL_NAME, claims["full-name"].ToString());
                            context.Request.Headers.Add(HeaderKey.EMAIL, claims["email"].ToString());
                        }
                        //context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        //return;
                    }
                }
            }
            catch (SecurityTokenException ex)
            {
                Console.WriteLine(ex.ToString());
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }

            await _next(context);
        }
    }
}
