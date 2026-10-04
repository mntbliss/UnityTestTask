using System.Collections.Generic;
using _Bludoku.Scripts.Config;

namespace _Bludoku.Scripts.Analytics
{
    public class AnalyticsService
    {
        private static AnalyticsService instance;

        private readonly List<IAnalyticsProvider> providers = new List<IAnalyticsProvider>();

        public AnalyticsSettings Settings { get; private set; }

        public static AnalyticsService Instance
        {
            get
            {
                if (instance == null) instance = new AnalyticsService();

                return instance;
            }
        }

        private AnalyticsService()
        {
            Settings = ConfigService.Instance.Get<AnalyticsSettings>();

            if (Settings.LogToConsole) AddProvider(new ConsoleAnalyticsProvider());
        }

        public void AddProvider(IAnalyticsProvider provider)
        {
            providers.Add(provider);
        }

        public void Track(string eventId, string description = "", params (string name, string value)[] queryParameters)
        {
            if (!Settings.EnableAnalytics) return;

            string safeDescription = description ?? string.Empty;
            IReadOnlyDictionary<string, string> dictionary = ToDictionary(queryParameters);

            for (int i = 0; i < providers.Count; i++)
            {
                providers[i].Track(eventId, safeDescription, dictionary);
            }
        }

        public void Track(AnalyticsKey key, string description = "", params (string name, string value)[] queryParameters)
        {
            string resolvedDescription = string.IsNullOrEmpty(description) ? key.Description ?? string.Empty : description;
            Track(key.Id, resolvedDescription, queryParameters);
        }

        public void SetProperty(string key, string value)
        {
            if (!Settings.EnableAnalytics) return;

            for (int i = 0; i < providers.Count; i++)
            {
                providers[i].SetProperty(key, value);
            }
        }

        public void SetProperty(AnalyticsKey key, string value) => SetProperty(key.Id, value);

        private static IReadOnlyDictionary<string, string> ToDictionary((string name, string value)[] queryParameters)
        {
            if (queryParameters == null || queryParameters.Length == 0) return null;

            Dictionary<string, string> dictionary = new Dictionary<string, string>(queryParameters.Length);
            for (int i = 0; i < queryParameters.Length; i++)
            {
                dictionary[queryParameters[i].name] = queryParameters[i].value ?? string.Empty;
            }

            return dictionary;
        }
    }
}
