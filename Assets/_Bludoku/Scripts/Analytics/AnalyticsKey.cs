using System;

namespace _Bludoku.Scripts.Analytics
{
    [Serializable]
    public class AnalyticsKey
    {
        public string Id = string.Empty;
        public string Description = string.Empty;

        public AnalyticsKey()
        {
        }

        public AnalyticsKey(string id, string description = "")
        {
            Id = id;
            Description = description;
        }
    }
}
