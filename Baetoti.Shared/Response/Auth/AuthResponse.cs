namespace Baetoti.Shared.Response.Auth
{
    public class AuthResponse
    {
        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }

        public string ExpiresAt { get; set; }

        public string Name { get; set; }

    }
}
