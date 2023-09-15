using System;
using System.Collections;
using System.Collections.Generic;
using _Game._Scripts.Animations;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private Animator m_animator;
    public Animator WeaponAnimator => m_animator;
    [SerializeField] private WeaponType m_type;
    [SerializeField] private Transform m_barrel;
    [SerializeField] private GameObject m_gun;
    [SerializeField] private float m_timeToHide = .25f;
    [SerializeField] private float m_shotDistance;
    public float TimeToHide => m_timeToHide;
    public WeaponType Type => m_type;

    private void Update()
    {
        
    }

    public virtual void Show()
    {
        m_gun.SetActive(true);
        PlayerAnimationsManager.Instance.ShowGun();
    }

    public virtual void Hide()
    {
        PlayerAnimationsManager.Instance.HideGun();
        StartCoroutine(HideProcess());
    }

    private IEnumerator HideProcess()
    {
        yield return new WaitForSeconds(m_timeToHide);
        m_gun.SetActive(false);
    }

    public virtual void Shot()
    {
        
        Debug.Log("Shot");
        if (Physics.Raycast(m_barrel.position, m_barrel.forward * m_shotDistance, out var hit))
        {
            var obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            obj.transform.localScale = Vector3.one * .3f;
            obj.transform.position = hit.point;
        }
    }

    private void OnDrawGizmos()
    {
        // Gizmos.DrawRay(m_barrel.position, m_barrel.forward * 20);
    }
}
