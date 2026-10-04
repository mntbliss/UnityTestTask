using System.Collections.Generic;

namespace _Bludoku.Scripts.Analytics
{
    public interface IAnalyticsProvider
    {
        void Track(string eventId, string description, IReadOnlyDictionary<string, string> queryParameters);
        void SetProperty(string key, string value);
    }
}
