using System.Collections.Concurrent;
using System.Text.Json.Nodes;

namespace Helps
{
    public static class ConvertObject
    {
        public static Task<List<string>> ListJsonObjectToListString(List<JsonObject> jo)
        {
            ConcurrentBag<string> tempCase = new();
            jo.ForEach(x => tempCase.Add(x.ToJsonString()));
            return Task.FromResult(tempCase.ToList());
        }
        public static Task<List<string>> ListJsonObjectToListString(List<JsonObject> jo)
        {
            ConcurrentBag<string> tempCase = new();
            jo.ForEach(x => tempCase.Add(x.ToJsonString()));
            return Task.FromResult(tempCase.ToList());
        }
    }
}