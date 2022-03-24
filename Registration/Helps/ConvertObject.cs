using System.Text.Json.Nodes;

namespace Helps
{
    public static class ConvertObject
    {
        public static async Task<string> JsonObjectToStringAsync(JsonObject jo)
        {
            return await Task.FromResult(jo.ToJsonString());
        }
        public static async Task<JsonObject> StringToJsonObjectAsync(string st)
        {
            return await Task.FromResult(JsonNode.Parse(st).AsObject());
        }
    }
}