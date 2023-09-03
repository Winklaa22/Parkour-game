using System;
using UnityEngine;

namespace _Game._Scripts.Animations
{
    public class PlayerAnimationsManager : SceneSingleton<PlayerAnimationsManager>
    {
        [SerializeField] private Animator m_cameraAnimator;
        [SerializeField] private Animator m_pistolAnimator;
        private AnimationsHandler _cameraHandler;
        public AnimationsHandler CameraHandler => _cameraHandler;
        
        private AnimationsHandler _pistolHandler;
        public AnimationsHandler PistolHandler => _pistolHandler;

        protected override void OnAwake()
        {
            base.OnAwake();
            _cameraHandler = new AnimationsHandler(m_cameraAnimator);
            _pistolHandler = new AnimationsHandler(m_pistolAnimator);
        }
    }
}
