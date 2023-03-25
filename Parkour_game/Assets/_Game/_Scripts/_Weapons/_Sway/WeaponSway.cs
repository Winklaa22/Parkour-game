using System;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace _Game._Scripts._Weapons._Sway
{
    public class WeaponSway : MonoBehaviour
    {
        [SerializeField] private float m_smooth;
        [SerializeField] private float m_swayMultiplier;
        private InputActions m_input;

        private void Awake()
        {
            m_input = new InputActions();
            m_input.Enable();
        }

        private void Update()
        {
            var mouseVector = m_input.Player.Look.ReadValue<Vector2>();

            var mouseX = mouseVector.y * m_swayMultiplier;
            var mouseY = mouseVector.x * m_swayMultiplier;

            var rotationX = Quaternion.AngleAxis(-mouseX, Vector3.right);
            var rotationY = Quaternion.AngleAxis(mouseY, Vector3.up);

            var targetRotation = rotationX * rotationY;

            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, m_smooth * Time.deltaTime);
        }
    }
}
