using UnityEngine;
using _Bludoku.Scripts.Config;

namespace _Bludoku.Scripts.Save
{
    public class SaveService
    {
        private static SaveService instance;

        public SaveSettings Settings { get; private set; }

        public static SaveService Instance
        {
            get
            {
                if (instance == null) instance = new SaveService();

                return instance;
            }
        }

        private SaveService()
        {
            Settings = ConfigService.Instance.Get<SaveSettings>();
        }

        public void Save(string key, string value)
        {
            PlayerPrefs.SetString(key, value);
            PlayerPrefs.Save();
        }

        public void Save(string key, object value)
        {
            Save(key, JsonUtility.ToJson(value));
        }

        public bool Has(string key) => PlayerPrefs.HasKey(key);

        public bool Has(string key, string exactValue) => Has(key) && Get(key) == exactValue;

        public string Get(string key, string defaultValue = "")
        {
            return PlayerPrefs.GetString(key, defaultValue);
        }

        public TSave Get<TSave>(string key, TSave defaultValue = null)
            where TSave : class
        {
            string json = PlayerPrefs.GetString(key, string.Empty);
            if (string.IsNullOrEmpty(json))
                return defaultValue;

            TSave loaded = JsonUtility.FromJson<TSave>(json);
            if (loaded == null) return defaultValue;

            return loaded;
        }
    }
}

