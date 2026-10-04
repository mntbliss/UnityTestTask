using UnityEngine;
using _Bludoku.Scripts.Analytics;

namespace _Bludoku.Scripts.Config
{
    [CreateAssetMenu(fileName = "Analytics Settings", menuName = "Data/Analytics Settings")]
    public class AnalyticsSettings : ConfigItem
    {
        [Space]
        public bool EnableAnalytics = true;
        public bool LogToConsole = true;

        [Space]
        public AnalyticsKeys Keys = new AnalyticsKeys();
    }
}
