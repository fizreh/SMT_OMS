using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;

namespace SMT.API.Configuration
{
    public static class FirebaseConfig
    {
        public static void InitializeFirebase()
        {
            if (FirebaseApp.DefaultInstance == null)
            {
                FirebaseApp.Create(new AppOptions()
                {
                    Credential = GoogleCredential.FromFile("Config/firebase-service.json")
                });
            }
        }
    }
}