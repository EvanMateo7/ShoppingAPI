
using System.Text.Json.Serialization;

namespace ShoppingAPI.API.Controllers
{
  public class APIResponse
  {
    [JsonPropertyName("message")]
    public string Message { get; init; }

    [JsonPropertyName("data")]
    public object Data { get; init; }
  }
}
