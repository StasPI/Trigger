using System.Text.Json.Nodes;

namespace Helps
{
    public static class ConvertObject
    {
        public static Task<List<string>> ListJsonObjectToListString(List<JsonObject> jo)
        {
            return Task.FromResult(jo.AsParallel().AsOrdered().Select(x => x.ToJsonString()).ToList());
        }
        public static Task<List<JsonObject>> ListStringToJsonObject(List<string> st)
        {
            return Task.FromResult(st.AsParallel().AsOrdered().Select(x => JsonNode.Parse(x).AsObject()).ToList());
        }
    }
}