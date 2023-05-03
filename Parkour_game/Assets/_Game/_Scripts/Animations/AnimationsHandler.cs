using UnityEngine;

namespace _Game._Scripts.Animations
{
    public class AnimationsHandler
    {
        private Animator m_animator;

        public AnimationsHandler(Animator animator)
        {
            m_animator = animator;
        }

        public void SetBool(string name, bool value)
        {
            if (m_animator is null)
            {
                Debug.Log("Animator is null");
                return;
            }
            
            m_animator.SetBool(name, value);
        }

        public void SetTrigger(string name)
        {
            if (m_animator is null)
            {
                Debug.Log("Animator is null");
                return;
            }
            
            Debug.Log(name);
            m_animator.SetTrigger(name);
        }

        public void SetFloat(string name, float value)
        {
            if (m_animator is null)
            {
                Debug.Log("Animator is null");
                return;
            }
            
            m_animator.SetFloat(name, value);
        }
        
        public void SetInt(string name, int value)
        {
            if (m_animator is null)
            {
                Debug.Log("Animator is null");
                return;
            }
            
            m_animator.SetInteger(name, value);
        }

        public void ChangeAnimator(Animator animator)
        {
            m_animator = animator;
        }
    }
}
