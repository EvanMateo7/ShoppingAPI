
using System.Text.Json.Serialization;

namespace ShoppingAPI.Controllers
{
    public class APIResponse
    {
      [JsonPropertyName("message")]
      public string Message { get; init; }
      
      [JsonPropertyName("data")]
      public object Data { get; init; }
    }
}
