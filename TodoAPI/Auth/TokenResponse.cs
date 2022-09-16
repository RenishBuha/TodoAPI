using TodoAPI.Models;
using System;
using System.Text.Json.Serialization;

namespace Bethubs.Auth
{
    public class TokenResponse : IModel
    {
        [JsonPropertyName("token")]
        public string Token { get; set; }

        [JsonPropertyName("expiration")]
        public DateTime? Expiration { get; set; }
    }
}
