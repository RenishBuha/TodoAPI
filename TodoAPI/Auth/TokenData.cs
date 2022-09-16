using System.Text.Json.Serialization;
using TodoAPI.Models;

namespace TodoAPI.Auth
{
    public class TokenData : IModel
    {
        [JsonPropertyName("userId")]
        public string UserId { get; set; }

        [JsonPropertyName("userName")]
        public string Username { get; set; }

        [JsonPropertyName("role")]
        public string Role { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("expiration")]
        public DateTime? Expiration { get; set; }

        [JsonPropertyName("ip")]
        public string IpAddress { get; set; }
    }

}
