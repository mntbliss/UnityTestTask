using DG.Tweening;
using UnityEngine;
using _Bludoku.Scripts.Config;

namespace _Bludoku.Scripts.Effects
{
    public class CameraBumpEffect
    {
        private readonly Transform cameraTransform;

        public CameraBumpEffect(Transform cameraTransform)
        {
            this.cameraTransform = cameraTransform;
        }

        public void Play(ComboSettings settings)
        {
            cameraTransform.DOKill(true);
            cameraTransform.DOPunchPosition(
                Vector3.up * settings.CameraBumpStrength,
                settings.CameraBumpDuration,
                settings.CameraBumpVibrato,
                settings.CameraBumpElasticity);
        }
    }
}
