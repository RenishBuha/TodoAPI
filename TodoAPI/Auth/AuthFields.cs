namespace TodoAPI.Auth
{
    /// <summary>
    /// Move to app settings and use from configuration
    /// </summary>
    public sealed class AuthFields
    {
        private AuthFields()
        {

        }

        public const string AUTH_TOKEN = "X-AUTH-TOKEN";
        public const string COOKIE_SCHEME = "COOKIES";
        public const string AUTH_HEADER = "authorization";
        public const string AUTH_HEADER_TOKEN_PREFIX = "Bearer ";
    }
}
