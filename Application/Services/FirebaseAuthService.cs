using FirebaseAdmin.Auth;
using SMT.Application.Services;

namespace SMT.Application.Services
{
    public class FirebaseAuthService : IFirebaseAuthService
{
    public async Task<FirebaseToken> VerifyTokenAsync(string idToken)
    {
        if (string.IsNullOrWhiteSpace(idToken))
            throw new ArgumentException("Token is null or empty");

        var decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(idToken);
        return decodedToken;
    }
}
}