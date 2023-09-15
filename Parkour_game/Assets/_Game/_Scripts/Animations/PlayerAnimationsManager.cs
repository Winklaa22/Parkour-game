using System;
using UnityEngine;

namespace _Game._Scripts.Animations
{
    public class PlayerAnimationsManager : SceneSingleton<PlayerAnimationsManager>
    {
        [SerializeField] private Animator m_cameraAnimator;
        private Animator _fpsAnimator => WeaponsManager.Instance.GetCurrentWeapon().WeaponAnimator;
        private AnimationsHandler _cameraHandler;
        public AnimationsHandler CameraHandler => _cameraHandler;
        
        private AnimationsHandler _fpsHandler;
        public AnimationsHandler FPSHandler => _fpsHandler;

        protected override void OnAwake()
        {
            base.OnAwake();
            _cameraHandler = new AnimationsHandler(m_cameraAnimator);
        }

        protected override void OnStart()
        {
            base.OnStart();
            _fpsHandler = new AnimationsHandler(_fpsAnimator);

        }

        public void ShowGun()
        {
            _fpsHandler = new AnimationsHandler(_fpsAnimator);
            _fpsHandler.SetTrigger(PlayerAnimations.ShowTrigger);
        }

        public void HideGun()
        {
            _fpsHandler.SetTrigger(PlayerAnimations.HideTrigger);
        }
    }
}
