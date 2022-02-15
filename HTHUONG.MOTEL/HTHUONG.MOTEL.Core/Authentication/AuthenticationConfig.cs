using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ThirdParty.BouncyCastle.OpenSsl;

namespace HTHUONG.MOTEL.Core.Authentication
{
    public class AuthenticationConfig
    {
        private readonly ILogger<AuthenticationConfig> _logger;
        private readonly IWebHostEnvironment _host;

        public AuthenticationConfig(
            IWebHostEnvironment host,
            ILogger<AuthenticationConfig> logger)
        {
            _host = host;
            _logger = logger;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GenerateAccessToken(string fullName, string email)
        {
            string privateKey = File.ReadAllText(_host.WebRootPath + @"/content/RSA256/privateKey.pem");
            var claims = new List<Claim>();
            claims.Add(new Claim("email", email));
            claims.Add(new Claim("full-name", fullName));
            var token = CreateAccessToken(claims, privateKey);
            return token;
        }

        /// <summary>
        /// Tạo access token để gửi cho ứng dụng khác
        /// </summary>
        /// <param name="claims">Thông tin cần có trong token</param>
        /// <param name="privateRsaKey">Khóa bí mật của thuật toán RSA sinh ra bằng openssl</param>
        /// <returns>Access token</returns>
        public string CreateAccessToken(List<Claim> claims, string privateRsaKey)
        {
            try
            {
                RSAParameters rsaParams;
                using (var tr = new StringReader(privateRsaKey))
                {
                    var pemReader = new Org.BouncyCastle.OpenSsl.PemReader(tr);
                    var keyPair = pemReader.ReadObject() as AsymmetricCipherKeyPair;
                    if (keyPair == null)
                    {
                        throw new Exception("Could not read RSA private key");
                    }
                    var privateRsaParams = keyPair.Private as RsaPrivateCrtKeyParameters;
                    rsaParams = DotNetUtilities.ToRSAParameters(privateRsaParams);
                }
                using RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                rsa.ImportParameters(rsaParams);
                Dictionary<string, object> payload = claims.ToDictionary(k => k.Type, v => (object)v.Value);
                payload.Add("exp", ((DateTimeOffset)DateTime.Now.AddDays(1)).ToUnixTimeSeconds());
                payload.Add("nbf", ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds());
                return Jose.JWT.Encode(payload, rsa, Jose.JwsAlgorithm.RS256);
            }
            catch (Exception ex)
            {
                _logger.LogError("Email exception in /AuthenticationConfig/CreateAccessToken: " + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Giải mã access token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public string DecodeAccessToken(string token)
        {
            try
            {
                string publicRsaKey = File.ReadAllText(_host.WebRootPath + @"/content/RSA256/publicKey.pem");
                RSAParameters rsaParams;

                using (var tr = new StringReader(publicRsaKey))
                {
                    var pemReader = new Org.BouncyCastle.OpenSsl.PemReader(tr);
                    var publicKeyParams = pemReader.ReadObject() as RsaKeyParameters;
                    if (publicKeyParams == null)
                    {
                        throw new Exception("Could not read RSA public key");
                    }
                    rsaParams = DotNetUtilities.ToRSAParameters(publicKeyParams);
                }
                using RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                rsa.ImportParameters(rsaParams);
                // This will throw if the signature is invalid
                return Jose.JWT.Decode(token, rsa, Jose.JwsAlgorithm.RS256);
            }
            catch (Exception ex)
            {
                _logger.LogError("Email exception in /AuthenticationConfig/DecodeAccessToken: " + ex.ToString());
                return null;
            }
        }

    }
}
