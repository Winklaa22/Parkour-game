using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class RagdollController : MonoBehaviour
{
    [SerializeField] private Rigidbody[] m_parts;

    private void Awake()
    {
        m_parts = GetComponentsInChildren<Rigidbody>();

        foreach (var part in m_parts)
        {
            part.AddComponent<RagdollPart>().Controller = this;
            Disable();
        }
    }

    public void Disable()
    {
        foreach (var part in m_parts)
        {
            part.isKinematic = true;
        }
    }

    public void Active()
    {
        foreach (var part in m_parts)
        {
            part.isKinematic = false;
        }
    }
}
