using System;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace _Game._Scripts._Weapons._Sway
{
    public class WeaponSway : MonoBehaviour
    {
        [SerializeField] private float m_smooth;
        [SerializeField] private float m_swayMultiplier;

        private void Update()
        {
            var mouseX = Input.GetAxisRaw("Mouse Y") * m_swayMultiplier;
            var mouseY = Input.GetAxisRaw("Mouse X") * m_swayMultiplier;

            var rotationX = Quaternion.AngleAxis(-mouseX, Vector3.right);
            var rotationY = Quaternion.AngleAxis(mouseY, Vector3.up);

            var targetRotation = rotationX * rotationY;

            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, m_smooth * Time.deltaTime);
        }
    }
}
