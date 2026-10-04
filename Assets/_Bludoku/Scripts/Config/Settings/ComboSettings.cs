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

        [Space]
        public string CrumbFormat = "+{0}";
        public float CrumbFlyHeight = 2;
        public float CrumbMoveDuration = 2;
        public float CrumbFadeDuration = 2;
        public float CrumbPopScale = 1.15f;
        public float CrumbPopDuration = 0.25f;
        public int CrumbMaxCount = 6;
        public Vector2 CrumbRotationRange = new Vector2(-15f, 15f);

        [Space]
        public float CameraBumpStrength = 0.25f;
        public float CameraBumpDuration = 0.2f;
        public int CameraBumpVibrato = 20;
        public float CameraBumpElasticity = 0.5f;
    }
}
