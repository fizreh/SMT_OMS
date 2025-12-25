using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SMT.Application.Services;
using System.Security.Claims;
using System.Text.Encodings.Web;


namespace API.Authentication
{
    public class FirebaseAuthenticationHandler
    : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IFirebaseAuthService _firebaseAuth;

        public FirebaseAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            IFirebaseAuthService firebaseAuth)
            : base(options, logger, encoder)
        {
            _firebaseAuth = firebaseAuth;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
                return AuthenticateResult.Fail("Missing Authorization Header");

            var header = Request.Headers["Authorization"].ToString();
            Logger.LogInformation("Authorization header received: {AuthHeader}", header);

            if (!header.StartsWith("Bearer "))
                return AuthenticateResult.Fail("Invalid Authorization Header");

            var token = header.Replace("Bearer ", "");

            try
            {
                var decodedToken = await _firebaseAuth.VerifyTokenAsync(token);

                if (string.IsNullOrEmpty(decodedToken.Uid))
                {
                    Logger.LogError("Firebase UID is missing");
                    return AuthenticateResult.Fail("Firebase UID missing");
                }

                var claims = decodedToken.Claims.Select(c =>
                    new Claim(c.Key, c.Value.ToString()!)
                ).ToList();

                claims.Add(new Claim(ClaimTypes.NameIdentifier, decodedToken.Uid));

                var identity = new ClaimsIdentity(claims, Scheme.Name);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);

                Logger.LogInformation("Firebase authentication succeeded with token {Token} and ticket {Ticket}",token, ticket);

                return AuthenticateResult.Success(ticket);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Firebase authentication failed");
                return AuthenticateResult.Fail("Invalid Firebase token");
            }
        }
    }
}
