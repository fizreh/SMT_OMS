using FirebaseAdmin.Auth;

namespace SMT.Application.Services
{
    public interface IFirebaseAuthService
    {
        Task<FirebaseToken> VerifyTokenAsync(string idToken);
    }
}