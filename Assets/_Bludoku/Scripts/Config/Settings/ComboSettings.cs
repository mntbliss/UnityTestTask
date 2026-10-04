using UnityEngine;

namespace _Bludoku.Scripts.Config
{
    [CreateAssetMenu(fileName = "Combo Settings", menuName = "Data/Combo Settings")]
    public class ComboSettings : ConfigItem
    {
        [Space]
        public int MinVisibleCombo = 2;
        public int MinMultiplier = 1;

        [Space]
        public float ShowDuration = 0.8f;
        public float HideDuration = 0.2f;

        [Space]
        public float PunchDuration = 0.35f;
        public float PunchScale = 0.35f;
        public int PunchVibrato = 8;

        [Space]
        public float PunchElasticity = 0.5f;
        public float PulseScale = 1.08f;
        public float PulseDuration = 0.45f;

        [Space]
        public string ComboFormat = "Combo x{0}!";
        public string PlayerPrefsKey = "Combo";
    }
}
