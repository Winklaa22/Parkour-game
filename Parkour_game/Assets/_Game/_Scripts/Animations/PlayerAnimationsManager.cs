using System;
using UnityEngine;

namespace _Game._Scripts.Animations
{
    public class PlayerAnimationsManager : SceneSingleton<PlayerAnimationsManager>
    {
        [SerializeField] private Animator m_cameraAnimator;
        private AnimationsHandler _cameraHandler;
        public AnimationsHandler CameraHandler => _cameraHandler;
        
        protected override void OnAwake()
        {
            base.OnAwake();
            _cameraHandler = new AnimationsHandler(m_cameraAnimator);
        }
    }
}
