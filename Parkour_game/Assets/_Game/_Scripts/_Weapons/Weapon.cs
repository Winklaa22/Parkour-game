using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private Animator m_animator;
    public Animator WeaponAnimator => m_animator;
    [SerializeField] private WeaponType m_type;
    public WeaponType Type => m_type;
}
