using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace _Bludoku.Scripts.Analytics
{
    public class ConsoleAnalyticsProvider : IAnalyticsProvider
    {
        public void Track(string eventId, string description, IReadOnlyDictionary<string, string> queryParameters)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("[Analytics] ").Append(eventId);

            if (!string.IsNullOrEmpty(description))
                stringBuilder.Append(" — ").Append(description);

            if (queryParameters != null && queryParameters.Count > 0)
            {
                stringBuilder.Append(" |");
                foreach (KeyValuePair<string, string> queryParameter in queryParameters)
                {
                    stringBuilder.Append(' ').Append(queryParameter.Key).Append('=').Append(queryParameter.Value);
                }
            }

            Debug.Log(stringBuilder.ToString());
        }

        public void SetProperty(string key, string value)
        {
            Debug.Log($"[Analytics] property {key}={value}");
        }
    }
}
