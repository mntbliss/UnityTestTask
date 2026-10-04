using System.Collections.Generic;
using UnityEngine;

namespace _Bludoku.Scripts.Config
{
    public class ConfigItem : ScriptableObject
    {
        public string Key = string.Empty;

        public List<ConfigItem> Collection = new List<ConfigItem>();
    }
}
