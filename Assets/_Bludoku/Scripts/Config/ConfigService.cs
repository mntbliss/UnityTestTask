using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Bludoku.Scripts.Config
{
    public class ConfigService
    {
        private static ConfigService instance;

        private readonly List<ConfigItem> configs = new List<ConfigItem>();

        private const string CONFIGS_RESOURCE_PATH = "Configs";

        public static ConfigService Instance
        {
            get
            {
                if (instance == null) instance = new ConfigService();

                return instance;
            }
        }

        private ConfigService()
        {
            ConfigItem[] loadedConfigs = Resources.LoadAll<ConfigItem>(CONFIGS_RESOURCE_PATH);
            for (int i = 0; i < loadedConfigs.Length; i++)
            {
                configs.Add(loadedConfigs[i]);
            }
        }

        public TConfig Get<TConfig>(string customKey = "")
            where TConfig : ConfigItem
        {
            if (!string.IsNullOrEmpty(customKey)) return GetByKey<TConfig>(customKey);

            return GetByType<TConfig>();
        }

        private TConfig GetByType<TConfig>()
            where TConfig : ConfigItem
        {
            for (int i = 0; i < configs.Count; i++)
            {
                if (configs[i] is TConfig typedConfig) return typedConfig;
            }

            return null;
        }

        private TConfig GetByKey<TConfig>(string customKey)
            where TConfig : ConfigItem
        {
            for (int i = 0; i < configs.Count; i++)
            {
                ConfigItem config = configs[i];
                if (config is not TConfig typedConfig) continue;
                if (!string.Equals(config.Key, customKey, StringComparison.OrdinalIgnoreCase)) continue;

                return typedConfig;
            }

            return null;
        }
    }
}
